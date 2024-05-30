using System.Text;
using RabbitMQ.Client;

namespace PublisherProgram{


    public class Send{

        public ConnectionFactory Factory{get; set;}
        public IConnection Connection {get; set;}
        public IModel Channel {get; set;}
        public string RoutingKey {get; set;}

        public Send(){
            Factory = new ConnectionFactory { HostName = "localhost" };
            Connection = Factory.CreateConnection();
            Channel = Connection.CreateModel();
            
            // ExchangeType.Direct
            // ExchangeType.Fanout
            // ExchangeType.Headers
            Channel.ExchangeDeclare("topic_logs", ExchangeType.Topic);
        }

        public (bool, string) SendMessageToQueue(string logContent, string logTypeAndSeverity) {
            RoutingKey = logTypeAndSeverity;

            //message to send
            var body = Encoding.UTF8.GetBytes(logContent);

            try{
                var properties = Channel.CreateBasicProperties();
                properties.Persistent = true;

                Channel.BasicPublish(
                        exchange: "topic_logs",
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