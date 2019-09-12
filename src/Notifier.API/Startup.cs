using System;
using System.Text;
using Google.Cloud.Firestore;
using Google.Cloud.Firestore.V1;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Notifier.API.Application;

namespace Notifier.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddOptions();

            var settings = Configuration.GetSection(nameof(FirestoreOrderNotifierOptions)).Get<FirestoreOrderNotifierOptions>();
            services.AddSingleton(_ =>
            {
                var builder = new FirestoreClientBuilder
                {
                    JsonCredentials = Encoding.UTF8.GetString(
                        Convert.FromBase64String(settings.FirebaseCredentials))
                };
            
                var client = builder.Build();

                return FirestoreDb.Create(settings.ProjectId, client);
            });
            
            services.Configure<FirestoreOrderNotifierOptions>(Configuration.GetSection(nameof(FirestoreOrderNotifierOptions)));
            services.AddScoped<IOrderChangeNotifier, FirestoreOrderChangeNotifier>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}