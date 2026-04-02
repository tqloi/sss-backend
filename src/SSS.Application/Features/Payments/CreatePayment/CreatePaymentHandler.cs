using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SSS.Application.Abstractions.External.Payment.PayOS;
using SSS.Application.Abstractions.Persistence.Sql;
using SSS.Application.Common.Exceptions;
using SSS.Application.Features.Payments.Common;
using SSS.Domain.Constants;
using SSS.Domain.Entities.Payment;
using SSS.Domain.Enums;

namespace SSS.Application.Features.Payments.CreatePayment;

public sealed class CreatePaymentHandler(
    IAppDbContext context,
    IPayOsGateway payOsGateway,
    IConfiguration config,
    IMapper mapper
) : IRequestHandler<CreatePaymentCommand, CreatePaymentResult>
{
    public async Task<CreatePaymentResult> Handle(CreatePaymentCommand req, CancellationToken ct)
    {
        if (req.SubscriptionType == SubscriptionType.Free)
        {
            throw new ConflictException("Free subscription does not require payment");
        }

        var validDurations = new[] {
            PaymentConstants.SubscriptionDuration.OneMonth,
            PaymentConstants.SubscriptionDuration.SixMonths
        };

        if (!validDurations.Contains(req.SubscriptionDuration))
        {
            throw new ConflictException($"Invalid subscription duration. Valid options: {string.Join(", ", validDurations)}");
        }

        var payment = mapper.Map<UserPayment>(req);
        payment.Amount = PaymentConstants.GetSubscriptionAmount(req.SubscriptionType, req.SubscriptionDuration);

        context.UserPayments.Add(payment);
        await context.SaveChangesAsync(ct);

        var returnUrl = string.IsNullOrWhiteSpace(req.ReturnUrl)
            ? config["PayOS:ReturnUrl"] ?? throw new InvalidOperationException("PayOS:ReturnUrl is missing")
            : req.ReturnUrl;

        var cancelUrl = string.IsNullOrWhiteSpace(req.CancelUrl)
            ? config["PayOS:CancelUrl"] ?? throw new InvalidOperationException("PayOS:CancelUrl is missing")
            : req.CancelUrl;

        var description = BuildDescription(req.SubscriptionType, payment.Id);

        var paymentLink = await payOsGateway.CreatePaymentLinkAsync(
            new PayOsCreatePaymentRequest
            {
                OrderCode = payment.Id,
                Amount = (int) payment.Amount,
                Description = description,
                ReturnUrl = returnUrl,
                CancelUrl = cancelUrl,
            },
            ct
        );

        return new CreatePaymentResult
        {
            Success = true,
            Message = "Payment link created successfully",
            Data = new CreatePaymentDto
            {
                PaymentId = payment.Id,
                Amount = payment.Amount,
                SubscriptionType = payment.SubscriptionType,
                SubscriptionDuration = payment.SubscriptionDuration,
                CheckoutUrl = paymentLink.CheckoutUrl,
            },
        };
    }

    private static string BuildDescription(SubscriptionType type, long paymentId)
    {
        // Format ngắn gọn, đủ thông tin
        var raw = $"{type} #{paymentId}";

        // Ensure ASCII-safe (PayOS khuyến nghị)
        raw = new string(raw.Where(c => c <= 127).ToArray());

        // Limit 25 chars
        return raw.Length <= 25 ? raw : raw.Substring(0, 25);
    }
}
