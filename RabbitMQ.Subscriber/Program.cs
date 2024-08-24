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
// channel.ExchangeDeclare("logs-fanout", durable: true, type: ExchangeType.Fanout);

var randomQueueName = channel.QueueDeclare().QueueName;

//Makes the queue permanent.
#region Permanent Queue

// var randomQueueName = "log-database-save-queue";
// channel.QueueDeclare(randomQueueName, true, false, false); 

#endregion

channel.QueueBind(randomQueueName,  "logs-fanout", "", null);
channel.BasicQos(0, 1, false);

var consumer = new EventingBasicConsumer(channel);

Console.WriteLine("The logs are listening...");

consumer.Received += (sender, eventArgs) =>
{
    var message = Encoding.UTF8.GetString(eventArgs.Body.ToArray());
    
    Thread.Sleep(1500);
    Console.WriteLine("Incoming message: " + message);
    channel.BasicAck(eventArgs.DeliveryTag, false);
};

channel.BasicConsume(randomQueueName, false, consumer);

Console.ReadLine();