using System.Text;
using Core.Services;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;

namespace Data.Services;

/// <summary>
/// RabbitMq producer.
/// </summary>
public class RabbitMqProducer : IRabbitMqProducer
{
    private readonly IConfiguration configuration;

    /// <summary>
    /// Initializes a new instance of the <see cref="RabbitMqProducer"/> class.
    /// </summary>
    /// <param name="configuration">IConfiguration.</param>
    public RabbitMqProducer(IConfiguration configuration) => this.configuration = configuration;

    /// <summary>
    /// Send message about success adding new event.
    /// </summary>
    /// <param name="message">Message.</param>
    public void SendSuccessEventMessage(string message)
    {
        var factory = new ConnectionFactory { HostName = configuration.GetSection("RabbitMq:HostName").Get<string>() };
        using IConnection? connection = factory.CreateConnection();
        using (IModel? channel = connection.CreateModel())
        {
            byte[] body = Encoding.UTF8.GetBytes(message);

            channel.QueueDeclare("SuccessEventQueue", durable: true, exclusive: false, autoDelete: false, arguments: null);

            IBasicProperties? props = channel.CreateBasicProperties();
            props.Persistent = true;

            channel.BasicPublish("", "SuccessEventQueue", props, body);
        }
    }

    /// <summary>
    /// Send message about error adding new event.
    /// </summary>
    /// <param name="message">Message.</param>
    public void SendErrorEventMessage(string message)
    {
        var factory = new ConnectionFactory { HostName = "localhost" };
        using IConnection? connection = factory.CreateConnection();
        using (IModel? channel = connection.CreateModel())
        {
            byte[] body = Encoding.UTF8.GetBytes(message);

            channel.QueueDeclare("ErrorEventQueue", durable: true, exclusive: false, autoDelete: false, arguments: null);

            IBasicProperties? props = channel.CreateBasicProperties();
            props.Persistent = true;

            channel.BasicPublish("", "ErrorEventQueue", props, body);
        }
    }
}
