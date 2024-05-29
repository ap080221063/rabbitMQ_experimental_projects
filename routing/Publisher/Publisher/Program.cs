// See https://aka.ms/new-console-template for more information
using PublisherProgram;
Random r = new Random();
int instanceRandom = r.Next(1,100);
Console.WriteLine($"Publisher{instanceRandom}");
string consoleInput = string.Empty;

Send send = new Send();
Console.WriteLine($"rounting Key = {send.RoutingKey}");
Console.WriteLine("type 'Stop' to exit");
    
    do{
        consoleInput = Console.ReadLine();

        if(consoleInput != "Stop"){
            //var marker = Send.GetMessage(args);

            WorkItem k = new WorkItem($"Publisher{instanceRandom}", consoleInput, DateTime.UtcNow);
            k.Content = $"{k.Content}";
            var sendResult = send.SendMessageToQueue(k.ToJson());

            if(sendResult.Item1 == true)
                Console.WriteLine($" [✅] {sendResult.Item2}");
            else
                Console.WriteLine($" [❌] {sendResult.Item2}");
        }

    }while(consoleInput != "Stop");


Console.ReadLine();