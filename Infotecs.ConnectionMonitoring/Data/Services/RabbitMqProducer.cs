using System.Text;
using System.Text.Json;
using Core.Models;
using Core.Services;
using Mapster;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;

namespace Data.Services;

/// <summary>
/// RabbitMq producer.
/// </summary>
public class RabbitMqProducer : IRabbitMqProducer
{
    private static readonly TypeAdapterConfig eventRabbitModelConfig = TypeAdapterConfig<ConnectionEvent, ConnectionEventRabbitModel>
        .NewConfig()
        .Map(dest => dest.EventId, src => src.Id)
        .Map(dest => dest.NodeId, src => src.ConnectionId)
        .Map(dest => dest.Name, src => src.Name)
        .Map(dest => dest.Date, src => src.EventTime)
        .Config;

    private readonly IConfiguration configuration;
    private IConnection connection;
    private IModel channel;

    /// <summary>
    /// Initializes a new instance of the <see cref="RabbitMqProducer"/> class.
    /// </summary>
    /// <param name="configuration">IConfiguration.</param>
    public RabbitMqProducer(IConfiguration configuration)
    {
        this.configuration = configuration;

        (connection, channel) = CreateConnection(configuration);
    }

    /// <summary>
    /// Send message about success adding new event.
    /// </summary>
    /// <param name="connectionEvent">ConnectionEvent.</param>
    public void SendSuccessEventMessage(ConnectionEvent connectionEvent)
    {
        var message = connectionEvent.Adapt<ConnectionEventRabbitModel>(eventRabbitModelConfig);

        byte[] body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));

        IBasicProperties? props = channel.CreateBasicProperties();
        props.Persistent = true;

        BasicPublish("", "SuccessEventQueue", props, body);
    }

    /// <summary>
    /// Send message about error adding new event.
    /// </summary>
    /// <param name="connectionEvent">ConnectionEvent.</param>
    public void SendErrorEventMessage(ConnectionEvent connectionEvent)
    {
        var message = connectionEvent.Adapt<ConnectionEventRabbitModel>(eventRabbitModelConfig);

        byte[] body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));

        IBasicProperties? props = channel.CreateBasicProperties();
        props.Persistent = true;

        BasicPublish("", "ErrorEventQueue", props, body);
    }

    /// <summary>
    /// Publish message to Rabbit.
    /// </summary>
    /// <param name="exchange">Exchange name.</param>
    /// <param name="routingKey">Routing key</param>
    /// <param name="props">Properties.</param>
    /// <param name="body">Body.</param>
    protected virtual void BasicPublish(string exchange, string routingKey, IBasicProperties props, ReadOnlyMemory<byte> body) =>
        channel.BasicPublish(exchange, routingKey, props, body);

    /// <summary>
    /// Creating connection and channel.
    /// </summary>
    /// <param name="conf">IConfiguration.</param>
    /// <returns></returns>
    protected virtual (IConnection, IModel) CreateConnection(IConfiguration conf)
    {
        var factory = new ConnectionFactory { HostName = conf.GetSection("RabbitMq:HostName").Get<string>() };
        IConnection? newConnection = factory.CreateConnection();
        IModel? newChannel = newConnection.CreateModel();
        newChannel.QueueDeclare("SuccessEventQueue", durable: true, exclusive: false, autoDelete: false, arguments: null);
        newChannel.QueueDeclare("ErrorEventQueue", durable: true, exclusive: false, autoDelete: false, arguments: null);

        return (newConnection, newChannel);
    }
}
