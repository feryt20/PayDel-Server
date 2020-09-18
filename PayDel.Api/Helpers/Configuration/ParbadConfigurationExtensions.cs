using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Parbad.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;

namespace PayDel.Api.Helpers.Configuration
{
    public static class ParbadConfigurationExtensions
    {
        public static void AddMadParbad(this IServiceCollection services, IConfiguration configuration)
        {
            //var con = configuration.GetSection("ConnectionStrings");

            //services.AddParbad()
            //    .ConfigureGateways(gateWayes =>
            //    {
            //        gateWayes
            //        .AddMellat()
            //        .WithAccounts(accs =>
            //        {
            //            accs.AddFromConfiguration(configuration.GetSection("MellatBank"));
            //        });
            //        gateWayes
            //        .AddZarinPal()
            //        .WithAccounts(accs =>
            //        {
            //            accs.AddFromConfiguration(configuration.GetSection("ZarinPalBank"));
            //        });
            //        gateWayes
            //        .AddParbadVirtual()
            //        .WithOptions(bld => bld.GatewayPath = "/MadpayGateWay");
            //    })
            //    .ConfigureHttpContext(bld => bld.UseDefaultAspNetCore())
            //    .ConfigureStorage(bld =>
            //    {
            //        bld.UseEntityFrameworkCore(ef =>
            //            ef.UseSqlServer(con.GetSection("Financial").Value,
            //            opt => opt.UseParbadMigrations()))
            //        .ConfigureDatabaseInitializer(bld =>
            //        {
            //            bld.CreateAndMigrateDatabase();
            //        });
            //    });


            services.AddParbad()
                 .ConfigureGateways(gateways =>
                 {
                     gateways
                         .AddMellat()
                         .WithAccounts(accounts =>
                         {
                             accounts.AddInMemory(account =>
                             {
                                 account.TerminalId = 123;
                                 account.UserName = "MyId";
                                 account.UserPassword = "MyPassword";
                             });
                         });

                     gateways
                         .AddParbadVirtual()
                         .WithOptions(options => options.GatewayPath = "/MyVirtualGateway");
                 })
                 .ConfigureHttpContext(builder => builder.UseDefaultAspNetCore())
                 .ConfigureStorage(builder => builder.UseMemoryCache());
        }

        public static void UseMadParbad(this IApplicationBuilder app)
        {
            app.UseParbadVirtualGateway();
        }
    }
}
