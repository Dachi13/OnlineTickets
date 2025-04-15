namespace MessageSendingWorker.Models;

public class WorkerConfig
{
    public int DelayInSeconds { get; set; }
}

public class EmailConfig
{
    public string Name { get; set; }
    public string FromEmail { get; set; }
    public string ApiKey { get; set; }
}

public class Config
{
    public EmailConfig EmailConfig { get; set; }
    public WorkerConfig WorkerConfig { get; set; }
}