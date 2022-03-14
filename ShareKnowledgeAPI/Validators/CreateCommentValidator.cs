using FluentValidation;
using ShareKnowledgeAPI.Mapper.DTOs;

namespace ShareKnowledgeAPI.Validators
{
    public class CreateCommentValidator : AbstractValidator<CreateCommentDto>
    {
        public CreateCommentValidator()
        {
            RuleFor(c => c.CommentText)
                .MaximumLength(1000)
                .NotEmpty()
                .NotNull();
        }
    }
}
