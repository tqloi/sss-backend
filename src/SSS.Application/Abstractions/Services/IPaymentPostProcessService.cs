namespace SSS.Application.Abstractions.Services
{
    public interface IPaymentPostProcessService
    {
        Task HandlePaymentSuccessAsync(long paymentId, CancellationToken ct = default);
    }
}