using FluentValidation;
using ShareKnowledgeAPI.Mapper.DTOs;

namespace ShareKnowledgeAPI.Validators
{
    public class CreatePostValidator : AbstractValidator<CreatePostDto>
    {
        public CreatePostValidator() 
        {
            RuleFor(dto => dto.Title)
                .MaximumLength(250)
                .NotEmpty()
                .NotNull()
                .WithMessage("Title cannot be empty.");
            
            RuleFor(dto => dto.Description)
                .MaximumLength(600)
                .NotEmpty()
                .NotNull()
                .WithMessage("Description cannot be empty.");
        }
    }
}
