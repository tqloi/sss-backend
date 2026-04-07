using FastEndpoints;
using FluentValidation;

namespace SSS.WebApi.Endpoints.Storage.Upload
{
    public sealed class UploadValidator : Validator<UploadRequest>
    {
        public UploadValidator()
        {
            RuleFor(x => x.File).NotNull().WithMessage("File is required.");
            RuleFor(x => x.File.Length).GreaterThan(0).WithMessage("Empty file.");
            RuleFor(x => x.File.Length).LessThanOrEqualTo(20 * 1024 * 1024) // 20MB
                .WithMessage("Max file size is 20MB.");
        }
    }
}
