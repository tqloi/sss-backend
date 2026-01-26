using FastEndpoints;
using MediatR;
using SSS.Application.Features.Surveys.SurveyQuestionOptions.DeleteSurveyQuestionOption;

namespace SSS.Web.Endpoints.Surveys.SurveyQuestionOptions.DeleteSurveyQuestionOption
{
    public class DeleteSurveyQuestionOptionEndpoint(ISender sender) : EndpointWithoutRequest<DeleteSurveyQuestionOptionResponse>
    {
        public override void Configure()
        {
            Delete("api/surveys/question/option/{id}");
            Description(d => d.WithTags("SurveyOptions"));
            Summary(s =>
            {
                s.Summary = "Delete a survey question option by id";
            });
            Roles("Admin");
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            var id = Route<int>("id");
            var response = await sender.Send(new DeleteSurveyQuestionOptionCommand(id), ct);
            await SendAsync(response, cancellation: ct);
        }
    }
}
