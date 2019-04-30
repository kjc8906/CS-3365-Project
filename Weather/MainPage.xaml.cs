using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Net.Http;



namespace Weather
{


    public partial class MainPage : ContentPage
    {
        string current_zip = " ";
        string zip_check = "";
        bool in_refresh = false;
        //string zip_code = "";
        string API_KEY = "2bcba8ce2d22934fa4fba2e9c7c46014";
        TimeSpan interval = new TimeSpan(2000);
        HttpClient webclient;

        public MainPage()
        {
            InitializeComponent();
            webclient = new HttpClient();
        }

        public async Task<string> GetWeatherData(string URL)
        {
            string weatherData = null;

            var response = await webclient.GetAsync(URL);
            if (response.IsSuccessStatusCode)
            {
                weatherData = await response.Content.ReadAsStringAsync();
                return weatherData;
            }
            else
            {
                return null;
            }
        }
        async void SearchZIP(object sender, EventArgs e)
        {
            zip_check = zip_code.Text;
            if (string.IsNullOrWhiteSpace(zip_check) == true)
            {
                Label1.Text = "Invalid Input Please Try Again";
                await Task.Delay(1000);
                Label1.Text = "Search Weather By Zip Code";
            }
            else if (string.IsNullOrWhiteSpace(zip_check) == false)
            {
                current_zip = zip_code.Text;
                PrintData(zip_check);
            }
        }
        async void Refresh(object sender, EventArgs e)
        {
            zip_check = current_zip;
            if (string.IsNullOrWhiteSpace(zip_check) == true)
            {
                Label1.Text = "Need Zip Code to refresh";
                await Task.Delay(1000);
                Label1.Text = "Search Weather By Zip Code";
                zip_check = zip_code.Text;
            }
            else if (string.IsNullOrWhiteSpace(zip_check) == false)
            {
                PrintData(zip_check);
            }
        }
        async void PrintData(string zip)
        {
            string URL = "http://api.openweathermap.org/data/2.5/weather?zip=" + zip_check + ",us&appid=" + API_KEY + "&units=imperial";
            string Data = await GetWeatherData(URL);
            if (string.IsNullOrWhiteSpace(Data) == true)
            {
                Label1.Text = "Invalid Input Please Try Again";
                await Task.Delay(1000);
                Label1.Text = "Search Weather By Zip Code";
            }
            else
            {
                int nameA = Data.IndexOf("name") + 7;
                int nameB = Data.IndexOf("cod") - 3;
                int nameDif = nameB - nameA;
                string cityName = Data.Substring(nameA, nameDif);

                int cloudsA = Data.IndexOf("description") + 14;
                int cloudsB = Data.IndexOf("icon") - 3;
                int cloudsDif = cloudsB - cloudsA;
                string cloudDescription = Data.Substring(cloudsA, cloudsDif);

                int tempA = Data.IndexOf("temp") + 6;
                int tempB = Data.IndexOf("pressure") - 2;
                int tempDif = tempB - tempA;
                string temp = Data.Substring(tempA, tempDif);

                int pressureA = tempB + 12;
                int pressureB = Data.IndexOf("humidity") - 2;
                int pressureDif = pressureB - pressureA;
                string pressure = Data.Substring(pressureA, pressureDif);

                int humidityA = pressureB + 12;
                int humidityB = pressureB + 14;
                int humidityDif = humidityB - humidityA;
                string humidity = Data.Substring(humidityA, humidityDif);

                Label2.Text = cityName;
                Label3.Text = "Cloud Coverage: " + cloudDescription;
                Label4.Text = "Temp: " + temp;
                Label5.Text = "Pressure: " + pressure;
                Label6.Text = "Humidity: " + humidity;
            }
            await Task.Delay(1000);
        }
        async void Clear(object sender, EventArgs e)
        {
            zip_code.Text = "";
            zip_code.Placeholder = "Zip Code";
            current_zip = "";
            Label2.Text = "";
            Label3.Text = "";
            Label4.Text = "";
            Label5.Text = "";
            Label6.Text = "";

            await Task.Delay(1000);
        }
    }
}