// See https://aka.ms/new-console-template for more information
using producerProgram;

Console.WriteLine("Hello, World!");


Send send = new Send();

List<WorkItem> workItems = new List<WorkItem>() {
    new WorkItem("producer1", "WI1", DateTime.UtcNow),
    new WorkItem("producer1", "WI2", DateTime.UtcNow),
    new WorkItem("producer1", "WI3", DateTime.UtcNow)
};

foreach (WorkItem k in workItems){

    var marker = Send.GetMessage(args);
    k.Content = $"{marker}_{k.Content}";
    var sendResult = send.SendMessageToQueue(k.ToJson());

    if(sendResult.Item1 == true)
        Console.WriteLine($" [✅] {sendResult.Item2}");
    else
        Console.WriteLine($" [❌] {sendResult.Item2}");

}

Console.WriteLine(" Press [enter] to exit.");
Console.ReadLine();