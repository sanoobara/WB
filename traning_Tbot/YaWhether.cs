﻿using Newtonsoft.Json.Linq;

namespace traning_Tbot
{
    internal class YaWeather
    {
        const string token = "c6426c21-3c1d-4481-b867-89163e09a7ac";
        const string baseUrlResponce = "https://api.weather.yandex.ru/v2/informers?";
        const string lat = "lat=44.75928";
        const string lon = "lon=34.46293";
        const string lang = "lang=ru_RU";
        public string urlReponse = baseUrlResponce + lat + "&" + lon + "&" + lang;




        public YaWeather()
        {

            //this.urlReponse = baseUrlResponce + lat + "&" + lon + "&" + lang;

            Console.WriteLine(urlReponse);


        }

        public static async Task<string> GetWeather()
        {
            const string token = "c6426c21-3c1d-4481-b867-89163e09a7ac";
            const string baseUrlResponce = "https://api.weather.yandex.ru/v2/informers?";
            const string lat = "lat=44.75928";
            const string lon = "lon=34.46293";
            const string lang = "lang=ru_RU";
            string urlReponse = baseUrlResponce + lat + "&" + lon + "&" + lang;



            var resultMessage = $"Погода в Алуште:\n";
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("X-Yandex-API-Key", token);

                HttpResponseMessage response = await client.GetAsync(urlReponse);
                string content = await response.Content.ReadAsStringAsync();

                JObject jsonObject = JObject.Parse(content);
                JObject factObject = jsonObject["fact"] as JObject;

                // Получение значений полей
                double temp = (double)factObject["temp"];
                resultMessage += $"Температура: {temp} °C\n";
                double feelsLike = (double)factObject["feels_like"];
                resultMessage += $"Ощущается как: {feelsLike} °C\n";
                //string icon = (string)factObject["icon"];
                double windSpeed = (double)factObject["wind_speed"];
                resultMessage += $"Скорость ветра: {windSpeed} м/с\n";
                int pressureMm = (int)factObject["pressure_mm"];
                resultMessage += $"Давление: {pressureMm} мм рт.ст.\n";
                int humidity = (int)factObject["humidity"];
                resultMessage += $"Влажность: {humidity}%\n";

                string daytime = (string)factObject["daytime"];
                bool polar = (bool)factObject["polar"];
                string season = (string)factObject["season"];
                long obsTime = (long)factObject["obs_time"];

                return resultMessage;
            }


        }



        public void GetWether1()
        {

            using (var client = new HttpClient())
            {


                client.DefaultRequestHeaders.Add("X-Yandex-API-Key", token);
                var response = client.GetStringAsync(urlReponse);

                Console.WriteLine(response);

            }

        }

    }
}
