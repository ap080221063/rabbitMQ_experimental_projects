// See https://aka.ms/new-console-template for more information
 using SubscriberProgram;

Subscriber Subscriber = new Subscriber(args[0].ToString());
Subscriber.ReceiveMessageFromQueue();


