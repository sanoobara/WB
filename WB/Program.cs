// See https://aka.ms/new-console-template for more information

using Microsoft.Extensions.Logging;
using NLog;
using NLog.Config;

var token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJhY2Nlc3NJRCI6Ijg3MWE4OTVhLTM3YmEtNDhjZC1iZDRiLTMyODUyMWZkZTUxYiJ9.50kj13J30fZZ2eQrpOVnckL9OLuAIdcAWJW30xt5U0A";
var url = "https://suppliers-api.wildberries.ru/api/v3/orders/new";

Logger logger= LogManager.GetCurrentClassLogger();

int timeout = 60000;


while (true)
{
    try
    {
        using (var client = new HttpClient())
        {
            //var url = "https://www.theidentityhub.com/{tenant}/api/identity/v1";
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            var response = await client.GetStringAsync(url);
            Console.WriteLine(response);
            logger.Info(response);

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






