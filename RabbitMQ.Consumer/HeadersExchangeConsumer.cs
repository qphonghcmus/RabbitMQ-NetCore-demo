using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQ.Consumer
{
    public static class HeadersExchangeConsumer
    {
        public static void Consume(IModel channel)
        {
            channel.ExchangeDeclare("demo-headers-exchange", ExchangeType.Headers);
            channel.QueueDeclare(queue: "demo-headers-queue",
                       durable: true,
                       exclusive: false,
                       autoDelete: false,
                       arguments: null);
            
            var headers = new Dictionary<string, object> { { "account", "new" } };

            channel.QueueBind("demo-headers-queue", "demo-headers-exchange", string.Empty, headers);
            channel.BasicQos(0, 10, false);


            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (sender, e) =>
            {
                var body = e.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine(message);
            };
            channel.BasicConsume("demo-headers-queue", true, consumer);
            Console.WriteLine("Consumer started");
            Console.ReadLine();
        }
    }
}
