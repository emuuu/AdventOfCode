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
            var appSecretSettings = "appsettings.secret.json";

            var builder = new ConfigurationBuilder()
                .AddJsonFile(appSettings, optional: false, reloadOnChange: true)
                .AddJsonFile(appSecretSettings, optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }


        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IConfiguration>(Configuration);
            services.Configure<Settings>(Configuration.GetSection("Settings"));


            services.AddScoped<AdventOfCode2019>();
            services.AddScoped<AdventOfCode2020>();
            services.AddScoped<AdventOfCode2021>();
            services.AddScoped<AdventOfCode2022>();
            services.AddScoped<AdventOfCode2023>();
            services.AddScoped<AdventOfCode2024>();
            services.AddScoped<AdventOfCode2025>();

            services.AddTransient<Application>();
        }

    }
}
