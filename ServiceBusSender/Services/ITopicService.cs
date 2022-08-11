using System.Threading.Tasks;

namespace ServiceBusSender.Services
{
    public interface ITopicService
    {
        Task SendMessageAsync<T>(T serviceBusMessageData, string topicName);
    }
}