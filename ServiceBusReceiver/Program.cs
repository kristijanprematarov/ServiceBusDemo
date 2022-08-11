using System;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using ServiceBusShared.Models;

namespace ServiceBusReceiver
{
    public class Program
    {
        const string CONNECTION_STRING_QUEUE = "Endpoint=sb://servicebusdemo-kpr.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=yJHI/e+K0DmhoWWtBq4+SjCboYg1q9N8phf/EJOg2Vk=";
        const string CONNECTION_STRING_TOPIC = "Endpoint=sb://servicebus-kpr-topics.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=Lvea+dr4UYn+Iut0WMi2954THIG4bp+Z9UTveOAbSiQ=";
        const string QUEUE_NAME = "personqueue";
        const string TOPIC_NAME = "persontopic";
        const string TOPIC_SUBSCRIPTION = "persontopic-kris";
        static ServiceBusClient serviceBusClient = new ServiceBusClient(CONNECTION_STRING_QUEUE);
        static ServiceBusClient serviceBusClientTopics = new ServiceBusClient(CONNECTION_STRING_TOPIC);

        async static Task Main(string[] args)
        {
            //var receiver = serviceBusClient.CreateReceiver(QUEUE_NAME, new ServiceBusReceiverOptions()
            //{
            //    //ReceiveMode = ServiceBusReceiveMode.ReceiveAndDelete,
            //    ReceiveMode = ServiceBusReceiveMode.PeekLock
            //});

            //var messageFromQueue = await receiver.ReceiveMessageAsync();

            //var personFromQueue = JsonSerializer.Deserialize<Person>(messageFromQueue.Body);

            //Console.WriteLine(personFromQueue.FirstName);
            //Console.WriteLine(personFromQueue.LastName);

            //await receiver.CompleteMessageAsync(messageFromQueue);

            //*****************************************************************************************

            //TOPIC
            var processor =
                serviceBusClientTopics.CreateSessionProcessor(TOPIC_NAME,
                    TOPIC_SUBSCRIPTION,
                    new ServiceBusSessionProcessorOptions
                    {
                        AutoCompleteMessages = false,
                        MaxAutoLockRenewalDuration = TimeSpan.FromDays(3),
                        MaxConcurrentSessions = 10,
                        MaxConcurrentCallsPerSession = 1
                    });

            processor.ProcessMessageAsync += MessageHandler;
            processor.ProcessErrorAsync += ErrorHandler;

            await processor.StartProcessingAsync();

            Console.WriteLine("Wait for a minute and then press any key to end the processing");
            Console.ReadKey();

            // stop processing 
            Console.WriteLine("\nStopping the receiver...");
            await processor.StopProcessingAsync();
            Console.WriteLine("Stopped receiving messages");
        }
        private static Task ErrorHandler(ProcessErrorEventArgs args)
        {
            Console.WriteLine("Failed to process the message");
            return Task.CompletedTask;
        }
        private static async Task MessageHandler(ProcessSessionMessageEventArgs args)
        {
            var message = JsonSerializer.Deserialize<Person>(args.Message.Body);

            Console.WriteLine(message.FirstName);
            Console.WriteLine(message.LastName);
            Console.WriteLine("*******************");

            await args.CompleteMessageAsync(args.Message);
        }
    }
}
