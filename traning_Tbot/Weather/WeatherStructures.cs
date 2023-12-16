namespace traning_Tbot.Weather
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Fact
    {
        public int obs_time { get; set; }
        public int temp { get; set; }
        public int feels_like { get; set; }
        public int temp_water { get; set; }
        public string icon { get; set; }
        public string condition { get; set; }
        public double wind_speed { get; set; }
        public string wind_dir { get; set; }
        public int pressure_mm { get; set; }
        public int pressure_pa { get; set; }
        public int humidity { get; set; }
        public string daytime { get; set; }
        public bool polar { get; set; }
        public string season { get; set; }
        public double wind_gust { get; set; }
    }

    public class Forecast
    {
        public string date { get; set; }
        public int date_ts { get; set; }
        public int week { get; set; }
        public string sunrise { get; set; }
        public string sunset { get; set; }
        public int moon_code { get; set; }
        public string moon_text { get; set; }
        public List<Part> parts { get; set; }
    }

    public class Info
    {
        public string url { get; set; }
        public double lat { get; set; }
        public double lon { get; set; }
    }

    public class Part
    {
        public string part_name { get; set; }
        public int temp_min { get; set; }
        public int temp_avg { get; set; }
        public int temp_max { get; set; }
        public int temp_water { get; set; }
        public double wind_speed { get; set; }
        public double wind_gust { get; set; }
        public string wind_dir { get; set; }
        public int pressure_mm { get; set; }
        public int pressure_pa { get; set; }
        public int humidity { get; set; }
        public double prec_mm { get; set; }
        public int prec_prob { get; set; }
        public int prec_period { get; set; }
        public string icon { get; set; }
        public string condition { get; set; }
        public int feels_like { get; set; }
        public string daytime { get; set; }
        public bool polar { get; set; }
    }

    public class YaweatherStruct

    {
        //public int now { get; set; }
        //public DateTime now_dt { get; set; }
        //public Info info { get; set; }
        //public Fact fact { get; set; }
        public Forecast forecast { get; set; }
    }


}
