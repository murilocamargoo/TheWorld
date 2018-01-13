using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;
using TheWorld.Context;
using TheWorld.Models;
using TheWorld.Services;

namespace TheWorld
{
    public class Startup
    {
        private IHostingEnvironment _environment;
        private IConfigurationRoot _config;

        /// <summary>
        /// teste
        /// </summary>
        /// <param name="environment"></param>

        public Startup(IHostingEnvironment environment)
        {
            _environment = environment;

            var builder = new ConfigurationBuilder()
                .SetBasePath(_environment.ContentRootPath)
                .AddJsonFile("config.json")
                .AddEnvironmentVariables();

            _config = builder.Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(_config);

            if (_environment.IsEnvironment("Development"))
            {
                services.AddScoped<IEmailService, DebugMailService>();
            }
            else
            {
                ///implement real service
            }

            services.AddDbContext<WorldContext>();

            services.AddScoped<IWorldRepository, WorldRepository>();

            services.AddTransient<WorldContextSeedData>();

            services.AddLogging();

            services.AddMvc()
                .AddJsonOptions(config =>
                {
                    config.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, WorldContextSeedData seederData, ILoggerFactory factory)
        {
            //#if DEBUG
            if (env.IsEnvironment("Development"))
            {
                app.UseDeveloperExceptionPage();
                factory.AddDebug(LogLevel.Information);
            }
            //#endif
            else
            {
                factory.AddDebug(LogLevel.Error);
            }

            app.UseStaticFiles();

            app.UseMvc(config =>
            {
                config.MapRoute(
                    name: "Default",
                    template: "{controller}/{action}/{id?}",
                    defaults: new {controller = "App", action = "Index"}
                );
            });

            //Essa porra de seeder da pau quando for usar migrations, quando for atualizar o banco
            //Ou seja, quando precisar atualizar comentar essa merda
            seederData.EnsureSeedData().Wait();
        }
    }
}
