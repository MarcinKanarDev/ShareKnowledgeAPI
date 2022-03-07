using FluentValidation;
using ShareKnowledgeAPI.Database;
using ShareKnowledgeAPI.Mapper.DTOs;

namespace ShareKnowledgeAPI.Validators
{
    public class UserRegisterValidator : AbstractValidator<UserRegisterDto>
    {
        public UserRegisterValidator(ApplicationDbContext dbContext)
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress()
                .WithMessage("Invalid Email Format");

            RuleFor(x => x.Password)
                .NotEmpty()
                .MinimumLength(8)
                .MaximumLength(30);

            RuleFor(x => x.ConfirmPassword)
                .Equal(e => e.Password)
                .WithMessage("Password and cofirmed password must be equal.");

            RuleFor(x => x.Email)
                .Custom((value, context) =>
                {
                    var emailInUse = dbContext.Users.Any(u => u.Email == value);
                    if (emailInUse) 
                    {
                        context.AddFailure("Email", "That Email is taken");
                    }
                });

            RuleFor(x => x.FirstName)
                .NotEmpty()
                .WithMessage("First Name should not be empty.")
                .MinimumLength(2)
                .MaximumLength(60);

            RuleFor(x => x.LastName)
                .NotEmpty()
                .WithMessage("Last Name should not be empty.")
                .MinimumLength(2)
                .MaximumLength(60);

            RuleFor(x => x.DateOfBirth)
                .Custom((value, context) =>
                {
                    var todayDate = DateTime.UtcNow;

                    if (value.CompareTo(todayDate) == 1 || value.CompareTo(todayDate) == 0) 
                    {
                        context.AddFailure("DateOfBirth", "Date of birth must be less than now local date.");
                    }
                });
        }
    }
}
