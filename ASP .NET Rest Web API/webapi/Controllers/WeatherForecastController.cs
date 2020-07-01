using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using webapi.Models;
using System.Text;
using System.Net.Http;
using System.Net;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace webapi.Controllers
{
    public class WeatherForecastController : ControllerBase
    {
        // create a logger to log any errors occured during the application
        private readonly ILogger<WeatherForecastController> _logger;
        private static readonly HttpClient client = new HttpClient();

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [Route("getResult")]
        [HttpPost]
        public async Task<IActionResult> getResult([FromBody] Data data2)
        {
            if (string.IsNullOrEmpty(data2.text))
            {
                return Content("Please enter a string to evaluate");
            }
            var values = new Dictionary<string, string>
            {
                { "expr", data2.text}
            };

            var json = JsonConvert.SerializeObject(values);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            var url = "http://api.mathjs.org/v4/";
            using var client = new HttpClient();
            var response = await client.PostAsync(url, data);
            string serverResponse = response.Content.ReadAsStringAsync().Result;
            JObject jsonResult = JObject.Parse(serverResponse);
            string result = jsonResult["result"].ToString();
            if(result == "null" || string.IsNullOrEmpty(result))
            {
                return Content("error");
            }
            else
            {
                return Content(result);
            }
        }

    }
}
