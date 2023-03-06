
using Pulsar.Client.Api;
using Pulsar.Client.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MessageProcessorLib
{
    public class MessageProcessor<T> : IMessageProcessor<T>
    {
        public event EventHandler<MessageReceivedEventArgs<T>>? MessageReceived;
        Dictionary<string, Pulsar.Client.Api.IConsumer<byte[]>> consumers = new();
        Dictionary<string, Pulsar.Client.Api.IProducer<byte[]>> producers = new();
      
        private string _serviceUrl;
        private PulsarClient? client;

        public string ServiceUrl => _serviceUrl;

        public async Task<IConsumer<byte[]>> AddConsumer(string topicName, string subscriptionName, string name)
        {
            if (client == null)
            {
                throw new Exception("Client is null");
            }
            var consumer = await client.NewConsumer()
            .Topic(topicName).ConsumerName(name)
            .SubscriptionName(subscriptionName)
            .SubscribeAsync();
            consumers.Add(topicName, consumer);
            return consumer;
        }

        public async Task<IProducer<byte[]>> AddProducer(string topicName, string name)
        {
            if (client == null)
            {
                throw new Exception("Client is null");
            }
            var producer = await client.NewProducer()
            .Topic(topicName).ProducerName(name)
            .CreateAsync();
            producers.Add(topicName, producer);
            return producer;
        }
        public void ListeningOnce()
        {
            foreach(var consumer in consumers)
            {
                var conitem = new ConsumerItem<T>(consumer.Value);
                conitem.MessageReceived += Conitem_MessageReceived;
                conitem.ListeningAsync();
            }
        }

      
        private void Conitem_MessageReceived(object? sender, MessageReceivedEventArgs<T> e)
        {
           if (MessageReceived!=null)
            {
                MessageReceived(this, e);
            }
        }

        public Task Stop()
        {
            throw new NotImplementedException();
        }

        private async void InitClientAsync(string serviceUrl)
        {
            client = await new PulsarClientBuilder()
           .ServiceUrl(serviceUrl)
           .BuildAsync();
        }

        public void RemoveConsumer(string topicName)
        {
            consumers.Remove(topicName);
        }

        public void RemoveProducer(string topicName)
        {
            producers.Remove(topicName);
        }

        public async Task<MessageId> SendAsync(string topicName, T message, string? key = null, IReadOnlyDictionary<string, string>? properties = null, long? deliveryAt = null, long? sequenceId = null, byte[]? keyBytes = null, byte[]? orderingKey = null, long? eventTime = null)
        {
            bool success = producers.TryGetValue(topicName, out IProducer<byte[]>? producer);
            if (!success || producer==null)
            {
                throw new Exception($"Producer for topic {topicName} not found");
            }
            byte[] payload;
            
            payload = Encoding.UTF8.GetBytes(SerializeToJson(message));
            MessageBuilder < byte[]> builder = producer.NewMessage(payload,key, properties,deliveryAt, sequenceId,keyBytes, orderingKey, eventTime);
            return await producer.SendAsync(builder);            
        }

        public string SerializeToJson(T dataObject)
        {
            return JsonSerializer.Serialize<T>(dataObject);
        }

        public T? DeserializeFromJson(string json)
        {
            return JsonSerializer.Deserialize<T>(json);
        }

        public MessageProcessor(string serviceUrl)
        {
            _serviceUrl = serviceUrl;
            InitClientAsync(serviceUrl);
        }

    }
}
