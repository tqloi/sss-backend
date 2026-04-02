using FastEndpoints;
using FluentValidation;
using SSS.Application.Features.Content.ManagerStats;

namespace SSS.Web.Endpoints.Content.ManagerStats
{
    public sealed class GetContentManagerStatsValidator : Validator<GetContentManagerStatsQuery>
    {
        public GetContentManagerStatsValidator()
        {
            RuleFor(x => x.SubjectId)
                .GreaterThan(0)
                .WithMessage("SubjectId must be greater than 0.")
                .When(x => x.SubjectId.HasValue);
        }
    }
}
