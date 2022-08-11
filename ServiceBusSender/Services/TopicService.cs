using System.Text.Json;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Configuration;

namespace ServiceBusSender.Services
{
    public class TopicService : ITopicService
    {
        private readonly IConfiguration _configuration;

        public TopicService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendMessageAsync<T>(T serviceBusMessageData, string topicName)
        {
            var serviceBusClient = new ServiceBusClient(_configuration.GetConnectionString("AzureServiceBusTopic"));

            var sender = serviceBusClient.CreateSender(topicName);

            var messageBody = JsonSerializer.Serialize(serviceBusMessageData);

            var serviceBusMessage = new ServiceBusMessage(messageBody) { SessionId = "S1" };

            await sender.SendMessageAsync(serviceBusMessage);
        }
    }
}