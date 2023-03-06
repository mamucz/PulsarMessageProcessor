using Pulsar.Client.Api;
using Pulsar.Client.Common;
using System.Text;
using System.Text.Json;

namespace MessageProcessorLib
{
    public class MessageReceivedEventArgs<T> : EventArgs, IMessageReceivedEventsArgs<T>
    {
        
        //privátní proměnné pro vlastnosti
        private string _topic;
    
        private Pulsar.Client.Common.MessageId _messageId;
        private T? _messageData;
        private string _messageKey;
        private DateTime _messagePublishTime;
        private DateTime _messageReceivTime;

        //všechny argumenty pro příjem zprávy přes Pulsar
        public string Topic { get => _topic; }       
        public Pulsar.Client.Common.MessageId MessageId { get =>_messageId; }
        public T? messageData { get => _messageData; }
        public string MessageKey { get => _messageKey; }
        public DateTime MessagePublishTime { get => _messagePublishTime; }
        public DateTime MessageReceivTime { get => _messageReceivTime; }

        public MessageReceivedEventArgs(Message<byte[]> message, string topic)
        {
            _messageReceivTime = DateTime.Now;
            _topic = topic;
            _messageId = message.MessageId;

             string jsonString = Encoding.UTF8.GetString(message.Data);
            _messageData = JsonSerializer.Deserialize<T>(jsonString);
            _messageKey = message.Key;
            _messagePublishTime = DateTime.Now.FromUnixTime(message.PublishTime);
        }

    }    
}