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
                     durable: false,
                     exclusive: false,
                     autoDelete: false,
                     arguments: null);

            Console.WriteLine(" [ℹ️] Waiting for messages.");

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                MessageToReceive messageDto = JsonSerializer.Deserialize<MessageToReceive>(message);

                Console.WriteLine("  ");
                Console.WriteLine($"[✅] Received at {messageDto.CreateTimestamp}");               
                Console.WriteLine($"[✅] From {messageDto.From}");          
                Console.WriteLine($"[✅] Content {messageDto.Content}");

                int dots = message.Split('.').Length - 1;
                Thread.Sleep(dots * 1000);

            };

            channel.BasicConsume(queue: "work_queues",
            autoAck: true,
            consumer: consumer);

            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();

        }

    }

}