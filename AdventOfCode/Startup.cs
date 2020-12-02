using AdventOfCode.App;
using AdventOfCode.Configuration;
using AdventOfCode.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AdventOfCode
{
    public class Startup
    {
        public IConfigurationRoot Configuration { get; }

        public Startup()
        {
            var appSettings = "appsettings.json";

            var builder = new ConfigurationBuilder()
                .AddJsonFile(appSettings, optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }


        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IConfiguration>(Configuration);
            services.Configure<Settings>(Configuration.GetSection("Settings"));


            services.AddScoped<AdventOfCode2019>();
            services.AddScoped<AdventOfCode2020>();

            services.AddTransient<Application>();
        }

    }
}
