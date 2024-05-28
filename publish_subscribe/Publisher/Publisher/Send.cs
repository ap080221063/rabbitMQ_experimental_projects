using System.Text;
using RabbitMQ.Client;

namespace PublisherProgram{


    public class Send{
        public ConnectionFactory Factory{get; set;}
        public IConnection Connection {get; set;}
        public IModel Channel {get; set;}

        public Send(){
            Factory = new ConnectionFactory { HostName = "localhost" };
            Connection = Factory.CreateConnection();
            Channel = Connection.CreateModel();
        }

        public (bool, string) SendMessageToQueue(string workItem) {

            // ExchangeType.Topic
            // ExchangeType.Direct
            // ExchangeType.Headers
            Channel.ExchangeDeclare("logs", ExchangeType.Fanout);

            //message to send
            var body = Encoding.UTF8.GetBytes(workItem);

            try{
                var properties = Channel.CreateBasicProperties();
                properties.Persistent = true;

                Channel.BasicPublish(
                        exchange: "logs",
                        routingKey: string.Empty,
                        basicProperties: properties,
                        body: body);
            }catch(Exception ex){
                Console.WriteLine(ex);
                return (false, "Error");
            }

            return (true, "Success");
        }

        public static string GetMessage(string[] args)
        {
            return ((args.Length > 0) ? string.Join(" ", args) : "P1");
        }
    }

}