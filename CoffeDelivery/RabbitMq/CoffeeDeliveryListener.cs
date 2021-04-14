using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using System.Threading;
using RabbitMQ.Client.Events;
using System.Text;
using System.Diagnostics;
using CoffeDelivery.SignalR;

namespace CoffeDelivery.RabbitMq
{
    public class CoffeeDeliveryListener : BackgroundService
    {
        private IConnection _connection; 
        private IModel _channel;
        private readonly IConfiguration _configuration;
        private readonly CoffeeDeliveryHub _coffeDeliveryHub;
        public CoffeeDeliveryListener(IConfiguration configuration, CoffeeDeliveryHub coffeDeliveryHub)
        {
            _configuration = configuration;
            _coffeDeliveryHub = coffeDeliveryHub;
            InitializeRabbitMqListener(); 
        }
        private void InitializeRabbitMqListener()
        {
            var factory = new ConnectionFactory
            {
                Uri = new Uri(_configuration.GetConnectionString("MqCloudConnectionString")) 
            };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.QueueDeclare(queue: _configuration["RabbitMq:DbChangedQueueName"],
                                    durable: false,
                                    exclusive: false,
                                    autoDelete: false,
                                    arguments: null);  
        }
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received +=async (ch, ea) =>
            {
                var content = Encoding.UTF8.GetString(ea.Body.ToArray());
                Debug.WriteLine(content);
                await _coffeDeliveryHub.SendMessageAsync(content);
                _channel.BasicAck(ea.DeliveryTag, false);
            };
            _channel.BasicConsume(_configuration["RabbitMq:DbChangedQueueName"], false, consumer);
            return Task.CompletedTask;
        }
    }
}
