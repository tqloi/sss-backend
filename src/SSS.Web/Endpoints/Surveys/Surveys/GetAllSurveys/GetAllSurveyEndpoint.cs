using FastEndpoints;
using MediatR;
using SSS.Application.Features.Surveys.Surveys.GetAllSurvey;

namespace SSS.Web.Endpoints.Surveys.Surveys.GetAllSurveys
{
    public class GetAllSurveyEndpoint(ISender sender): Endpoint<GetAllSurveyQuery,GetAllSurveyResult>
    {
        public override void Configure()
        {
            Get("/api/surveys/all");
            Description(d => d.WithTags("Surveys"));
            Summary(s => s.Summary = "Get all surveys (paginated)");
            
            Roles("Admin");
        }

        public override async Task HandleAsync(GetAllSurveyQuery req, CancellationToken ct)
        {
            var result = await sender.Send(req, ct);
            await SendOkAsync(result, ct);
        }
    }
}
