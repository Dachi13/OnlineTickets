namespace Auth.Api.User.Register;

public class RegisterUserRepository(DapperContext context) : IRegisterUserRepository
{
    public async Task<Result<long>> RegisterUserAsync(string username, string password, string email,
        CancellationToken cancellationToken = default)
    {
        try
        {
            await using var connection = await context.CreateConnectionAsync();

            var parameters = new DynamicParameters();
            parameters.Add("p_username", username);
            parameters.Add("p_password", password);
            parameters.Add("p_email", email);
            parameters.Add("userid", dbType: DbType.Int64, direction: ParameterDirection.Output);

            await connection.ExecuteAsync(
                "public.spadduser",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            var userId = parameters.Get<long>("userid");

            return userId;
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