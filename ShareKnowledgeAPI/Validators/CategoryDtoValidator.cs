using FluentValidation;
using ShareKnowledgeAPI.Mapper.DTOs;

namespace ShareKnowledgeAPI.Validators
{
    public class CategoryDtoValidator : AbstractValidator<CategoryDto>
    {
        public CategoryDtoValidator()
        {
            RuleFor(c => c.CategoryName).MaximumLength(100)
                .NotEmpty()
                .NotNull();
        }
    }
}
