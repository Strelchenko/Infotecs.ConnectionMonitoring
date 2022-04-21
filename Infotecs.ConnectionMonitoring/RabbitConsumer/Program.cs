using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

Console.WriteLine("Choose which event you want to see:");
Console.WriteLine("\tS - Success");
Console.WriteLine("\tE - Error");
Console.WriteLine("Or press [enter] to exit.");

var messageQueue = "";
var viewMessage = "";

switch (Console.ReadLine())
{
    case "S":
        messageQueue = "SuccessEventQueue";
        viewMessage = "Зарегистрировано новое событие узла:";
        break;
    case "E":
        messageQueue = "ErrorEventQueue";
        viewMessage = "Ошибка регистрации события узла:";
        break;
}

var factory = new ConnectionFactory() { HostName = "localhost" };
using (var connection = factory.CreateConnection())
using (var channel = connection.CreateModel())
{
    channel.QueueDeclare(queue: messageQueue,
        durable: true,
        exclusive: false,
        autoDelete: false,
        arguments: null);

    var consumer = new EventingBasicConsumer(channel);
    consumer.Received += (model, ea) =>
    {
        var body = ea.Body.ToArray();
        var message = Encoding.UTF8.GetString(body);
        Console.WriteLine("- {0} {1}", viewMessage, message);
    };
    channel.BasicConsume(queue: messageQueue,
        autoAck: true,
        consumer: consumer);
    Console.ReadLine();
}
