using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Config
{
    public class RabbitMQConfig
    {
        protected readonly IConnection _connection;
        protected readonly IModel _channel;

        public RabbitMQConfig(IConfiguration configuration)
        {
            var factory = new ConnectionFactory
            {
                Uri = new Uri(configuration["RabbitMQ:Url"])
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
        }
    }
}
