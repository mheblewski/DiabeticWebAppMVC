using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using AutoMapper;
using DiabeticApp.ApiInfrastructure.Clients.Authentication;
using DiabeticApp.ApiInfrastructure.Clients.Measurements;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DiabeticApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            Uri baseAddressUri = new Uri("http://www.diabeticwebapp.pl"); 
            HttpClient httpClient = new HttpClient()
            {
                BaseAddress = baseAddressUri,
            };

            ServicePointManager.FindServicePoint(baseAddressUri).ConnectionLeaseTimeout = 60000; 

            services.AddSingleton<HttpClient>(httpClient);

            services.AddScoped<IMeasurementsClient, MeasurementsClient>();
            services.AddScoped<IAuthenticationClient, AuthenticationClient>();

            services.AddMvc();
            services.AddDistributedMemoryCache();

            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(15);
                options.Cookie.HttpOnly = true;
            });
            services.AddAutoMapper();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseSession();
            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Account}/{action=Login}/{id?}");
            });
        }
    }
}
