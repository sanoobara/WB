using Newtonsoft.Json.Linq;

namespace traning_Tbot
{
    internal class ExchangeRate
    {
        const string s = "https://api.currencyapi.com/v3/latest?apikey=cur_live_SA4RC6qQKn2r8MAw3hXuIgCqw2QUhJUpp093hXPN";




        public static async Task<string> GetRate()
        {




            using (HttpClient client = new HttpClient())
            {


                HttpResponseMessage response = await client.GetAsync(s);
                string content = await response.Content.ReadAsStringAsync();

                JObject jsonObject = JObject.Parse(content);
                JObject factObject = jsonObject["data"] as JObject;
                factObject = factObject["RUB"] as JObject;

                // Получение значений полей
                double RUB = (double)factObject["value"];
                var resultMessage = $"{RUB}";

                return resultMessage;
            }


        }









    }
}
