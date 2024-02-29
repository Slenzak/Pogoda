using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Net.Http;
using Newtonsoft.Json;
using Microsoft.Maps.MapControl.WPF;
using System.Drawing;
using System.Linq.Expressions;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;


namespace MapaPogodad
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        public void DoubleClick(object sender, MouseButtonEventArgs e)
        { 
            e.Handled = true;
            var point =e.GetPosition(myMap);
            var location = myMap.ViewportPointToLocation(point);
            Pushpin pin = new Pushpin();
            Microsoft.Maps.MapControl.WPF.Location pinLocation = myMap.ViewportPointToLocation(e.GetPosition(this));
            pin.Location = pinLocation;
            myMap.Children.Clear();
            myMap.Children.Add(pin);
            var queryLocation = "&lat=" + location.Latitude + "&lon=" + location.Longitude;
            Query(queryLocation);
        }

        public void textQuery(object sender, EventArgs e)
        {
            HttpClient client = new HttpClient();

            string uriString = "http://api.openweathermap.org/geo/1.0/direct?limit=1&appid=30c4a448b7f41d650026b05f4ddf15b4&q=" + textInput.Text.Replace(' ', '+');

            Uri uri = new Uri(uriString);

            var response = client.GetAsync(uri).Result;

            if (!response.IsSuccessStatusCode)
            {
                //error
            }
            else
            {
                var responseContent = response.Content.ReadAsStringAsync().Result;
                GeoApiResponse geoApiResponse = JsonConvert.DeserializeObject<GeoApiResponse[]>(responseContent)[0];

                Query("&lat=" + geoApiResponse.lat + "&lon=" + geoApiResponse.lon);
            }
        }

        private void Query(string text)
        {
            HttpClient client = new HttpClient();

            string uriString = "https://api.openweathermap.org/data/2.5/forecast?appid=30c4a448b7f41d650026b05f4ddf15b4&cnt=3&units=metric" + text;

            Uri uri = new Uri(uriString);

            var response = client.GetAsync(uri).Result;

            if (!response.IsSuccessStatusCode)
            {
                //error
            }
            else
            {
                var responseContent = response.Content.ReadAsStringAsync().Result;
                OWApiResponse forecast = JsonConvert.DeserializeObject<OWApiResponse>(responseContent);

                weather.Text = "";

                for (int i = 0; i < 3; i++)
                {
                    weather.Text += $"{forecast.list[i].dt_txt}\n" +
                        $"  temperature: {forecast.list[i].main.temp}°C" +
                        $"  sensed temperature {forecast.list[i].main.feels_like}°C\n" +
                        $"  pressure: {forecast.list[i].main.pressure}hPa\n" +
                        $"  weather: {forecast.list[i].weather[0].description}\n" +
                        $"  humidity: {forecast.list[i].main.humidity}%" +
                        $"  propability of rain: {forecast.list[i].pop}%\n" +
                        $"  cloudiness: {forecast.list[i].clouds.all}%\n" +
                        $"\n";
                }
            }
        }
    }
    public class GeoApiResponse
    {
        public string name;
        public object local_names;
        public double lon;
        public double lat;
        public string country;
        public string? state;
    }
    public class Main
    {
        public double temp { get; set; }
        public double feels_like { get; set; }
        public double temp_min { get; set; }
        public double temp_max { get; set; }
        public int pressure { get; set; }
        public int sea_level { get; set; }
        public int grnd_level { get; set; }
        public int humidity { get; set; }
        public double temp_kf { get; set; }

    }
    public class Weather
    {
        public int id { get; set; }
        public string main { get; set; }
        public string description { get; set; }
        public string icon { get; set; }

    }
    public class Clouds
    {
        public int all { get; set; }

    }
    public class Wind
    {
        public double speed { get; set; }
        public int deg { get; set; }
        public double gust { get; set; }

    }
    public class Rain
    {
        public double h { get; set; }

    }
    public class Sys
    {
        public string pod { get; set; }

    }
    public class List
    {
        public int dt { get; set; }
        public Main main { get; set; }
        public IList<Weather> weather { get; set; }
        public Clouds clouds { get; set; }
        public Wind wind { get; set; }
        public int visibility { get; set; }
        public double pop { get; set; }
        public Rain rain { get; set; }
        public Sys sys { get; set; }
        public DateTime dt_txt { get; set; }

    }
    public class Coord
    {
        public double lat { get; set; }
        public double lon { get; set; }

    }
    public class City
    {
        public int id { get; set; }
        public string name { get; set; }
        public Coord coord { get; set; }
        public string country { get; set; }
        public int population { get; set; }
        public int timezone { get; set; }
        public int sunrise { get; set; }
        public int sunset { get; set; }

    }
    public class OWApiResponse
    {
        public string cod { get; set; }
        public int message { get; set; }
        public int cnt { get; set; }
        public IList<List> list { get; set; }
        public City city { get; set; }

    }
}
