using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SSS.Application.Abstractions.Background;
using SSS.Application.Abstractions.External.Communication.Email;
using SSS.Application.Abstractions.External.Document.Pdf;
using SSS.Application.Abstractions.External.Storage.Gcs;
using SSS.Application.Abstractions.Persistence.Sql;
using SSS.Application.Abstractions.Services;
using SSS.Domain.Enums;

namespace SSS.Infrastructure.Services
{
    public class PaymentPostProcessService(
        IAppDbContext db,
        IPdfService pdfService,
        IGcsStorageService gcsStorageService,
        IMailTemplateBuilder mailTemplateBuilder,
        IEmailJobDispatcher emailJobDispatcher,
        ILogger<PaymentPostProcessService> logger) : IPaymentPostProcessService
    {
        public async Task HandlePaymentSuccessAsync(long paymentId, CancellationToken ct = default)
        {
            var payment = await db.UserPayments
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == paymentId, ct);

            if (payment is null)
            {
                logger.LogWarning("[PaymentPostProcessService] Payment not found. PaymentId={PaymentId}", paymentId);
                return;
            }

            if (payment.Status != PaymentStatus.Success)
            {
                logger.LogInformation("[PaymentPostProcessService] Skip post-process because payment is not success. PaymentId={PaymentId}, Status={Status}", paymentId, payment.Status);
                return;
            }

            var user = await db.Users
                .AsNoTracking()
                .Where(u => u.Id == payment.UserId)
                .Select(u => new
                {
                    u.Email,
                    u.FirstName,
                    u.LastName,
                    u.UserName
                })
                .FirstOrDefaultAsync(ct);

            if (user is null || string.IsNullOrWhiteSpace(user.Email))
            {
                logger.LogWarning("[PaymentPostProcessService] User/email not found. PaymentId={PaymentId}, UserId={UserId}", paymentId, payment.UserId);
                return;
            }

            var displayName = string.Join(" ", new[] { user.FirstName, user.LastName }
                .Where(x => !string.IsNullOrWhiteSpace(x))).Trim();

            if (string.IsNullOrWhiteSpace(displayName))
                displayName = user.UserName ?? "Learner";

            var invoiceNumber = $"INV-{payment.Id:D8}";
            var invoiceDate = FormatInvoiceDateForVietnam(payment.PaymentDate);
            var packageName = BuildPackageName(payment.SubscriptionType, payment.SubscriptionDuration);
            var amount = $"{payment.Amount:N0} {payment.Currency}";
            var transactionId = $"PAYOS-{payment.Id}";

            using var invoicePdf = await pdfService.GenerateInvoceAsync(
                invoiceNumber: invoiceNumber,
                invoiceDate: invoiceDate,
                customerName: displayName,
                customerEmail: user.Email,
                packageName: packageName,
                amount: amount,
                paymentMethod: "PayOS",
                transactionId: transactionId,
                invoiceUrl: string.Empty);

            var objectName = $"invoices/{DateTime.UtcNow:yyyy/MM}/{invoiceNumber}.pdf";
            var uploadedObjectName = await gcsStorageService.UploadAsync(
                stream: invoicePdf,
                objectName: objectName,
                contentType: "application/pdf",
                ct: ct);

            var invoiceUrl = gcsStorageService.GetPublicUrl(uploadedObjectName);

            var emailBody = await mailTemplateBuilder.BuildPremiumUpgradeEmailAsync(
                studentName: displayName,
                packageName: packageName,
                invoiceNumber: invoiceNumber,
                invoiceDate: invoiceDate,
                invoiceUrl: invoiceUrl,
                email: user.Email);

            emailJobDispatcher.DispatchSendEmail(
                to: user.Email,
                subject: "StudySense - Premium upgrade successful",
                body: emailBody);

            logger.LogInformation("[PaymentPostProcessService] Completed post-payment flow. PaymentId={PaymentId}, InvoiceUrl={InvoiceUrl}", paymentId, invoiceUrl);
        }

        private static string BuildPackageName(SubscriptionType subscriptionType, int durationMonths)
        {
            var monthText = durationMonths == 1 ? "1 month" : $"{durationMonths} months";
            return $"{subscriptionType} - {monthText}";
        }

        private static string FormatInvoiceDateForVietnam(DateTime paymentDate)
        {
            var utcDate = DateTime.SpecifyKind(paymentDate, DateTimeKind.Utc);
            var vietnamTimeZone = ResolveVietnamTimeZone();
            var vietnamDate = TimeZoneInfo.ConvertTimeFromUtc(utcDate, vietnamTimeZone);
            return vietnamDate.ToString("dd/MM/yyyy HH:mm");
        }

        private static TimeZoneInfo ResolveVietnamTimeZone()
        {
            try
            {
                return TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
            }
            catch (TimeZoneNotFoundException)
            {
                try
                {
                    return TimeZoneInfo.FindSystemTimeZoneById("Asia/Bangkok");
                }
                catch (TimeZoneNotFoundException)
                {
                    return TimeZoneInfo.Utc;
                }
                catch (InvalidTimeZoneException)
                {
                    return TimeZoneInfo.Utc;
                }
            }
            catch (InvalidTimeZoneException)
            {
                return TimeZoneInfo.Utc;
            }
        }
    }
}