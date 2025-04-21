namespace Auth.Api.User.Login;

public class LoginUserQueryValidator : AbstractValidator<LoginUserQuery>
{
    public LoginUserQueryValidator()
    {
        RuleFor(user => user.Email).NotEmpty().EmailAddress().WithMessage("Email is invalid");
        RuleFor(user => user.Password).NotEmpty().MinimumLength(6)
            .WithMessage("Password must be at least 6 characters");
    }
}