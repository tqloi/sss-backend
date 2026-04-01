using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SSS.Application.Abstractions.External.Payment.PayOS;
using SSS.Application.Abstractions.Persistence.Sql;
using SSS.Application.Common.Exceptions;
using SSS.Application.Features.Payments.Common;
using SSS.Domain.Entities.Payment;
using SSS.Domain.Enums;

namespace SSS.Application.Features.Payments.CreatePayment;

public sealed class CreatePaymentHandler(
    IAppDbContext context,
    IPayOsGateway payOsGateway,
    IConfiguration config
) : IRequestHandler<CreatePaymentCommand, CreatePaymentResult>
{
    public async Task<CreatePaymentResult> Handle(CreatePaymentCommand req, CancellationToken ct)
    {
        if (req.SubscriptionType == SubscriptionType.Free)
        {
            throw new ConflictException("Free subscription does not require payment");
        }

        //var amount = ResolveAmount(req.SubscriptionType, req.Amount);
        var payment = new UserPayment
        {
            UserId = req.UserId,
            SubscriptionType = req.SubscriptionType,
            Amount = req.Amount,
            Currency = "VND",
            Status = PaymentStatus.Pending,
            PaymentDate = DateTime.UtcNow,
        };

        context.UserPayments.Add(payment);
        await context.SaveChangesAsync(ct);

        var returnUrl = string.IsNullOrWhiteSpace(req.ReturnUrl)
            ? config["PayOS:ReturnUrl"] ?? throw new InvalidOperationException("PayOS:ReturnUrl is missing")
            : req.ReturnUrl;

        var cancelUrl = string.IsNullOrWhiteSpace(req.CancelUrl)
            ? config["PayOS:CancelUrl"] ?? throw new InvalidOperationException("PayOS:CancelUrl is missing")
            : req.CancelUrl;

        var description = BuildDescription(req.Description, req.SubscriptionType, payment.Id);

        var paymentLink = await payOsGateway.CreatePaymentLinkAsync(
            new PayOsCreatePaymentRequest
            {
                OrderCode = payment.Id,
                Amount = (int)payment.Amount,
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
                OrderCode = payment.Id,
                Amount = (int) payment.Amount,
                SubscriptionType = req.SubscriptionType,
                CheckoutUrl = paymentLink.CheckoutUrl,
                PaymentLinkId = paymentLink.PaymentLinkId,
                QrCode = paymentLink.QrCode,
            },
        };
    }

    private static decimal ResolveAmount(SubscriptionType type, decimal? amount)
    {
        if (amount.HasValue && amount.Value > 0)
        {
            return amount.Value;
        }

        return type switch
        {
            SubscriptionType.Premium => 99000m,
            SubscriptionType.Pro => 199000m,
            _ => 0m,
        };
    }

    private static string BuildDescription(string? customDescription, SubscriptionType type, long paymentId)
    {
        var raw = string.IsNullOrWhiteSpace(customDescription)
            ? $"{type} subscription #{paymentId}"
            : customDescription.Trim();

        // PayOS description length should be short and ASCII-safe.
        return raw.Length <= 25 ? raw : raw[..25];
    }
}
