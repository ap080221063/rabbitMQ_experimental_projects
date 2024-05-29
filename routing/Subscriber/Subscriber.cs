using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace SubscriberProgram{

    public class Subscriber{

        public readonly string[] ROUTINGOPTIONS = {"INFO", "WARN", "ERROR"};
        public ConnectionFactory Factory {get; set;}
        public IConnection Connection {get; set;}
        public IModel Channel {get; set;}

        public string QueueName {get; set;}

        public Subscriber(){
            Factory = new ConnectionFactory { HostName = "localhost" };
            Connection = Factory.CreateConnection();
            Channel = Connection.CreateModel();
            QueueName = Channel.QueueDeclare();


            //randomizing routingKeys to randomize number of queue binds
            Random j = new Random(); 
            foreach(var k in ROUTINGOPTIONS.Take(j.Next(1,3)))
            {
                Console.WriteLine("BINDING TO LOG TYPE: " + k);
                Channel.QueueBind(
                    queue: QueueName,
                    exchange: "logs",
                    routingKey: k
                );
            }

        }

        public void ReceiveMessageFromQueue(){

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