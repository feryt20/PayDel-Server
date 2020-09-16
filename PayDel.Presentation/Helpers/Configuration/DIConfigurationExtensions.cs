using DnsClient;
using Microsoft.Extensions.DependencyInjection;
using PayDel.Common.Helpers;
using PayDel.Repo.Infrastructures;
using PayDel.Services.Seed.Service;
using PayDel.Services.Site.Admin.Auth.Interface;
using PayDel.Services.Site.Admin.Auth.Service;
using PayDel.Services.Site.Admin.Upload.Interface;
using PayDel.Services.Site.Admin.Upload.Service;
using PayDel.Services.Site.Admin.User;
using PayDel.Services.Site.Admin.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PayDel.Presentation.Helpers.Configuration
{
    public static class DIConfigurationExtensions
    {
        public static void AddPayDI(this IServiceCollection services)
        {

           
            services.AddHttpContextAccessor();

            services.AddTransient<SeedService>();

            //////services.AddTransient();//false to use --> create instance from db for each request 
            //////services.AddSingleton();//single instance of database
            //////services.AddScoped(typeof(IUnitOfWork<>), typeof(UnitOfWork<>)); //normal between Singleton and Transiant
            //services.AddScoped<IUnitOfWork<PayDelDbContext>, UnitOfWork<PayDelDbContext>>();
            //services.AddScoped<IUnitOfWork<FinDbContext>, UnitOfWork<FinDbContext>>();

            services.AddScoped(typeof(IUnitOfWork<>), typeof(UnitOfWork<>));

            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IUtilitiess, Utilitiess>();
            services.AddScoped<ISmsService, SmsService>();
            services.AddScoped<IUploadService, UploadService>();
            services.AddSingleton<ILookupClient, LookupClient>();
            services.AddScoped<IWalletService, WalletService>();
            services.AddScoped<UserCheckIdFilter>();
            //services.AddScoped<TokenSetting>();
        }
    }
}
