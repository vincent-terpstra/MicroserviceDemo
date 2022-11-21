using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace PlatformService.AsyncDataServices;

public class MessageBusClient : IMessageBusClient, IDisposable
{
    private readonly ILogger<MessageBusClient> _logger;
    private readonly IConnection _connection;
    private readonly IModel _channel;

    public MessageBusClient(IConfiguration config, ILogger<MessageBusClient> logger)
    {
        _logger = logger;
        var factory = new ConnectionFactory()
        {
            HostName = config["RabbitMQHost"],
            Port = int.Parse(config["RabbitMQPort"]!),
        };

        try
        {
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.ExchangeDeclare(exchange: "trigger", type: ExchangeType.Fanout);

            _connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unable to connect to the Message Queue");
            throw;
        }
    }

    private void RabbitMQ_ConnectionShutdown(object? sender, ShutdownEventArgs e)
    {
        _logger.LogInformation("Connection has been shutdown {Sender}, {ShutdownEvent}", sender, e);
    }


    public void PublishNewPlatform(PlatformPublishedDto platformPublishedDto)
    {
        try
        { 
            var message = JsonConvert.SerializeObject(platformPublishedDto);
            SendMessage(message);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Unable to publish platform {@Platform}", platformPublishedDto);
        }
        
    }

    private void SendMessage(string message)
    {
        if (_connection.IsOpen)
        {
            _logger.LogInformation("Rabbit MQ Connection is open sending message");
            var body = Encoding.UTF8.GetBytes(message);
            _channel.BasicPublish("trigger", "", null, body);
        }
        else
        {
            throw new Exception("Rabbit MQ Connection is closed");
        }
        
    }

    public void Dispose()
    {
        _logger.LogWarning("Message Bus dispossed");
        if (_channel.IsOpen)
        {
            _channel.Close();
            _connection.Close();
        }
            
    }

}