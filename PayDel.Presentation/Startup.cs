using System;
using System.Net;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PayDel.Common.Helpers;
using PayDel.Presentation.Helpers.Configuration;
using PayDel.Services.Seed.Service;

namespace PayDel.Presentation
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddPayDbContext();
            services.AddPayInitialize();
            services.AddPayIdentityInit();
            services.AddPayAuth(Configuration);

            services.AddAutoMapper(typeof(Startup));

            services.AddPayDI();

            services.AddPayApiVersioning();
            services.AddPaySwagger();
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, SeedService seed)
        {
            app.UsePayExceptionHandle(env);
            app.UsePayInitialize(seed);

            app.UsePayAuth();
            app.UsePaySwagger();


            app.UseStaticFiles(new StaticFileOptions
            {
                RequestPath = new PathString("/wwwroot")
            });

            //app.UseMvc();

            //app.UseEndpoints(end =>
            //{
            //    end.MapControllers();
            //    end.MapControllerRoute(
            //         name: "default",
            //       pattern: "{controller=home}/{action=index}/{id}");
            //});

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
