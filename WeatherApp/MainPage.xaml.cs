using Newtonsoft.Json;
using WeatherApp.Models;
using System.Windows.Input;

namespace WeatherApp
{
    public partial class MainPage : ContentPage
    {
        private double _temp;

        public double Temp
        {
            get { return _temp; }
            set { _temp = value; OnPropertyChanged(); }
        }


        private GPS _gps;


        private string _currentWeather;

        public MainPage()
        {
            InitializeComponent();
            ShowWeatherCommand = new Command(GetLatestWeather);
            _client = new HttpClient();
            _client.DefaultRequestHeaders.Add("Accept", "application/json");
            _gps = new GPS();
            BindingContext = this;

        }

        public ICommand ShowWeatherCommand { get; set; }

        private HttpClient _client;

        private void OnCounterClicked(object sender, EventArgs e)
        {

        }
        public async void GetLatestWeather(object parameters)
        {

            Location location = await _gps.GetCurrentLocation();
            double lat = location.Latitude;
            double lng = location.Longitude;

            string appid = "c40b6d8a0180a6a744694f3b0f0a0771";
            string response = await _client.GetStringAsync(new Uri($"https://api.openweathermap.org/data/2.5/weather?lat={lat}&lon={lng}&appid={appid}&units=metric"));

            WeatherForeCast currentweather = JsonConvert.DeserializeObject<WeatherForeCast>(response);

            if (currentweather != null)
            {
                Temp = currentweather.main.temp;
            }

        }

    }


}

