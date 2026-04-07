using FastEndpoints;
using FluentValidation;
using SSS.Application.Features.Content.LearningSubject.Update;

namespace SSS.Web.Endpoints.Content.LearningSubject.Update
{
    public class UpdateLearningSubjectValidator : Validator<UpdateLearningSubjectCommand>
    {
        public UpdateLearningSubjectValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Id must be greater than 0.");
            
            RuleFor(x => x.CategoryId)
                .GreaterThan(0).WithMessage("CategoryId must be greater than 0.");
            
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(100).WithMessage("Name cannot exceed 100 characters.");
            
            RuleFor(x => x.Description)
                .MaximumLength(500).WithMessage("Description cannot exceed 500 characters.");
        }
    }
}
