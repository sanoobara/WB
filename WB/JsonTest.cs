using Newtonsoft.Json;
namespace WB
{
    public class JsonTest
    {


        public JsonTest(string pathJson)
        {
            using (StreamReader reader = new StreamReader(pathJson))
            {
                var json = reader.ReadToEnd();
                // Newtonsoft.Json.JsonSerializer serializer = new Newtonsoft.Json.JsonSerializer();

                Root r = JsonConvert.DeserializeObject<Root>(json);
                if (r.orders.Count != 0)
                {


                    Console.WriteLine(r.orders[0].skus[0]);
                }

            }

        }



    }
    //internal class orders
    //{
    //    public Skus skus;
    //}
    //internal class Skus
    //{
    //    public string[] value;
    //}

    public class Order
    {
        public object address { get; set; }
        public string deliveryType { get; set; }
        public object user { get; set; }
        public string orderUid { get; set; }
        public string article { get; set; }
        public string rid { get; set; }
        public DateTime createdAt { get; set; }
        public List<string> offices { get; set; }
        public List<string> skus { get; set; }
        public int id { get; set; }
        public int warehouseId { get; set; }
        public int nmId { get; set; }
        public int chrtId { get; set; }
        public int price { get; set; }
        public int convertedPrice { get; set; }
        public int currencyCode { get; set; }
        public int convertedCurrencyCode { get; set; }
        public int cargoType { get; set; }
    }

    public class Root
    {
        public List<Order> orders { get; set; }
    }


}
