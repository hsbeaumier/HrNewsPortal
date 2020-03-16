using System;
using System.Configuration;
using System.Threading.Tasks;
using HrNewsPortal.Data.Repositories;
using HrNewsPortal.Services;
using HrNewsPortal.Services.Builders;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NLog.Extensions.Logging;

namespace HrNewsPortal.DataLoader.WebJob
{
    public static class Program
    {
        /// <summary>
        /// We need access to service provider later in TimedTrigger - to get data from the config file
        /// </summary>
        public static IServiceProvider Services { get; set; }

        [NoAutomaticTrigger]
        private static async Task Main(string[] args)
        {
            var builder = new HostBuilder();

            builder.ConfigureWebJobs((context, jobsBuilder) =>
                {
                    jobsBuilder.AddAzureStorageCoreServices();
                    jobsBuilder.AddAzureStorage();
                    jobsBuilder.AddTimers();
                }).ConfigureAppConfiguration(configurationBuilder =>
                {
                    // Adding command line as additional configuration source
                    configurationBuilder.AddCommandLine(args);

                    configurationBuilder.AddJsonFile("./appsettings.json",
                        optional: false,
                        reloadOnChange: false);
                })
                .ConfigureLogging(loggingBuilder => { loggingBuilder.AddNLog(); })
                .ConfigureServices((context, services) =>
                   {
                       services.AddSingleton(context.Configuration);
                       services.AddMemoryCache();

                       //IHrNewsWebApiService service, IHrNewsRepository repo

                       // other DI configuration here
                       var generalSettings = GeneralSettingsBuilder.Build(context.Configuration);
                       services.AddSingleton(generalSettings);

                       var hrSettings = HrNewsClientSettingsBuilder.Build(context.Configuration);
                       services.AddSingleton(hrSettings);
                       
                       services.AddScoped<IHrNewsRepository, HrNewsRepository>();
                       services.AddScoped<IHrNewsWebApiService, HrNewsWebApiService>();

                       services.AddSingleton(services);

                       // add the hosted service.  HostBuilder will start this automatically
                       services.AddSingleton<IHostedService, Host>();
                   })
                   .UseConsoleLifetime();

            var host = builder.Build();
            Services = host.Services;

            using (host)
            {
                await host.RunAsync();
            }
        }
    }
}
