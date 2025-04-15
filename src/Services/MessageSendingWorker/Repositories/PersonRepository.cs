using MessageSendingWorker.Models;

namespace MessageSendingWorker.Repositories;

public class PersonRepository : IPersonRepository
{
    public Task<Person> GetPersonAsync(long id)
    {
        throw new NotImplementedException();
    }
}