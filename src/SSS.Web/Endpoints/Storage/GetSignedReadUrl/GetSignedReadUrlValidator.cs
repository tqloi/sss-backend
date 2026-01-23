using FastEndpoints;
using FluentValidation;

namespace SSS.WebApi.Endpoints.Storage.GetSignedReadUrl
{
    public sealed class GetSignedReadUrlValidator : Validator<GetSignedReadUrlRequest>
    {
        public GetSignedReadUrlValidator()
        {
            RuleFor(x => x.ObjectName).NotEmpty();
            RuleFor(x => x.TtlSeconds).GreaterThan(0).LessThanOrEqualTo(3600);
        }
    }
}
