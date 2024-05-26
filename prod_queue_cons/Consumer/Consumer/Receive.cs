using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace receiverProgram{

    public class Receive{

        public Receive(){

        }

        public void ReceiveMessageFromQueue(){

            var factory = new ConnectionFactory { HostName = "localhost" };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            Console.WriteLine(" [ℹ️] Waiting for messages.");

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                MessageToReceive messageDto = JsonSerializer.Deserialize<MessageToReceive>(message);

                Console.WriteLine($"[✅] Received at {messageDto.CreateTimestamp}");
                
                Console.WriteLine($"[✅] From {messageDto.From}");
                
                Console.WriteLine($"[✅] Content {messageDto.Content}");
            };

            channel.BasicConsume(queue: "pqc",
            autoAck: true,
            consumer: consumer);

            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();

        }

    }

}