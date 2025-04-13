namespace Events.Api.Events.Create;

public record CreateEventCommand(
    string Name,
    string Description,
    string Location,
    int CategoryId,
    int AmountOfTickets,
    double Price,
    DateTime StartTime,
    DateTime EndTime,
    string? ImageFile)
    : ICommand<CreateEventResult>;

public record CreateEventResult(long Id);

internal class CreateEventCommandHandler(IEventRepository repository)
    : ICommandHandler<CreateEventCommand, CreateEventResult>
{
    public async Task<Result<CreateEventResult>> Handle(CreateEventCommand command,
        CancellationToken cancellationToken)
    {
        var result = await repository.AddEventAsync(command);

        return result.IsSuccess
            ? new CreateEventResult(result.Value)
            : result.Error;
    }
}