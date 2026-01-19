using FastEndpoints;
using MediatR;
using SSS.Application.Abstractions.External.Communication.Email;
using SSS.Application.Features.Auth.Register;
using SSS.WebApi.Endpoints.Auth.ConfirmEmail;

namespace SSS.WebApi.Endpoints.Auth.Register
{
    public sealed class RegisterEndpoint(
        ISender sender,
        ISmtpEmailSender mail,
        IMailTemplateBuilder mailTpl,
        ILogger<RegisterEndpoint> logger,
        AutoMapper.IMapper mapper        
        )
        : Endpoint<RegisterRequest, RegisterResponse>
    {
        public override void Configure()
        {
            Post("/api/auth/register");
            AllowAnonymous();
            Description(d => d.WithTags("Auth"));
        }

        public override async Task HandleAsync(RegisterRequest req, CancellationToken ct)
        {
            var cmd = mapper.Map<RegisterCommand>(req);
            var result = await sender.Send(cmd, ct);

            var reqHttp = HttpContext.Request;
            var scheme = reqHttp.Scheme;
            var host = reqHttp.Host.Value;
            var basePath = reqHttp.PathBase.Value;
            var feBaseUrl = Config["Frontend:BaseUrl"]!.TrimEnd('/');

            var confirmUrl = $"{feBaseUrl}/confirm-email"
               + $"?userId={Uri.EscapeDataString(result.UserId)}"
               + $"&token={Uri.EscapeDataString(result.EncodedEmailConfirmToken)}"
               + (string.IsNullOrEmpty(req.ReturnUrl) ? "" : $"&returnUrl={Uri.EscapeDataString(req.ReturnUrl)}");

            // render template + gửi mail
            var body = await mailTpl.BuildConfirmEmailAsync(
                confirmationUrl: confirmUrl!,
                email: result.Email);

            await mail.SendMailAsync(new EmailContent
            {
                To = result.Email,
                Subject = "Confirm Otp SSS",
                Body = body
            });

            logger.LogInformation("User registered, confirmation email sent.");

            await SendOkAsync(new RegisterResponse
            {

                Message = "Registration successful. Please check your email to confirm your account."
                //ConfirmUrl = confirmUrl
            },
            ct);
        }
    }
}