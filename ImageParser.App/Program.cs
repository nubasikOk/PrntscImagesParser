using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using AngleSharp.Html.Parser;
using ImageParser.App.Options;
using ImageParser.App.Services;
using ImageParser.App.Services.Contracts;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Extensions.Logging;
using NLog.Web;

namespace ImageParser.App
{
    internal class Program
    {
        public static IConfigurationRoot Configuration;

        private static async Task Main(string[] args)
        {
            Configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetParent(AppContext.BaseDirectory).FullName)
                .AddJsonFile(GetBaseDirectoryPath("appsettings.json"), true)
                .Build();

            var builder = new HostBuilder()
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddMediatR(typeof(Program).Assembly);
                    services.AddTransient<IHtmlParser>(provider => new HtmlParser());
                    services.AddTransient(provider => new WebClient());
                    services.AddTransient<IHttpRequestsFactory, HttpRequestsFactory>();
                    services.AddHostedService<FileParsingService>();

                    var storageAccountOptions = new StorageAccountOptions();
                    Configuration.GetSection(StorageAccountOptions.SectionName).Bind(storageAccountOptions);
                    services.AddSingleton(storageAccountOptions);
                    services.AddScoped<IBlobStorageService, BlobStorageService>();
                })
                .ConfigureLogging(logBuilder =>
                {
                    logBuilder.ClearProviders();
                    logBuilder.AddConsole(options =>
                    {
                        options.DisableColors = true;
                        options.TimestampFormat = "[MM.dd.yyyy HH:mm:ss.fff] ";
                    });
                })
                .ConfigureHostConfiguration(provider =>
                {
                    provider.AddConfiguration(Configuration);
                    // NLog
                    var nLogConfigSection = Configuration.GetSection("NLog");
                    LogManager.Configuration = new NLogLoggingConfiguration(nLogConfigSection);
                    NLogBuilder.ConfigureNLog(LogManager.Configuration);
                })
                .UseConsoleLifetime();

            await builder.Build().RunAsync();
        }

        public static string GetBaseDirectoryPath(string filename) => Path.Combine(AppContext.BaseDirectory,
            string.Format("..{0}..{0}..{0}", Path.DirectorySeparatorChar), filename);
    }
}
