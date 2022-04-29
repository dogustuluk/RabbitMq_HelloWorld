using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace RabbitMQ.subscriber
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory();
            factory.Uri = new Uri("amqps://nxdranwu:n_3nr-xZlXx0NoCWuFP05gTqZfp7_hwK@sparrow.rmq.cloudamqp.com/nxdranwu ");
            using var connection = factory.CreateConnection();
            var channel = connection.CreateModel();
            // channel.QueueDeclare("hello-queue", true, false, false);

            var consumer =new EventingBasicConsumer(channel);

            channel.BasicConsume("hello-queue", true, consumer);


            consumer.Received += (object sender, BasicDeliverEventArgs e) =>
            {
                var message = Encoding.UTF8.GetString(e.Body.ToArray());
                Console.WriteLine("Gelen Mesaj: " + message);
            };

            Console.ReadLine();
        }

    }
}
