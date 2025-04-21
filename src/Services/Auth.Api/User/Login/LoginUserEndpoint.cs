namespace Auth.Api.User.Login;

public record LoginUserRequest(string Email, string Password);

public record LoginUserResponse(bool WasSuccessful);

public static class LoginUserEndpoint
{
    public static void AddUserLoginRoute(this IEndpointRouteBuilder app)
    {
        app.MapGet("/login", async ([FromBody] LoginUserRequest request, ISender sender) =>
            {
                var loginUserQuery = request.Adapt<LoginUserQuery>();

                var result = await sender.Send(loginUserQuery);

                return result.Match(
                    _ => Results.Created($"/users/{result.Value}", result.Value),
                    error => error switch
                    {
                        { ErrorType: ErrorType.InternalServerError } => Results.InternalServerError(new
                            { message = error.Message }),
                        _ => Results.Conflict(error.Message)
                    });
            })
            .WithName("Login User")
            .Produces<RegisterUserResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Login User")
            .WithDescription("Login User");
    }
}