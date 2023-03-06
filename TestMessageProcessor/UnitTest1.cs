namespace TestMessageProcessor
{
    using MessageProcessorLib;

    class Customer
    {
        public string? Name { get; set; }
        public string? Surname { get; set; }
    }
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task TestSendAndReceiveOneString()
        {
           
            List<string> messages = new List<string>();
            IMessageProcessor<string> processor = new MessageProcessor<string>("pulsar://localhost:6650");
            processor.MessageReceived += (sender, e) =>
            {
                messages.Add($"{e.MessagePublishTime}, {e.MessageReceivTime} e.Message");                              
            };
            await processor.AddProducer("test-topic","BasicProducer");
            await processor.AddConsumer("test-topic", "test-subscription","Cunsumer1");
            await processor.SendAsync("test-topic", "Ahoj svìte!");
       
            processor.ListeningOnce();
            processor.ListeningOnce();
            processor.ListeningOnce();
            
            await Task.Delay(1000);
            Assert.True(messages.Count == 1);
        }
        [Test]
        public async Task TestSendAndReceiveOneCustomer()
        {
            Customer customer = new Customer();
            customer.Name = "Petr";
            customer.Surname = "Novák";

            List<string> messages = new List<string>();
            IMessageProcessor<Customer> processor = new MessageProcessor<Customer>("pulsar://localhost:6650");
            processor.MessageReceived += (sender, e) =>
            {
                messages.Add($"{e.MessagePublishTime}, {e.MessageReceivTime} e.Message");
            };
            await processor.AddProducer("test-topic", "BasicProducer");
            await processor.AddConsumer("test-topic", "test-subscription", "Cunsumer1");
            await processor.SendAsync("test-topic", customer);

            processor.ListeningOnce();
            processor.ListeningOnce();
            processor.ListeningOnce();

            await Task.Delay(1000);
            Assert.True(messages.Count == 1);
        }
        [Test]
        public async Task TestSendOneAndReceiveTwo()
        {

            List<string> messages = new List<string>();
            IMessageProcessor<string> processor = new MessageProcessor<string>("pulsar://localhost:6650");
            IMessageProcessor<string> processor2 = new MessageProcessor<string>("pulsar://localhost:6650"); 
            
            await processor.AddProducer("test-topic", "BasicProducer");
            await processor.AddConsumer("test-topic", "test-subscription", "Cunsumer1");
            await processor2.AddConsumer("test-topic", "test-subscription2", "Cunsumer2");

            processor.MessageReceived += (sender, e) =>
            {
                messages.Add($"{e.MessagePublishTime}, {e.MessageReceivTime} e.Message");
            };
            processor2.MessageReceived += (sender, e) =>
            {
                messages.Add($"{e.MessagePublishTime}, {e.MessageReceivTime} e.Message");
            };

            await processor.SendAsync("test-topic", "Ahoj svìte!");

            processor.ListeningOnce();
            processor2.ListeningOnce();
          
            await Task.Delay(1000);
            Assert.True(messages.Count == 2);
        }
    }
}
