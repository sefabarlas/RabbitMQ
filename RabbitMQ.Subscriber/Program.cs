// See https://aka.ms/new-console-template for more information

using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

var connectionFactory = new ConnectionFactory
{
    Uri = new Uri("amqp://localhost:5672")
};

using var connection = connectionFactory.CreateConnection();

var channel = connection.CreateModel();
// channel.QueueDeclare("hello-queue", true, false, false);
channel.BasicQos(0, 1, false);
var consumer = new EventingBasicConsumer(channel);
channel.BasicConsume("hello-queue", false, consumer);

consumer.Received += (sender, eventArgs) =>
{
    var message = Encoding.UTF8.GetString(eventArgs.Body.ToArray());
    Console.WriteLine("Gelen Mesaj: " + message);
    channel.BasicAck(eventArgs.DeliveryTag, false);
};

Console.ReadLine();