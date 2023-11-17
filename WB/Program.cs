// See https://aka.ms/new-console-template for more information

using Microsoft.Extensions.Logging;
using NLog;
using NLog.Config;

var token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJhY2Nlc3NJRCI6Ijg3MWE4OTVhLTM3YmEtNDhjZC1iZDRiLTMyODUyMWZkZTUxYiJ9.50kj13J30fZZ2eQrpOVnckL9OLuAIdcAWJW30xt5U0A";
var url = "https://suppliers-api.wildberries.ru/api/v3/orders/new";
var url2 = "https://suppliers-api.wildberries.ru/api/v3/orders/status";
var url3 = "https://suppliers-api.wildberries.ru/api/v3/orders";
Logger logger= LogManager.GetCurrentClassLogger();

int timeout = 60000;

while (true)
{
    try
    {
        using (var client = new HttpClient())
        {
            
            client.DefaultRequestHeaders.Add("Authorization", token);
            var response = await client.GetStringAsync(url);
            
            Console.WriteLine(response);
            logger.Info(response);



            using (var client2 = new HttpClient())
            {

                client2. DefaultRequestHeaders.Add("Authorization", token);

                
                var parameters = new Dictionary<string, string> ();
                parameters.Add("Authorization", token);
                var content2 = new FormUrlEncodedContent(parameters);
                client2.BaseAddress = new Uri(url2);
                
                
                var response2 = await client2.PostAsync(url2, content2);
                string responseText = await response2.Content.ReadAsStringAsync();
                Console.WriteLine(response2);
                //if (response2.IsSuccessStatusCode)
                //{
                //    Console.WriteLine("Запрос успешно отправлен");
                //}
                //else
                //{
                //    Console.WriteLine("Произошла ошибка при отправке запроса");
                //}
            }










            //var reqParams = new RequestParams();
            //StringContent content = new StringContent("");
            //content.Headers.Add("Authorization", token);
            //var response2 = await client.PostAsync(url2, content);

            //Console.WriteLine(response2);
            //logger.Info(response2);

            // Parse JSON response.

        }
        Thread.Sleep(100000);
    }
    catch (Exception)
    {
        logger.Error("PIZDA");
        throw;
    }
}


