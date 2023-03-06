using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageProcessorLib
{
    public interface IMessageReceivedEventsArgs<T>
    {
        //všechny argumenty pro příjem zprávy přes Pulsar
        public string Topic { get;}
        public Pulsar.Client.Common.MessageId MessageId { get; }
        public T? messageData { get;  }
        public string MessageKey { get;  }
        public DateTime MessagePublishTime { get; }
        public DateTime MessageReceivTime { get; }
    }
}
