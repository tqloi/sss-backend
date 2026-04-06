using iText.Html2pdf;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using SSS.Application.Abstractions.External.Document.Pdf;
using System.Reflection;
using System.Text;

namespace SSS.Infrastructure.External.Document.Pdf
{
    public sealed class PdfService : IPdfService
    {
        private const string TemplateInvoice = "InvoiceTemplate.html";

        public async Task<Stream> GenerateInvoceAsync(
            string invoiceNumber,
            string invoiceDate,
            string customerName,
            string customerEmail,
            string packageName,
            string amount,
            string paymentMethod,
            string transactionId,
            string invoiceUrl)
        {
            var asm = Assembly.GetExecutingAssembly();
            var resourcePath = $"{typeof(PdfService).Namespace}.Templates.{TemplateInvoice}";

            using var resourceStream = asm.GetManifestResourceStream(resourcePath)
                ?? throw new FileNotFoundException($"Invoice template not found: {resourcePath}");

            using var reader = new StreamReader(resourceStream, Encoding.UTF8);
            var html = await reader.ReadToEndAsync();

            html = html
                .Replace("{{ .InvoiceNumber }}", invoiceNumber)
                .Replace("{{ .InvoiceDate }}", invoiceDate)
                .Replace("{{ .CustomerName }}", customerName)
                .Replace("{{ .CustomerEmail }}", customerEmail)
                .Replace("{{ .PackageName }}", packageName)
                .Replace("{{ .Amount }}", amount)
                .Replace("{{ .PaymentMethod }}", paymentMethod)
                .Replace("{{ .TransactionId }}", transactionId)
                .Replace("{{ .InvoiceUrl }}", invoiceUrl);

            var output = new MemoryStream();
            using var htmlStream = new MemoryStream(Encoding.UTF8.GetBytes(html));

            var props = new ConverterProperties();

            var writerProps = new WriterProperties()
                .SetCompressionLevel(CompressionConstants.BEST_COMPRESSION)
                .SetFullCompressionMode(true);
            using (var writer = new PdfWriter(output, writerProps))
            {
                writer.SetCloseStream(false);
                using var pdfDoc = new PdfDocument(writer);
                pdfDoc.SetDefaultPageSize(PageSize.A4);
                HtmlConverter.ConvertToPdf(htmlStream, pdfDoc, props);
            }

            output.Position = 0;
            return output;
        }
    }
}