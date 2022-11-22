namespace CommandService.AsyncDataServices;

public class MessageBusSubscriber :BackgroundService
{
    private readonly IConfiguration _config;

    public MessageBusSubscriber(IConfiguration config)
    {
        _config = config;
    }
    
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        throw new NotImplementedException();
    }
}