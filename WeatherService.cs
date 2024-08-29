using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.VisualBasic;
using Newtonsoft.Json;

public class WeatherService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey = "b32dac3879f34100abe100738241308";
    private const string BaseUrl = "http://api.weatherapi.com/v1";

    public WeatherService()
    {
        var handler = new HttpClientHandler();
        handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;
        _httpClient = new HttpClient(handler);
    }

    public async Task<CurrentWeatherAPI> GetCurrentWeatherResponseAsync(string cityName)
    {
        string url = $"{BaseUrl}/current.json?key={_apiKey}&q={cityName}";
        try
        {
            var response = await _httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var weatherData = JsonConvert.DeserializeObject<CurrentWeatherAPI>(content);
                return weatherData;
            }
            else
            {
                return null;
            }
        }
        catch (HttpRequestException)
        {
            return null;
        }
    }
    public async Task<WeatherForecast> GetForecastAsync(string cityName)
    {
        string url = $"{BaseUrl}/forecast.json?key={_apiKey}&q={cityName}&days=7";
        try
        {
            var res = await _httpClient.GetAsync(url);
            if (res.IsSuccessStatusCode)
            {
                var content = await res.Content.ReadAsStringAsync();
                var weatherForecastData = JsonConvert.DeserializeObject<WeatherForecast>(content);
                return weatherForecastData;
            }
            else
            {
                return null;
            }
        }
        catch (HttpRequestException)
        {
            return null;
        }
    }

}