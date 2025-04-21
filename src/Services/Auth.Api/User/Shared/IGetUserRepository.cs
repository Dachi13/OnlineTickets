namespace Auth.Api.User.Shared;

public interface IGetUserRepository
{
    Task<Result<Models.User?>> GetUserAsync(string email, CancellationToken cancellationToken = default);
}