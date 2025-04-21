namespace Auth.Api.User.Register;

public record RegisterUserRequest(string Username, string Password, string Email);

public record RegisterUserResponse(bool WasSuccessful);

public static class RegisterUserEndpoint
{
    public static void AddRegisterUserRoute(this IEndpointRouteBuilder app)
    {
        app.MapPost("/register", async (RegisterUserRequest request, ISender sender) =>
            {
                var registerUserCommand = request.Adapt<RegisterUserCommand>();

                var result = await sender.Send(registerUserCommand);

                return result.Match(
                    _ => Results.Created($"/users/{result.Value}", result.Value),
                    error => error switch
                    {
                        { ErrorType: ErrorType.InternalServerError } => Results.InternalServerError(new
                            { message = error.Message }),
                        _ => Results.Conflict(error.Message)
                    });
            })
            .WithName("Register User")
            .Produces<RegisterUserResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Register User")
            .WithDescription("Register User");
    }
}