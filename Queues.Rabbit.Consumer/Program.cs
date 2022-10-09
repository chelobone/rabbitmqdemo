using MassTransit;
using Queues.Rabbit.Consumers.Consumer;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

services.AddHealthChecks();
services.AddMassTransit(x =>
{
    x.AddConsumer<Consumer>();
    x.AddConsumer<ConsumerNotification>();
    x.UsingRabbitMq((context, config) =>
    {
        config.Host(new Uri("rabbitmq://localhost"), h =>
        {
            h.Username("{user}");
            h.Password("{pwd}");
        });
        config.ReceiveEndpoint("ticketQueue", ep =>
        {
            ep.PrefetchCount = 16;
            ep.UseMessageRetry(r => r.Interval(10, 10000));
            ep.ConfigureConsumer<Consumer>(context);
        });

        config.ReceiveEndpoint("notificationQueue", ep =>
        {
            ep.PrefetchCount = 16;
            ep.UseMessageRetry(r => r.Interval(3, 10000));
            ep.ConfigureConsumer<ConsumerNotification>(context);
        });
    });
});

// OPTIONAL, but can be used to configure the bus options
services.AddOptions<MassTransitHostOptions>()
    .Configure(options =>
    {
        // if specified, waits until the bus is started before
        // returning from IHostedService.StartAsync
        // default is false
        options.WaitUntilStarted = true;

        // if specified, limits the wait time when starting the bus
        options.StartTimeout = TimeSpan.FromSeconds(10);

        // if specified, limits the wait time when stopping the bus
        options.StopTimeout = TimeSpan.FromSeconds(30);
    });

// Add services to the container.

services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
