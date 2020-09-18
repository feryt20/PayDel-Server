using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PayDel.Api.Helpers.Configuration;

namespace PayDel.Api
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
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UsePayExceptionHandle(env);
            app.UsePayInitialize();

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
