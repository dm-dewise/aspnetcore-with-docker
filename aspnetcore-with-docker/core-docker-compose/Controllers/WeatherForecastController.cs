using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace core_docker_compose.Controllers
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

        [HttpGet("GetEvent")]
        public async Task GetEvent()
        {
            var response = Response;
            response.Headers.Add("Content-Type", "text/event-stream");
            for (int i = 0; true; i++)
            {
                await response.WriteAsync($"data: Controller {i} at {DateTime.Now}\r\r");
                response.Body.Flush();
                await Task.Delay(5 * 1000);
            }
        }

        //React App
        //export default function EventSource()
        //{
        //    const eventSource = new EventSource("http://localhost:5000/WeatherForecast/GetEvent");
        //    const [value, setValue] = useState("");

        //    useEffect(() =>
        //    {
        //        eventSource.onmessage = e =>
        //        {
        //            setValue(e.data);
        //        }
        //    }, []);

        //    return (
        //        <div className="ms-Grid-row">
        //            { value }
        //        </div>
        //    )
        //}
    }
}
