using FluentValidation;
using ShareKnowledgeAPI.Models;

namespace ShareKnowledgeAPI.Validators
{
    public class QueryValidator : AbstractValidator<Query>
    {
        private int[] possiblePageSize = new[] { 5, 10, 15, 30 };
        public QueryValidator()
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
