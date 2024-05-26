using System.Text;
using RabbitMQ.Client;

namespace producerProgram{

    public class Send{

        public Send(){

        }

        public (bool, string) SendMessageToQueue(string workItem) {

            var factory = new ConnectionFactory { HostName = "localhost" };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();
            
            //declare a queue
            channel.QueueDeclare(queue: "work_queues",
                     durable: false,
                     exclusive: false,
                     autoDelete: false,
                     arguments: null);

            string stateMessage = "Preparing work item to Send";

            //message to send
            var body = Encoding.UTF8.GetBytes(workItem);

            stateMessage = "Sending work item";
            try{
                channel.BasicPublish(exchange: string.Empty,
                        routingKey: "work_queues",
                        basicProperties: null,
                        body: body);
            }catch(Exception ex){
                Console.WriteLine(ex);
                return (false, stateMessage);
            }

            stateMessage = "Work item sent";

            return (true, stateMessage);
        }

        public static string GetMessage(string[] args)
        {
            return ((args.Length > 0) ? string.Join(" ", args) : "P1");
        }
    }

}