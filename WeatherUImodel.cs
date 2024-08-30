using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using WeatherAPP.Animations;
using Microsoft.Maui.Controls;


public class WeatherUImodel : INotifyPropertyChanged
{
    private readonly WeatherService _weatherService;
    private Color _backgroundColor;

    private GraphicsView _weatherAnim;
    private string _cityName;
    private string _temperature;
    private string _description;
    private string _icon;
    private string _windKph;
    private string _cloud;
    private string _humidity;
    private string _feelsLike;

    private string _errorMessage;
    private bool _isErrorVisible;

    public ICommand GetWeatherCommand { get; }
    public ICommand GetForecastCommand { get; }
    public ObservableCollection<ForecastDay> ForecastDays { get; set; }

    public event PropertyChangedEventHandler PropertyChanged;

    public WeatherUImodel()
    {
        _weatherService = new WeatherService();
        GetWeatherCommand = new Command(async () => await GetWeatherAsync());
        GetForecastCommand = new Command(async () => await GetForecastWeatherAsync());
        ForecastDays = new ObservableCollection<ForecastDay>();
        UpdateView();
    }

    private void UpdateView(string condition = null)
    {
        if (string.IsNullOrEmpty(condition))
        {
            BackgroundColor = Colors.LightGray; // Цвет по умолчанию, если данных нет
            WeatherAnim = null;
            return;
        }

        var hour = DateTime.Now.Hour;

        if (hour >= 5 && hour < 20)
        {
            // Дневное время
            if (condition.IndexOf("Rain", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                BackgroundColor = Colors.Gray; // Серый цвет для дождя
            }
            else if (condition.IndexOf("Clear", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                BackgroundColor = Colors.LightBlue; // Голубой цвет для ясного неба
            }
            else if (condition.IndexOf("Sunny", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                BackgroundColor = Colors.LightYellow; // Желтый цвет для солнца
                WeatherAnim = new SunAnimation();

            }
            else
            {
                BackgroundColor = Colors.LightYellow;
                WeatherAnim = null; // Общий дневной цвет
            }
        }
        else
        {
            // Ночное время
            BackgroundColor = Colors.Black;
            WeatherAnim = new NightAnimations(); // Темно-синий цвет для ночи
        }
    }

    public GraphicsView? WeatherAnim
    {
        get => _weatherAnim;
        set
        {
            _weatherAnim = value;
            OnPropertyChanged();
        }
    }

    public Color BackgroundColor
    {
        get => _backgroundColor;
        set
        {
            _backgroundColor = value;
            OnPropertyChanged();
        }
    }

    public string CityName
    {
        get => _cityName;
        set
        {
            _cityName = value;
            OnPropertyChanged();
        }
    }

    public string Temperature
    {
        get => _temperature;
        set
        {
            _temperature = value;
            OnPropertyChanged();
        }
    }

    public string Description
    {
        get => _description;
        set
        {
            _description = value;
            OnPropertyChanged();
        }
    }

    public string Icon
    {
        get => _icon;
        set
        {
            _icon = value;
            OnPropertyChanged();
        }
    }

    public string WindKph
    {
        get => _windKph;
        set
        {
            _windKph = value;
            OnPropertyChanged();
        }
    }

    public string Cloud
    {
        get => _cloud;
        set
        {
            _cloud = value;
            OnPropertyChanged();
        }
    }

    public string FeelsLike
    {
        get => _feelsLike;
        set
        {
            _feelsLike = value;
            OnPropertyChanged();
        }
    }

    public string Humidity
    {
        get => _humidity;
        set
        {
            _humidity = value;
            OnPropertyChanged();
        }
    }

    public string ErrorMessage
    {
        get => _errorMessage;
        set
        {
            _errorMessage = value;
            OnPropertyChanged();
        }
    }

    public bool IsErrorVisible
    {
        get => _isErrorVisible;
        set
        {
            _isErrorVisible = value;
            OnPropertyChanged();
        }
    }

    private async Task GetWeatherAsync()
    {
        IsErrorVisible = false;
        ErrorMessage = string.Empty;

        try
        {
            if (string.IsNullOrWhiteSpace(CityName))
            {
                IsErrorVisible = true;
                ErrorMessage = "Введите название города.";
                return;
            }

            var weather = await _weatherService.GetCurrentWeatherResponseAsync(CityName);

            if (weather != null)
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    Temperature = $"{weather.Current.Temp_C}°C";
                    FeelsLike = $"Ощущается как {weather.Current.Feelslike_C}°C";
                    Humidity = $"Влажность {weather.Current.Humidity}%";
                    WindKph = $"Ветер {weather.Current.Wind_Kph} км/ч";
                    Cloud = $"Облачность {weather.Current.Cloud}%";
                    Description = weather.Current.Condition.Text;
                    Icon = $"https:{weather.Current.Condition.Icon}";
                    UpdateView(weather.Current.Condition.Text);
                });
            }
            else
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    IsErrorVisible = true;
                    ErrorMessage = "Не удалось получить данные о погоде. Проверьте подключение к интернету или правильность введенного названия города.";
                });
            }
        }
        catch (Exception ex)
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                IsErrorVisible = true;
                ErrorMessage = "Произошла ошибка при получении данных. Проверьте подключение к интернету.";
            });

            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    private async Task GetForecastWeatherAsync()
    {
        IsErrorVisible = false;
        ErrorMessage = string.Empty;

        try
        {
            if (string.IsNullOrWhiteSpace(CityName))
            {
                IsErrorVisible = true;
                ErrorMessage = "Введите название города.";
                return;
            }

            var forecast = await _weatherService.GetForecastAsync(CityName);

            if (forecast != null && forecast.Forecast?.ForecastDays != null)
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    ForecastDays.Clear();
                    foreach (var day in forecast.Forecast.ForecastDays)
                    {
                        ForecastDays.Add(day);
                    }
                });
            }
            else
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    IsErrorVisible = true;
                    ErrorMessage = "Не удалось получить данные о погоде. Проверьте подключение к интернету или правильность введенного названия города.";
                });
            }
        }
        catch (Exception ex)
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                IsErrorVisible = true;
                ErrorMessage = "Произошла ошибка при получении данных. Проверьте подключение к интернету.";
            });

            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
