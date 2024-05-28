using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace workerProgram{

    public class Worker{

        public Worker(){

        }

        public void ReceiveMessageFromQueue(){

            var factory = new ConnectionFactory { HostName = "localhost" };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.QueueDeclare(queue: "work_queues",
                     durable: true,
                     exclusive: false,
                     autoDelete: false,
                     arguments: null);

            channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

            Console.WriteLine(" [ℹ️] Waiting for messages.");

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                MessageToReceive messageDto = JsonSerializer.Deserialize<MessageToReceive>(message);

                Random r = new Random();
                var random = r.Next(0,2);
                //Console.WriteLine("random="+random);
                //simulate disruption
                if(random == 0)
                {
                    Console.WriteLine($"[❌]worker nack'ed, requeuing.. {messageDto.Content}");
                    channel.BasicNack(deliveryTag: ea.DeliveryTag, false, true);
                }
                else
                {

                    Console.WriteLine("  ");
                    Console.WriteLine($"[✅] Received at {messageDto.CreateTimestamp}");               
                    Console.WriteLine($"[✅] From {messageDto.From}");          
                    Console.WriteLine($"[✅] Content {messageDto.Content}");

                    int dots = message.Split('.').Length - 1;
                    Thread.Sleep(dots * 3000);

                    channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
                }
            };

            channel.BasicConsume(
            queue: "work_queues",
            autoAck: false,
            consumer: consumer);

            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();

        }

    }

}