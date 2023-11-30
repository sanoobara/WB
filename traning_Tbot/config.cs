using Microsoft.Extensions.Configuration;

namespace traning_Tbot
{
    internal class config
    {

        public static IConfiguration Configuration { get; set; }
        public config()
        {
            

            IConfigurationBuilder builder = new ConfigurationBuilder().AddJsonFile("appsettings.json", false, true);
            Configuration = builder.Build();

        }

    }
}
