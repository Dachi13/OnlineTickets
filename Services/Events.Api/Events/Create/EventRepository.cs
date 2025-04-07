namespace Events.Api.Events.Create;

public class EventRepository(DapperContext context) : IEventRepository
{
    public async Task<Result<long>> AddEventAsync(CreateEventCommand eventCommand)
    {
        try
        {
            var imageBytes = eventCommand.ImageFile is not null
                ? Convert.FromBase64String(eventCommand.ImageFile)
                : null;

            await using var connection = await context.CreateConnectionAsync();

            var parameters = new DynamicParameters();
            parameters.Add("p_name", eventCommand.Name);
            parameters.Add("p_description", eventCommand.Description);
            parameters.Add("p_location", eventCommand.Location);
            parameters.Add("p_categoryid", eventCommand.CategoryId);
            parameters.Add("p_amountoftickets", eventCommand.AmountOfTickets);
            parameters.Add("p_starttime", eventCommand.StartTime);
            parameters.Add("p_endtime", eventCommand.EndTime);
            parameters.Add("p_imagefile", imageBytes);
            parameters.Add("eventid", value: null, dbType: DbType.Int64, direction: ParameterDirection.Output);

            await connection.ExecuteAsync(
                "spaddevent",
                parameters,
                commandType: CommandType.StoredProcedure);

            var productId = parameters.Get<long>("eventid");

            return productId;
        }
        catch (PostgresException exception)
        {
            return new Error("Database_Exception", exception.Message, ErrorType.DatabaseError);
        }
        catch (Exception exception)
        {
            return new Error("Internal_Error", exception.Message, ErrorType.InternalServerError);
        }
    }
}