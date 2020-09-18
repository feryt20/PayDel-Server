using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Parbad.Builder;
using PayDel.Payment.Helpers.Configuration;

namespace PayDel.Payment
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
            //services.AddRazorPages();

            services.AddPayDbContext();
            services.AddPayInitialize();
            services.AddAutoMapper(typeof(Startup));
            services.AddPayDI();
            services.AddMadParbad(Configuration);

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UsePayExceptionHandle(env);
            app.UsePayInitialize();

            app.UseMadParbad();

            //var rewriteOptions = new RewriteOptions();
            //rewriteOptions.Rules.Add(new NonWwwRewriteRule());
            //app.UseRewriter(rewriteOptions);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                      name: "pay",
                    pattern: "{controller=bank}/{action=pay}/{token?}");
            });


        }
    }
}
