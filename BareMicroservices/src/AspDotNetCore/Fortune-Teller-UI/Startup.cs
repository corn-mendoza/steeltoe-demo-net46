
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Fortune_Teller_UI.Services;
using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace Fortune_Teller_UI
{
    public class Startup
    {
        private const string USER_PROVIDED_SERVICE_URI = "uri";
        private string _serviceEndpoint { get; set; }
        private ILoggerFactory _iLoggerFactory { get; set; }
         public IConfigurationRoot Configuration { get; set; }

        public Startup(IHostingEnvironment env, ILoggerFactory factory)
        { 
            // Set up configuration sources.
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();

            _iLoggerFactory = factory;

            // Simple Configuration
            // _serviceEndpoint = Configuration["FortuneServiceUri"];

            // User Provided Service configuration
            _serviceEndpoint = parseVcapServices( Configuration["VCAP_SERVICES"] );
          }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddLogging();

            IFortuneService fortuneService = new FortuneService(_iLoggerFactory, _serviceEndpoint);
            services.AddSingleton(fortuneService);

            // Add framework services.
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseMvc();
        }

        private string parseVcapServices(string vcapServicesEnvironment)
        {
            if (vcapServicesEnvironment == null || vcapServicesEnvironment == "")
                throw new System.ArgumentException("Unable to parse the VCAP_Services");

            JObject ups = JObject.Parse(vcapServicesEnvironment);
            if (ups == null)
                throw new System.ArgumentException("Unable to parse the VCAP_Services");

            return (string)ups["user-provided"][0]["credentials"]["uri"];
        }
    }
}
