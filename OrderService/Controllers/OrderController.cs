using Confluent.Kafka;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace OrderService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] Order order)
        {
            var config = new ProducerConfig { BootstrapServers = "localhost:9092" };
            using var producer = new ProducerBuilder<Null, string>(config).Build();
            var message = JsonSerializer.Serialize(order);
            await producer.ProduceAsync("order-topic", new Message<Null, string> { Value = message });
            return Ok(new { status = "Order sent to Kafka", order });
        }
    }

    public class Order
    {
        public int OrderId { get; set; }
        public string Product { get; set; }
        public double Price { get; set; }
    }
}