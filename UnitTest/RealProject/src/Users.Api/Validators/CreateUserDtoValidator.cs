using FluentValidation;
using Users.Api.Dtos;
using Users.Api.Models;

namespace Users.Api.Validators
{
    public class CreateUserDtoValidator:AbstractValidator<CreateUserDto>
    {
        public CreateUserDtoValidator()
        {
            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage("Full name is required.")
                .MinimumLength(3).WithMessage("Full name must be at least 3 characters long.")
                .MaximumLength(100).WithMessage("Full name must not exceed 100 characters.");
        }
    }
}
