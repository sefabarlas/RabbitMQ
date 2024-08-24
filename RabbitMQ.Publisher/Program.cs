// See https://aka.ms/new-console-template for more information

using System.Text;
using RabbitMQ.Client;

var connectionFactory = new ConnectionFactory
{
    Uri = new Uri("amqp://localhost:5672")
};

using var connection = connectionFactory.CreateConnection();

var channel = connection.CreateModel();
channel.ExchangeDeclare("logs-fanout", durable: true, type: ExchangeType.Fanout);

Enumerable.Range(1, 50).ToList().ForEach(x =>
{
    var message = $"log {x}";
    var messageBody = Encoding.UTF8.GetBytes(message);

    channel.BasicPublish("logs-fanout", "", null, messageBody);
    Console.WriteLine($"Message sent: {message}");
});

Console.ReadLine();