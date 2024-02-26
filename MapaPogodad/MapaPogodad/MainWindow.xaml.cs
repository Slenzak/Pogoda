using Microsoft.Maps.MapControl.WPF;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net;
using Newtonsoft.Json.Linq;


namespace MapaPogodad
{
    public partial class MainWindow : Window
    {
        string cords;
        public MainWindow()
        {
            InitializeComponent();
            myMap.Focus();
            myMap.Mode = new AerialMode(true);

        }
        private void MapWithPushpins_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            Point mousePosition = e.GetPosition(this);
            Microsoft.Maps.MapControl.WPF.Location pinLocation = myMap.ViewportPointToLocation(mousePosition);
            Pushpin pin = new Pushpin();
            pin.Location = pinLocation;
            cords = pinLocation.ToString();
            myMap.Children.Clear();
            myMap.Children.Add(pin);
            try
            {
                string[] loc = cords.Split(',');
                string firstloc = loc[0] +","+ loc[1];
                string secondloc = loc[2] + "," + loc[3];
                Trace.WriteLine(cords);
                Trace.WriteLine(secondloc+" "+firstloc);
                double firstlocdouble = Convert.ToDouble(firstloc);
                double secondlocdouble = Convert.ToDouble(secondloc);
                string url = $"https://api.openweathermap.org/data/3.0/onecall?lat={firstlocdouble}&lon={secondlocdouble}&appid=30c4a448b7f41d650026b05f4ddf15b4&units=metric&exclude=minutely";
                HttpWebRequest request= (HttpWebRequest)WebRequest.Create(url);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream resStream = response.GetResponseStream();
                StreamReader sr = new StreamReader(resStream);
                string responseBody = sr.ReadToEnd();
                JObject weatherData = JObject.Parse(responseBody);
                int temptime= new();

                foreach (var item in weatherData)
                {
                    if (Convert.ToString(item.Key) == "dt")
                    {
                        temptime = 1000 * Int32.Parse((string)item.Value);
                        var date = (new DateTime(1970, 1, 1)).AddMilliseconds(temptime);
                        Trace.WriteLine(item.Key+": "+date);
                    }else
                    Trace.WriteLine(item.Key + ": " + item.Value);
                }

            }
            catch (Exception ex)
            {

            }
        }
        
    }
}
