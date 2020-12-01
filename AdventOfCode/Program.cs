using AdventOfCode.App;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace AdventOfCode
{
    class Program
    {
        public static void Main(string[] args)
        {
            IServiceCollection services = new ServiceCollection();
            Startup startup = new Startup();
            startup.ConfigureServices(services);
            IServiceProvider serviceProvider = services.BuildServiceProvider();

            serviceProvider.GetService<Application>().InitComponents();
        }
    }
}
