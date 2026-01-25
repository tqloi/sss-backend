using FastEndpoints;
using MediatR;
using SSS.Application.Features.Surveys.SurveyQuestionOptions.CreateSurveyQuestionOption;

namespace SSS.Web.Endpoints.Surveys.SurveyQuestionOptions.CreateSurveyQuestionOption
{
    public class CreateSurveyQuestionOptionEndpoint(ISender sender) : Endpoint<CreateSurveyQuestionOptionCommand, CreateSurveyQuestionOptionResponse>
    {
      
            public override void Configure()
            {
                Post("api/surveys/question/option");
                Description(d => d.WithTags("Survey Options"));
                Summary(s => s.Summary = "Create new survey question option");
                Roles("Admin");
            }

            public override async Task HandleAsync(CreateSurveyQuestionOptionCommand req, CancellationToken ct)
                => await SendAsync(await sender.Send(req, ct), cancellation: ct);
        
    }
}
