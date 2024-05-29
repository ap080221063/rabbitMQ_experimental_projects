using System.Text;
using RabbitMQ.Client;

namespace PublisherProgram{


    public class Send{

        public readonly string[] ROUTINGOPTIONS = {"INFO", "WARN", "ERROR"};

        public ConnectionFactory Factory{get; set;}
        public IConnection Connection {get; set;}
        public IModel Channel {get; set;}
        public string RoutingKey {get; set;}

        public Send(){
            Factory = new ConnectionFactory { HostName = "localhost" };
            Connection = Factory.CreateConnection();
            Channel = Connection.CreateModel();
            
            Random k = new Random();
            RoutingKey = ROUTINGOPTIONS[k.Next(0,2)];

            // ExchangeType.Topic
            // ExchangeType.Fanout
            // ExchangeType.Headers
            Channel.ExchangeDeclare("logs", ExchangeType.Direct);
        }

        public (bool, string) SendMessageToQueue(string workItem) {

            //message to send
            var body = Encoding.UTF8.GetBytes(workItem);

            try{
                var properties = Channel.CreateBasicProperties();
                properties.Persistent = true;

                Channel.BasicPublish(
                        exchange: "logs",
                        routingKey: RoutingKey,
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