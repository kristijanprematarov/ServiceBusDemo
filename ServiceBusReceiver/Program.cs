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
        const string CONNECTION_STRING = "Endpoint=sb://servicebusdemo-kpr.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=yJHI/e+K0DmhoWWtBq4+SjCboYg1q9N8phf/EJOg2Vk=";
        const string QUEUE_NAME = "personqueue";
        static ServiceBusClient serviceBusClient = new ServiceBusClient(CONNECTION_STRING);

        async static Task Main(string[] args)
        {
            var receiver = serviceBusClient.CreateReceiver(QUEUE_NAME, new ServiceBusReceiverOptions()
            {
                //ReceiveMode = ServiceBusReceiveMode.ReceiveAndDelete,
                ReceiveMode = ServiceBusReceiveMode.PeekLock
            });

            var messageFromQueue = await receiver.ReceiveMessageAsync();

            var personFromQueue = JsonSerializer.Deserialize<Person>(messageFromQueue.Body);

            Console.WriteLine(personFromQueue.FirstName);
            Console.WriteLine(personFromQueue.LastName);
        }
    }
}
