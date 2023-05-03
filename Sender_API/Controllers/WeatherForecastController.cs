using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System.Text;
using Newtonsoft.Json;

namespace Sender_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [Route("get")]
        public IEnumerable<WeatherForecast> Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpPost]
        [Route("message")]
        public object PostMessage()
        {
            var factory = new ConnectionFactory() {
                HostName = "localhost"
            };
       
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare("AV AX4B", false, false, false, null);

                    var rng = new Random();

                    var weatherForecast = new WeatherForecast
                    {
                        Date = DateTime.Now,
                        TemperatureC = rng.Next(-20, 55),
                        Summary = Summaries[rng.Next(Summaries.Length)]
                    };

                    var weatherForecastJson = JsonConvert.SerializeObject(weatherForecast);

                    var body = Encoding.UTF8.GetBytes(weatherForecastJson);

                    channel.BasicPublish("", "AV AX4B", null, body);
                }
            }

            return new { Message= "Endpoint finalizado"+ AmqpTcpEndpoint.UseDefaultPort };
        }
    }
}
