using System.Text.Json;

namespace producerProgram{

    public class WorkItem{

        public string From {get; set;}

        public string Content {get; set;}

        public DateTime CreateTimestamp {get; set;}

        public WorkItem(string from, string content, DateTime createTimestamp){
            From = from;
            Content = content;
            CreateTimestamp = createTimestamp;
        }

        public string ToJson(){
            return JsonSerializer.Serialize(this);
        }

    }

}