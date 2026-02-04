using FastEndpoints;
using MediatR;
using SSS.Application.Features.Surveys.SurveyFieldSemantics.DeleteSurveyFieldSemantic;
using SSS.Application.Features.Surveys.SurveyQuestionOptions.DeleteSurveyQuestionOption;

namespace SSS.Web.Endpoints.Surveys.SurveyFieldSemantics.DeleteSurveyFieldSemantic
{
    public class DeleteSurveyFieldSemanticEndpoint(ISender sender): EndpointWithoutRequest<DeleteSurveyFieldSemanticResponse>
    {
        public override void Configure()
        {
            Delete("api/surveys/question/surveyfieldsemantic/{id}");
            Description(d => d.WithTags("SurveyFieldSemantics"));
            Summary(s =>
            {
                s.Summary = "Delete a survey field semantic  by id";
            });
            Roles("Analyst");
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            var id = Route<int>("id");
            var response = await sender.Send(new DeleteSurveyFieldSemanticCommand(id), ct);
            await SendAsync(response, cancellation: ct);
        }
    }
}
