using FastEndpoints;
using MediatR;
using SSS.Application.Features.Surveys.SurveyFieldSemantics.EditSurveyFieldSemantic;
using SSS.Application.Features.Surveys.SurveyQuestionOptions.EditSurveyQuestionOption;

namespace SSS.Web.Endpoints.Surveys.SurveyFieldSemantics.EditSurveyFieldSemantic
{
    public class EditSurveyFieldSemanticEndpoint(ISender sender):Endpoint<EditSurveyFieldSemanticCommand, EditSurveyFieldSemanticResponse>
    {
        public override void Configure()
    {
        Patch("/api/surveys/question/surveyfieldsemantic");
        Description(d => d.WithTags("SurveyFieldSemantics"));
        Summary(s => s.Summary = "Edit an SurveyFieldSemantic");
        Roles("Analyst");
    }
    public override async Task HandleAsync(EditSurveyFieldSemanticCommand req, CancellationToken ct)
        => await SendAsync(await sender.Send(req, ct), cancellation: ct);
    }

}
