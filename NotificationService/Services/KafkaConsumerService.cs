using Confluent.Kafka;
using System.Text.Json;

namespace NotificationService.Services
{
    public class KafkaConsumerService : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = "localhost:9092",
                GroupId = "notification-group",
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            using var consumer = new ConsumerBuilder<Ignore, string>(config).Build();

            consumer.Subscribe("order-topic");

            while (!stoppingToken.IsCancellationRequested)
            {
                var consumeResult = consumer.Consume(stoppingToken);

                var order = JsonSerializer.Deserialize<Order>(consumeResult.Message.Value);
                Console.WriteLine($"[NotificationService] Order Received: {order?.OrderId} - {order?.Product} (${order?.Price})");
            }
        }
    }

    public class Order
    {
        public int OrderId { get; set; }
        public string Product { get; set; }
        public double Price { get; set; }
    }
}