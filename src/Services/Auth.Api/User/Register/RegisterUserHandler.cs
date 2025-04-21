namespace Auth.Api.User.Register;

public record RegisterUserCommand(string Username, string Password, string Email) : ICommand<RegisterUserResult>;

public record RegisterUserResult(bool WasSuccessful);

public class RegisterUserHandler(
    IRegisterUserRepository userRepository,
    IGetUserRepository getUserRepository,
    PasswordHasher<object> passwordHasher) : ICommandHandler<RegisterUserCommand, RegisterUserResult>
{
    public async Task<Result<RegisterUserResult>> Handle(RegisterUserCommand command,
        CancellationToken cancellationToken)
    {
        var user = await getUserRepository.GetUserAsync(command.Email, cancellationToken);

        if (!user.IsSuccess) return user.Error;

        if (user.Value is not null)
            return new Error("Invalid_Request", "Such email already exists", ErrorType.InvalidRequest);

        var hashPassword = passwordHasher.HashPassword(new object(), command.Password);

        var userCreation = await userRepository.RegisterUserAsync(
            command.Username,
            hashPassword,
            command.Email,
            cancellationToken);

        return new RegisterUserResult(userCreation.IsSuccess);
    }
}