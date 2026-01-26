using FastEndpoints;
using MediatR;
using SSS.Application.Features.Quizzes.GetAllQuizzesByRoadmapNodeId;

namespace SSS.Web.Endpoints.Quizzes.GetAllQuizzesByStudyPlanModuleId
{
    public class GetAllQuizzesByRoadmapNodeIdHandler(ISender sender)
        : Endpoint<GetAllQuizzesByRoadmapNodeIdQuery, GetAllQuizzesByRoadmapNodeIdResult>
    {
        public override void Configure()
        {
            Get("/api/quizzes/studyplanmodule/{StudyPlanModuleId}");
            Summary(s => s.Summary = "Get all quizzes by Study Plan Module Id with pagination");
            Description(d => d.WithTags("Quizzes"));
            Roles("Admin", "User");
        }
        public override async Task HandleAsync(GetAllQuizzesByRoadmapNodeIdQuery req, CancellationToken ct)
            => await SendAsync(await sender.Send(req, ct), cancellation: ct);
    }
}
