using System.Text.Json;

namespace HNGSTAGEONE.Service
{
    public class LocationService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _openWeatherMapApiKey = "17e08ebaa1abcc538e4ff9b81cb6c5c6";

        public LocationService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<string> GetLocationAsync(string ip)
        {
            var client = _httpClientFactory.CreateClient();
            try
            {
                if (ip.StartsWith("::ffff:")) // Handle IPv6-mapped IPv4 addresses
                {
                    ip = ip.Substring(7);
                }

                var response = await client.GetStringAsync($"https://ipinfo.io/{ip}/json");
                var locationData = JsonDocument.Parse(response);
                string city = locationData.RootElement.GetProperty("city").GetString();
                return city;
            }
            catch (Exception ex)
            {
                // Log the exception (use a logging framework in a real application)
                Console.WriteLine($"Error fetching location for IP {ip}: {ex.Message}");
                return "unknown";
            }
        }

        public async Task<string> GetTemperatureAsync(string city)
        {
            var client = _httpClientFactory.CreateClient();
            var url = $"http://api.openweathermap.org/data/2.5/weather?q={city}&appid={_openWeatherMapApiKey}&units=metric";
            try
            {
                var response = await client.GetStringAsync(url);
                var json = JsonDocument.Parse(response);
                var temp = json.RootElement.GetProperty("main").GetProperty("temp").ToString();
                return $"{temp} degrees Celsius";
            }
            catch (Exception ex)
            {
                // Log the exception (use a logging framework in a real application)
                Console.WriteLine($"Error fetching temperature for city {city}: {ex.Message}");
                return "unknown degrees Celsius";
            }
        }
    }



}
