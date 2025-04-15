using MessageSendingWorker.Models;

namespace MessageSendingWorker.Repositories;

public interface IPersonRepository
{
    Task<Person> GetPersonAsync(long id);
}