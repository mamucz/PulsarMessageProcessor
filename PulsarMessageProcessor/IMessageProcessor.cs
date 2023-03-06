using Pulsar.Client.Api;
using Pulsar.Client.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageProcessorLib
{
   
    //public delegate void MessageReceivedEventHandler(object sender, MessageReceivedEventArgs<T> e);
    
    public interface IMessageProcessor<T>
    {
        public event EventHandler<MessageReceivedEventArgs<T>> MessageReceived;

        public string ServiceUrl { get; }        

        public Task<IConsumer<byte[]>> AddConsumer(string topicName, string subscriptionName, string name);
        public Task<IProducer<byte[]>> AddProducer(string topicName, string name);
        public void RemoveConsumer(string topicName);
        public void RemoveProducer(string topicName); 

        public Task<MessageId> SendAsync(string topicName, 
                                         T message,                                                
                                         string? key = null,
                                         IReadOnlyDictionary<string, string>? properties = null,
                                         long? deliveryAt = null,
                                         long? sequenceId = null,
                                         byte[]? keyBytes = null,
                                         byte[]? orderingKey = null,
                                         long? eventTime = null);

        public void ListeningOnce();
        //public Task Start();
        //public Task Stop();
    }
}
