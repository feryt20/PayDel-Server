using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.DependencyInjection;
using PayDel.Data.DatabaseContext;

namespace PayDel.Payment.Helpers.Configuration
{
    public static class InitConfigurationExtensions
    {
        public static void AddPayDbContext(this IServiceCollection services)
        {
            services.AddDbContext<PayDelDbContext>();
            services.AddDbContext<FinDbContext>();
            services.AddDbContext<LogDbContext>();
        }
        public static void AddPayInitialize(this IServiceCollection services)
        {
            services.AddControllersWithViews();

            services.AddMvcCore(config =>
            {
                config.ReturnHttpNotAcceptable = true;
                config.Filters.Add(typeof(RequireHttpsAttribute));
            });

            //services.AddHsts(opt =>
            //{
            //    opt.MaxAge = TimeSpan.FromDays(180);
            //    opt.IncludeSubDomains = true;
            //    opt.Preload = true;
            //});

            services.AddHttpsRedirection(opt =>
            {
                opt.RedirectStatusCode = StatusCodes.Status302Found;
            });

            services.AddResponseCaching();

        }


        public static void UsePayInitialize(this IApplicationBuilder app)
        {
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseStaticFiles();

        }

        public static void UsePayInitializeInProd(this IApplicationBuilder app)
        {
            app.UseHsts();
            app.UseResponseCaching();
        }

    }
}
