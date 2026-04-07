using FastEndpoints;
using MediatR;
using SSS.Application.Features.Surveys.SurveyFieldSemantics.CreateSurveyFieldSemantic;
using SSS.Application.Features.Surveys.SurveyQuestionOptions.CreateSurveyQuestionOption;

namespace SSS.Web.Endpoints.Surveys.SurveyFieldSemantics.CreateSurveyFieldSemantic
{
    public class CreateSurveyFieldSemanticEndpoint(ISender sender): Endpoint<CreateSurveyFieldSemanticCommand,CreateSurveyFieldSemanticResponse>
    {
        public override void Configure()
        {
            Post("api/surveys/question/surveyfieldsemantic");
            Description(d => d.WithTags("SurveyFieldSemantics"));
            Summary(s => s.Summary = "Create new survey question option");
            Roles("Analyst");
        }

        public override async Task HandleAsync(CreateSurveyFieldSemanticCommand req, CancellationToken ct)
            => await SendAsync(await sender.Send(req, ct), cancellation: ct);
    }
}
