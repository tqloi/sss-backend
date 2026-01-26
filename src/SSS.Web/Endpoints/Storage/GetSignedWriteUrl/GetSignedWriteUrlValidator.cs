using FastEndpoints;
using FluentValidation;

namespace SSS.WebApi.Endpoints.Storage.GetSignedWriteUrl
{
    public sealed class GetSignedWriteUrlValidator : Validator<GetSignedWriteUrlRequest>
    {
        public GetSignedWriteUrlValidator()
        {
            RuleFor(x => x.ObjectName).NotEmpty();
            RuleFor(x => x.ContentType).NotEmpty();
            RuleFor(x => x.TtlSeconds).GreaterThan(0).LessThanOrEqualTo(3600);
        }
    }

}
