namespace Auth.Api.User.Login;

public record LoginUserQuery(string Email, string Password) : IQuery<LoginUserResult>;

public record LoginUserResult(string Token);

public class LoginUserHandler(
    IGetUserRepository getUserRepository,
    JwtTokenGenerator jwtTokenGenerator
) : IQueryHandler<LoginUserQuery, LoginUserResult>
{
    public async Task<Result<LoginUserResult>> Handle(LoginUserQuery query, CancellationToken cancellationToken)
    {
        var user = await getUserRepository.GetUserAsync(query.Email, cancellationToken);

        if (!user.IsSuccess) return user.Error;

        if (user.Value is null) return new Error("NOT_FOUND", "User not found", ErrorType.NotFound);

        var passwordHasher = new PasswordHasher<object>();

        var verificationResult = passwordHasher.VerifyHashedPassword(new object(), user.Value.Password, query.Password);

        if (verificationResult != PasswordVerificationResult.Success)
            return new Error("INCORRECT", "Email or password is incorrect", ErrorType.InvalidRequest);

        var jwtToken = jwtTokenGenerator.Generate(user.Value.Password);

        return new LoginUserResult(jwtToken);
    }
}