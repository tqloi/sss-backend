namespace SSS.Application.Abstractions.External.Document.Pdf
{
    public interface IPdfService
    {
        Task<Stream> GenerateInvoceAsync(
            string invoiceNumber,
            string invoiceDate,
            string customerName,
            string customerEmail,
            string packageName,
            string amount,
            string paymentMethod,
            string transactionId,
            string invoiceUrl);
    }
}