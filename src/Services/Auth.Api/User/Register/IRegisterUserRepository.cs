namespace Auth.Api.User.Register;

public interface IRegisterUserRepository
{
    Task<Result<long>> RegisterUserAsync(string username, string password, string email, CancellationToken cancellationToken = default);
}