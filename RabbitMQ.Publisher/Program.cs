// See https://aka.ms/new-console-template for more information

using System.Text;
using RabbitMQ.Client;

var connectionFactory = new ConnectionFactory
{
    Uri = new Uri("amqp://localhost:5672")
};

using var connection = connectionFactory.CreateConnection();

var channel = connection.CreateModel();
channel.QueueDeclare("hello-queue", true, false, false);

Enumerable.Range(1, 50).ToList().ForEach(x =>
{
    var message = $"Message {x}";
    var messageBody = Encoding.UTF8.GetBytes(message);

    channel.BasicPublish(string.Empty, "hello-queue", null, messageBody);
    Console.WriteLine($"Mesaj Gönderildi: {message}");
});

Console.ReadLine();