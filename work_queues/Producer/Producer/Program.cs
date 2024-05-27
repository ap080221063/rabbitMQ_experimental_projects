// See https://aka.ms/new-console-template for more information
using producerProgram;

Console.WriteLine("type 'Stop' to exit");
int counter = 0;
string consoleInput = string.Empty;
Send send = new Send();

    do{
        consoleInput = Console.ReadLine();

        if(consoleInput != "Stop"){
            //var marker = Send.GetMessage(args);
            counter ++;
            WorkItem k = new WorkItem("Producer1", consoleInput, DateTime.UtcNow);
            k.Content = $"{counter}_{k.Content}";
            var sendResult = send.SendMessageToQueue(k.ToJson());

            if(sendResult.Item1 == true)
                Console.WriteLine($" [✅] {sendResult.Item2}");
            else
                Console.WriteLine($" [❌] {sendResult.Item2}");
        }

    }while(consoleInput != "Stop");


Console.ReadLine();