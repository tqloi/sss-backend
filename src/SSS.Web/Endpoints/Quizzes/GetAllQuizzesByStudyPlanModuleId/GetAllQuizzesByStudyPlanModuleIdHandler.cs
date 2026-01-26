using FastEndpoints;
using MediatR;
using SSS.Application.Features.Quizzes.GetAllQuizzesByStudyPlanModuleId;

namespace SSS.Web.Endpoints.Quizzes.GetAllQuizzesByStudyPlanModuleId
{
    public class GetAllQuizzesByStudyPlanModuleIdHandler(ISender sender)
        : Endpoint<GetAllQuizzesByStudyPlanModuleIdRequest, GetAllQuizzesByStudyPlanModuleIdResponse>
    {
        public override void Configure()
        {
            Get("/api/quizzes/studyplanmodule/{StudyPlanModuleId}");
            Summary(s => s.Summary = "Get all quizzes by Study Plan Module Id with pagination");
            Roles("Admin", "User");
        }
        public override async Task HandleAsync(GetAllQuizzesByStudyPlanModuleIdRequest req, CancellationToken ct)
            => await SendAsync(await sender.Send(req, ct), cancellation: ct);
    }
}
