using System.Text.Json;

namespace SubscriberProgram{

    public class MessageToReceive{

        public string From {get; set;}

        public string Content {get; set;}

        public DateTime CreateTimestamp {get; set;}

    }

}