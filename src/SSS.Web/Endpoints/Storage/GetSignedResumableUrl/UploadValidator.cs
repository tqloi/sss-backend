using FastEndpoints;
using FluentValidation;

namespace SSS.WebApi.Endpoints.Storage.GetSignedResumableUrl
{
    public sealed class GetSignedResumableUrlValidator : Validator<GetSignedResumableUrlRequest>
    {
        public GetSignedResumableUrlValidator()
        {
            RuleFor(x => x.ObjectName).NotEmpty();
            RuleFor(x => x.ContentType).NotEmpty();
            RuleFor(x => x.TtlSeconds).GreaterThan(0).LessThanOrEqualTo(3600);
        }
    }
}
