namespace MessageSendingWorker.Repositories;

public class PersonRepository(DapperContext context) : IPersonRepository
{
    public async Task<Person?> GetPersonAsync(long id)
    {
        await using var connection = await context.CreateConnectionAsync();

        var sql = "SELECT * FROM public.\"Users\" WHERE \"Id\" = @Id";
        var person = await connection.QueryFirstOrDefaultAsync<Person>(sql, new { Id = id });

        return person;
    }
}