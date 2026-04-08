using FastEndpoints;
using MediatR;
using SSS.Application.Features.AdminAnalytics.GetOverview;

namespace SSS.Web.Endpoints.Admin.Analytics.GetOverview
{
    public sealed class GetAdminDashboardOverviewEndpoint(ISender sender)
        : EndpointWithoutRequest<GetAdminDashboardOverviewResult>
    {
        public override void Configure()
        {
            Get("/api/admin/analytics/overview");
            Roles("Admin");
            Description(d => d.WithTags("AdminAnalytics"));
            Summary(s => s.Summary = "Get admin dashboard overview metrics");
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            var result = await sender.Send(new GetAdminDashboardOverviewQuery(), ct);
            await SendOkAsync(result, ct);
        }
    }
}
