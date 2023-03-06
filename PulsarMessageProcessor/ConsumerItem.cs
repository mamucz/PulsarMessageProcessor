using MessageProcessorLib;
using Pulsar.Client.Api;
using Pulsar.Client.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageProcessorLib
{
    public class ConsumerItem<T> : IConsumerItem<T>
    {
        public event EventHandler<MessageReceivedEventArgs<T>>? MessageReceived;
        IConsumer<byte[]> consumer;
        public ConsumerItem(IConsumer<byte[]> consumer)
        {
            this.consumer = consumer;
        }

        private async void ReceiveMessageAsync()
        {
            var message = await consumer.ReceiveAsync();            
            MessageReceived?.Invoke(this, new MessageReceivedEventArgs<T>(message, consumer.Topic));
            await consumer.AcknowledgeAsync(message.MessageId);
        }

        public void ListeningAsync()
        {
            ReceiveMessageAsync();
        }
    }
}
