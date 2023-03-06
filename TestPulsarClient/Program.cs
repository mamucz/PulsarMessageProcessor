using Pulsar.Client.Api;
using Pulsar.Client.Common;
using System;
using System.Text;
using System.Threading.Tasks;
// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");
await PulsarTest.Test();
Console.WriteLine("End World!");

public static class PulsarTest
{
    public static async Task Test()
    {
        const string serviceUrl = "pulsar://localhost:6650";
        const string subscriptionName = "my-subscription";
        var topicName = $"my-topic-{DateTime.Now.Ticks}";

        var client = await new PulsarClientBuilder()
            .ServiceUrl(serviceUrl)
            .BuildAsync();

        var producer = await client.NewProducer()
            .Topic(topicName)
            .CreateAsync();

        var consumer = await client.NewConsumer()
            .Topic(topicName)
            .SubscriptionName(subscriptionName)
            .SubscribeAsync();

        var messageId = await producer.SendAsync(Encoding.UTF8.GetBytes($"Sent from C# at '{DateTime.Now}'"));
        Console.WriteLine($"MessageId is: '{messageId}'");

        var message = await consumer.ReceiveAsync();
        Console.WriteLine($"Received: {Encoding.UTF8.GetString(message.Data)}");

        await consumer.AcknowledgeAsync(message.MessageId);
    }
}

