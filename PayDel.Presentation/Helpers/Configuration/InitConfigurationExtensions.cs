using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.DependencyInjection;
using PayDel.Data.DatabaseContext;
using PayDel.Services.Seed.Service;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace PayDel.Presentation.Helpers.Configuration
{
    public static class InitConfigurationExtensions
    {
        public static void AddPayDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            var con = configuration.GetSection("ConnectionStrings");

            services.AddDbContext<PayDelDbContext>(opt => {
                opt.UseSqlServer(con.GetSection("Main").Value);
            });
            services.AddDbContext<FinDbContext>(opt => {
                opt.UseSqlServer(con.GetSection("Financial").Value);
            });
            services.AddDbContext<LogDbContext>(opt => {
                opt.UseSqlServer(con.GetSection("Log").Value);
            });
        }
        public static void AddPayInitialize(this IServiceCollection services)
        {
            services.AddMvc(opt =>
            {
                //opt.EnableEndpointRouting = false;
                //opt.ReturnHttpNotAcceptable = true;
                //opt.SuppressAsyncSuffixInActionNames = false;
                //opt.SslPort = 4052;

                var policy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser().Build();
                opt.Filters.Add(new AuthorizeFilter(policy));

                //var jsonFormatter = opt.OutputFormatters.OfType<SystemTextJsonOutputFormatter>().Single();
                //opt.OutputFormatters.Remove(jsonFormatter);
                //opt.OutputFormatters.Add(new IonOutputFormatter(jsonFormatter));

                //opt.OutputFormatters.Add(new XmlDataContractSerializerOutputFormatter());
                //opt.InputFormatters.Add(new XmlSerializerInputFormatter(opt));
            });


            //services.AddMvcCore(config =>
            //{
            //    config.ReturnHttpNotAcceptable = true;
            //    config.Filters.Add(typeof(RequireHttpsAttribute));
            //    var policy = new AuthorizationPolicyBuilder()
            //        .RequireAuthenticatedUser()
            //        .Build();
            //    config.Filters.Add(new AuthorizeFilter(policy));
            //})
            //.AddApiExplorer()
            //.AddFormatterMappings()
            //.AddDataAnnotations()
            //.AddCors(opt =>
            //{
            //    opt.AddPolicy("CorsPolicy", builder =>
            //     builder.WithOrigins("http://localhost:4200", "https://localhost:44318")
            //             .AllowAnyMethod()
            //             .AllowAnyHeader()
            //             .AllowCredentials());
            //});


            //services.AddHsts(opt =>
            //{
            //    opt.MaxAge = TimeSpan.FromDays(180);
            //    opt.IncludeSubDomains = true;
            //    opt.Preload = true;
            //});



            services.AddCors(opt => opt.AddPolicy("CorsPolicy", builder =>
                builder.WithOrigins("http://localhost:4200").AllowAnyMethod().AllowAnyHeader().AllowCredentials()));


            services.AddResponseCaching();
            services.AddResponseCompression(opt => opt.Providers.Add<GzipCompressionProvider>());

            services.AddRouting(opt => opt.LowercaseUrls = true);

        }


        public static void UsePayInitialize(this IApplicationBuilder app, SeedService seeder)
        {
            app.UseResponseCompression();
            seeder.SeedUsers();
            app.UseRouting();

           // app.UseCsp(opt => opt
           //.StyleSources(s => s.Self()
           //.UnsafeInline().CustomSources("pay.madpay724.ir", "api.madpay724.ir", "fonts.googleapis.com"))
           //.ScriptSources(s => s.Self()
           //.UnsafeInline().UnsafeEval().CustomSources("pay.madpay724.ir", "api.madpay724.ir", "apis.google.com", "connect.facebook.net"))
           //.ImageSources(s => s.Self()
           //.CustomSources("pay.madpay724.ir", "api.madpay724.ir", "res.cloudinary.com", "cloudinary.com", "data:"))
           //.MediaSources(s => s.Self()
           //.CustomSources("pay.madpay724.ir", "api.madpay724.ir", "res.cloudinary.com", "cloudinary.com", "data:"))
           //.FontSources(s => s.Self()
           //.CustomSources("fonts.gstatic.com", "data:"))
           //.FrameSources(s => s.Self()
           //.CustomSources("accounts.google.com"))
           //);


           // app.UseXfo(o => o.Deny());
        }

        public static void UsePayInitializeInProduction(this IApplicationBuilder app)
        {
            app.UseHsts();
            app.UseHttpsRedirection();
            app.UseResponseCaching();
        }
    }
}
