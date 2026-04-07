using FastEndpoints;
using FluentValidation;

namespace SSS.Web.Endpoints.StudyPlans.StudyPlans.GetStudyPlanByRoadmapId
{
    public class GetStudyPlanByRoadmapIdValidator : Validator<GetStudyPlanByRoadmapIdRequest>
    {
        public GetStudyPlanByRoadmapIdValidator()
        {
            RuleFor(x => x.RoadmapId)
                .GreaterThan(0)
                .WithMessage("Invalid Roadmap ID");
        }
    }
}
