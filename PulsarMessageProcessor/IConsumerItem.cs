using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pulsar.Client.Api;
using Pulsar.Client.Common;

namespace MessageProcessorLib
{
    public interface IConsumerItem<T>
    {
        public event EventHandler<MessageReceivedEventArgs<T>> MessageReceived;
        public void ListeningAsync();
    }
}
