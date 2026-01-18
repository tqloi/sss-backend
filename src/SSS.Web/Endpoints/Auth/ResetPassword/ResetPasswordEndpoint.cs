using FastEndpoints;
using SSS.Application.Features.Auth.ResetPassword;
using MediatR;

namespace SSS.WebApi.Endpoints.Auth.ResetPassword
{
    public sealed class ResetPasswordEndpoint(
        AutoMapper.IMapper mapper,
        ISender sender)
      : Endpoint<ResetPasswordRequest>
    {
        public const string Route = "/api/auth/reset-password";

        public override void Configure()
        {
            Post(Route);
            AllowAnonymous();
            Description(d => d.WithTags("Auth"));
        }
        public override async Task HandleAsync(ResetPasswordRequest req, CancellationToken ct)
        {
            var command = mapper.Map<ResetPasswordCommand>(req);
            await sender.Send(command, ct);
            await SendOkAsync(new { message = "Your password has been reset successfully." }, ct);
        }
    }
}
