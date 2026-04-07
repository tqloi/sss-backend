using FastEndpoints;
using FluentValidation;
using SSS.Application.Features.Content.LearningSubject.Delete;

namespace SSS.Web.Endpoints.Content.LearningSubject.Delete
{
    public class DeleteLearningSubjectValidator : Validator<DeleteLearningSubjectCommand>
    {
        public DeleteLearningSubjectValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Id must be greater than 0.");
        }
    }
}
