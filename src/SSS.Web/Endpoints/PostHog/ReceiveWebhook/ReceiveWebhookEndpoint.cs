using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using FastEndpoints;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SSS.Application.Features.PostHog.ReceiveWebhook;

namespace SSS.Web.Endpoints.PostHog.ReceiveWebhook
{
    public class ReceiveWebhookEndpoint : EndpointWithoutRequest
    {
        private readonly ISender _sender;
        private readonly IConfiguration _config;
        private readonly ILogger<ReceiveWebhookEndpoint> _logger;

        public ReceiveWebhookEndpoint(ISender sender, IConfiguration config, ILogger<ReceiveWebhookEndpoint> logger)
        {
            _sender = sender;
            _config = config;
            _logger = logger;
        }

        public override void Configure()
        {
            Post("/api/posthog/webhook");
            AllowAnonymous();
            Description(d => d.WithTags("Tracking"));
            Summary(s => s.Summary = "Receive events from PostHog Webhook");
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            var secret = _config["PostHog:WebhookSecret"];
            if (string.IsNullOrEmpty(secret))
            {
                _logger.LogWarning("PostHog webhook secret is not configured.");
                ThrowError("Server configuration error.");
                return;
            }

            HttpContext.Request.EnableBuffering();
            using var reader = new StreamReader(HttpContext.Request.Body, Encoding.UTF8, leaveOpen: true);
            var rawBody = await reader.ReadToEndAsync(ct);
            HttpContext.Request.Body.Position = 0;

            var signatureHeader = HttpContext.Request.Headers["X-PostHog-Signature"].ToString();
            if (!VerifySignature(rawBody, signatureHeader, secret))
            {
                _logger.LogWarning("Invalid PostHog webhook signature.");
                await SendUnauthorizedAsync(ct);
                return;
            }

            try
            {
                // Parse raw JSON to JsonElement to pass to Application layer
                using var jsonDoc = JsonDocument.Parse(rawBody);
                var payload = jsonDoc.RootElement.Clone();

                var command = new ReceivePostHogWebhookCommand
                {
                    RawPayload = payload
                };

                await _sender.Send(command, ct);
                await SendOkAsync(new { ok = true }, ct);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to process PostHog webhook");
                throw;
            }
        }

        private static bool VerifySignature(string body, string signature, string secret)
        {
            if (string.IsNullOrEmpty(signature)) return false;

            var expectedBytes = Encoding.UTF8.GetBytes(secret);
            var actualBytes = Encoding.UTF8.GetBytes(signature);

            if (expectedBytes.Length != actualBytes.Length)
                return false;

            return CryptographicOperations.FixedTimeEquals(expectedBytes, actualBytes);
        }
    }
}
