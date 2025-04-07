namespace Events.Api.Events.Create;

public record CreateEventRequest(
    string Name,
    string Description,
    string Location,
    int CategoryId,
    int AmountOfTickets,
    DateTime StartTime,
    DateTime EndTime,
    string? ImageFile);

public record CreateEventResponse(long Id);

public static class CreateEventEndpoint
{
    public static void AddRoutes(this IEndpointRouteBuilder app)
    {
        app.MapPost("/events",
                async (CreateEventRequest request, ISender sender) =>
                {
                    var command = request.Adapt<CreateEventCommand>();

                    var result = await sender.Send(command);

                    return result.Match(
                        _ => Results.Created($"/events/{result.Value.Id}", result.Value),
                        error => error switch
                        {
                            { ErrorType: ErrorType.DatabaseError or ErrorType.InternalServerError } => Results.Conflict(
                                new { message = error.Message }),
                            { ErrorType: ErrorType.Validation } => Results.BadRequest(new { message = error.Message }),
                            _ => Results.Problem(error.Message)
                        });
                })
            .WithName("CreateEvent")
            .Produces<CreateEventResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Create Event")
            .WithDescription("Create Event");
    }
}