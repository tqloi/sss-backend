using FastEndpoints;
using MediatR;
using SSS.Application.Features.UserGamifications.RecordDailyLogin;
using System.Security.Claims;

namespace SSS.Web.Endpoints.UserGamifications.RecordDailyLogin
{
    public class RecordDailyLoginEndpoint(IMediator mediator) : EndpointWithoutRequest<RecordDailyLoginResult>
    {
        public override void Configure()
        {
            Post("/api/gamification/record-login");
            Summary(s => {
                s.Summary = "Record Daily Login for Streak Tracking";
                s.Description = "Evaluates the user's login date, increments the daily streak, and returns the updated gamification stats.";
            });
            Roles("User");
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                await SendUnauthorizedAsync(ct);
                return;
            }

            var command = new RecordDailyLoginCommand
            {
                UserId = userId
            };

            var result = await mediator.Send(command, ct);

            await SendOkAsync(result, ct);
        }
    }
}
