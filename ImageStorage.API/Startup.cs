using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ImageStorage.API.Options;
using ImageStorage.API.Services;
using ImageStorage.API.Services.Contracts;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace ImageStorage.API
{
    public class Startup
    {
        private IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddMediatR(typeof(Program).Assembly);
            services.AddScoped<IBlobStorageService, BlobStorageService>();
            var storageAccountOptions = new StorageAccountOptions();
            Configuration.GetSection(StorageAccountOptions.SectionName).Bind(storageAccountOptions);
            services.AddSingleton(storageAccountOptions);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }
    }
}
