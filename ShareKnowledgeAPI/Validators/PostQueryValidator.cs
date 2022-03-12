using FluentValidation;
using ShareKnowledgeAPI.Models;

namespace ShareKnowledgeAPI.Validators
{
    public class PostQueryValidator : AbstractValidator<PostQuery>
    {
        private int[] possiblePageSize = new[] { 5, 10, 15, 30 };
        public PostQueryValidator()
        {
            RuleFor(p => p.PageNumber).GreaterThanOrEqualTo(1);
            RuleFor(p => p.PageSize).Custom((value, context) =>
            {
                if (!possiblePageSize.Contains(value)) 
                {
                    context
                        .AddFailure("PageSize",
                            $"Page size must be in [{string.Join(",", possiblePageSize)}]");
                }
            });
        }
    }
}
