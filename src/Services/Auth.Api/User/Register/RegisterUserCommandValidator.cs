namespace Auth.Api.User.Register;

public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserCommandValidator()
    {
        RuleFor(user => user.Username).NotNull().NotEmpty().WithMessage("username is empty");
        RuleFor(user => user.Password)
            .NotEmpty()
            .Length(5, 16)
            .WithMessage("password lenght must be between 5 and 16");
        
        RuleFor(user => user.Email).NotEmpty().EmailAddress().WithMessage("email is invalid");
    }
}