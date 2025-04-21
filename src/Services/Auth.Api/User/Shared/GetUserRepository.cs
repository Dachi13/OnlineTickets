namespace Auth.Api.User.Shared;

public class GetUserRepository(DapperContext context) : IGetUserRepository
{
    public async Task<Result<Models.User?>> GetUserAsync(string email, CancellationToken cancellationToken = default)
    {
        try
        {
            await using var connection = await context.CreateConnectionAsync();

            var user = await connection.QueryFirstOrDefaultAsync<Models.User>(
                "SELECT * FROM public.\"Users\" WHERE \"Email\" = @email",
                new { email });

            return user;
        }
        catch (NpgsqlException exception)
        {
            return new Error("Database_Error", exception.Message, ErrorType.DatabaseError);
        }
        catch (Exception exception)
        {
            return new Error("Internal_Error", exception.Message, ErrorType.InternalServerError);
        }
    }
}