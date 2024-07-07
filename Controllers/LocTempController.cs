using HNGSTAGEONE.Model;
using HNGSTAGEONE.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;

namespace HNGSTAGEONE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocTempController : ControllerBase
    {
        /*        private readonly LocationService _locationService;

                public LocTempController(LocationService locationService)
                {
                    _locationService = locationService;
                }*/

        /* [HttpGet("hello")]
         public async Task<IActionResult> GetHello(string visitor_name = "Mark")
         {
             var clientIp = HttpContext.Connection.RemoteIpAddress.ToString();
             var location = await GetLocationAsync(clientIp);
             var temperature = await GetTemperatureAsync(location);

             var response = new Hello
             {
                 ClientIP = clientIp,
                 Location = location,
                 Greetings = $"Hello, {visitor_name}!, the temperature is {temperature} in {location}"
             };

             return Ok(response);
         }

         private async Task<string> GetLocationAsync(string ip)
         {
             if (ip == "::1" || ip == "127.0.0.1")
             {
                 return "London";
             }

             try
             {
                 var url = $"http://ip-api.com/json/{ip}";
                 var client = new HttpClient();
                 var response = await client.GetStringAsync(url);

                 var json = JObject.Parse(response);


                 if (json["status"].ToString().ToLower() == "success")
                 {

                     if (json["city"] != null)
                     {
                         return json["city"].ToString();
                     }
                     else
                     {
                         return "Unknown";
                     }
                 }
                 else
                 {

                     string message = json["message"].ToString();
                     Console.WriteLine($"IP geolocation API returned status: fail. Message: {message}");
                     throw new Exception(message);
                 }
             }
             catch (Exception ex)
             {

                 Console.WriteLine($"Error in GetLocationAsync: {ex.Message}");
                 throw new Exception(ex.Message);

             }
         }


         private async Task<string> GetTemperatureAsync(string city)
         {
             var apiKey = "17e08ebaa1abcc538e4ff9b81cb6c5c6";
             var url = $"http://api.openweathermap.org/data/2.5/weather?q={city}&appid={apiKey}&units=metric";
             var client = new HttpClient();
             var response = await client.GetStringAsync(url);
             var json = JObject.Parse(response);
             var temp = json["main"]["temp"].ToString();
             return $"{temp} degrees Celsius";
         }*/
        [HttpGet("hello")]
        public async Task<IActionResult> GetHello(string visitor_name = "Mark")
        {
            var clientIp = HttpContext.Connection.RemoteIpAddress.ToString();
            var location = await GetLocationAsync(clientIp);
            var temperature = await GetTemperatureAsync(location);

            var response = new Hello
            {
                ClientIP = clientIp,
                Location = location,
                Greetings = $"Hello, {visitor_name}!, the temperature is {temperature} in {location}"
            };

            return Ok(response);
        }

        private async Task<string> GetLocationAsync(string ip)
        {
            if (ip == "::1" || ip == "127.0.0.1" || IsPrivateIp(ip))
            {
                return "London";
            }

            try
            {
                var url = $"http://ip-api.com/json/{ip}";
                var client = new HttpClient();
                var response = await client.GetStringAsync(url);

                var json = JObject.Parse(response);

                if (json["status"].ToString().ToLower() == "success")
                {
                    if (json["city"] != null)
                    {
                        return json["city"].ToString();
                    }
                    else
                    {
                        return "London";
                    }
                }
                else
                {
                    string message = json["message"].ToString();
                    Console.WriteLine($"IP geolocation API returned status: fail. Message: {message}");
                    return "London";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetLocationAsync: {ex.Message}");
                return "London";
            }
        }

        private bool IsPrivateIp(string ip)
        {
            var privateRanges = new List<string>
    {
        "10.",
        "172.16.",
        "192.168."
    };

            return privateRanges.Any(range => ip.StartsWith(range));
        }

        private async Task<string> GetTemperatureAsync(string city)
        {
            try
            {
                var apiKey = "17e08ebaa1abcc538e4ff9b81cb6c5c6";
                var url = $"http://api.openweathermap.org/data/2.5/weather?q={city}&appid={apiKey}&units=metric";
                var client = new HttpClient();
                var response = await client.GetStringAsync(url);
                var json = JObject.Parse(response);
                var temp = json["main"]["temp"].ToString();
                return $"{temp} degrees Celsius";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetTemperatureAsync: {ex.Message}");
                return "15 degrees Celsius"; // Default temperature
            }
        }


    }
}
