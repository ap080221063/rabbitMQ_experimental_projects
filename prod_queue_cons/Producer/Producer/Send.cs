using System.Text;
using RabbitMQ.Client;

namespace producerProgram{

    public class Send{

        public Send(){

        }

        public (bool, string) SendMessageToQueue(string message) {

            var factory = new ConnectionFactory { HostName = "localhost" };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();
            
            //declare a queue
            channel.QueueDeclare(queue: "pqc",
                     durable: false,
                     exclusive: false,
                     autoDelete: false,
                     arguments: null);

            string stateMessage = "Preparing message to Send";

            //message to send
            var body = Encoding.UTF8.GetBytes(message);

            stateMessage = "Sending message";
            try{
                channel.BasicPublish(exchange: string.Empty,
                        routingKey: "pqc",
                        basicProperties: null,
                        body: body);
            }catch(Exception ex){
                Console.WriteLine(ex);
                return (false, stateMessage);
            }

            stateMessage = "Message sent";

            return (true, stateMessage);
        }


    }

}