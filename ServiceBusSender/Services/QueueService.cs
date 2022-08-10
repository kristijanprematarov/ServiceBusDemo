using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Configuration;

namespace ServiceBusSender.Services
{
    public class QueueService : IQueueService
    {
        private readonly IConfiguration _configuration;

        public QueueService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendMessageAsync<T>(T serviceBusMessageData, string queueName)
        {
            var serviceBusClient = new ServiceBusClient(_configuration.GetConnectionString("AzureServiceBus"));

            var sender = serviceBusClient.CreateSender(queueName);

            var messageBody = JsonSerializer.Serialize(serviceBusMessageData);

            var serviceBusMessage = new ServiceBusMessage(messageBody);

            await sender.SendMessageAsync(serviceBusMessage);
        }
    }
}
