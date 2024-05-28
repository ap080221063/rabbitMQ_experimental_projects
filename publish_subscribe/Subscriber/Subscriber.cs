using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace SubscriberProgram{

    public class Subscriber{

        public ConnectionFactory Factory {get; set;}
        public IConnection Connection {get; set;}
        public IModel Channel {get; set;}

        public string QueueName {get; set;}

        public Subscriber(){
            Factory = new ConnectionFactory { HostName = "localhost" };
            Connection = Factory.CreateConnection();
            Channel = Connection.CreateModel();
            QueueName = Channel.QueueDeclare();
        }

        public void ReceiveMessageFromQueue(){

            Channel.QueueBind(
                queue: QueueName,
                exchange: "logs",
                routingKey: string.Empty
            );

            Channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

            Console.WriteLine(" [ℹ️] Waiting for messages.");

            var consumer = new EventingBasicConsumer(Channel);
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
                Thread.Sleep(dots * 3000);

                Channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);

            };

            Channel.BasicConsume(
            queue: QueueName,
            autoAck: false,
            consumer: consumer);

            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();

        }

    }

}