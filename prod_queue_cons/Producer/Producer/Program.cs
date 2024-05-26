// See https://aka.ms/new-console-template for more information
using producerProgram;

Console.WriteLine("Hello, World!");


Send send = new Send();

Message m = new Message("Producer", "This is a test message", DateTime.UtcNow);

var sendResult = send.SendMessageToQueue(m.ToJson());

if(sendResult.Item1 == true)
    Console.WriteLine($" [✅] {sendResult.Item2}");
else
    Console.WriteLine($" [❌] {sendResult.Item2}");

    Console.WriteLine(" Press [enter] to exit.");
    Console.ReadLine();