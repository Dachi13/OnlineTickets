namespace Events.Api.Events.Create;

public interface IEventRepository
{
    Task<Result<long>> AddEventAsync(CreateEventCommand eventCommand);
}