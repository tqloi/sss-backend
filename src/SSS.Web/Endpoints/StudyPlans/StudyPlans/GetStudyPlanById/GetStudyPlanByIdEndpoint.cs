using FastEndpoints;
using MediatR;
using SSS.Application.Features.StudyPlans.StudyPlans.GetStudyPlanById;

namespace SSS.Web.Endpoints.StudyPlans.StudyPlans.GetStudyPlanById
{
    public class GetStudyPlanByIdEndpoint(ISender sender) 
        : Endpoint<GetStudyPlanByIdRequest, GetStudyPlanByIdResult>
    {
        public override void Configure()
        {
            Get("/api/study-plans/{StudyPlanId}");
            Description(d => d.WithTags("StudyPlans"));
            Summary(s => s.Summary = "Get study plan by ID");
        }

        public override async Task HandleAsync(GetStudyPlanByIdRequest req, CancellationToken ct)
        {
            var studyPlanId = Route<long>("StudyPlanId");
            
            var query = new GetStudyPlanByIdQuery
            {
                StudyPlanId = studyPlanId
            };

            var result = await sender.Send(query, ct);
            await SendOkAsync(result, ct);
        }
    }
}
