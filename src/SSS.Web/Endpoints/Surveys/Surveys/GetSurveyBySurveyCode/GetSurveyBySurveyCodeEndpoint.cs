using FastEndpoints;
using MediatR;
using SSS.Application.Features.Surveys.Surveys.GetSurveyBySurveyCode;

namespace SSS.Web.Endpoints.Surveys.Surveys.GetSurveyBySurveyCode;

public class GetSurveyBySurveyCodeEndpoint(ISender sender) 
    : EndpointWithoutRequest<GetSurveyBySurveyCodeResult>
{
    public override void Configure()
    {
        Get("/api/surveys/code/{surveyCode}");
        Description(d => d.WithTags("Surveys"));
        Summary(s => s.Summary = "Get survey by survey code");
        
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var surveyCode = Route<string>("surveyCode");

        var query = new GetSurveyBySurveyCodeQuery(surveyCode);
        var result = await sender.Send(query, ct);

        if (!result.Success)
        {
            await SendNotFoundAsync(ct);
            return;
        }

        await SendOkAsync(result, ct);
    }
}