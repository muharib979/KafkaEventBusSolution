using NotificationService.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHostedService<KafkaConsumerService>();

builder.Services.AddControllers();


var app = builder.Build();



app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();