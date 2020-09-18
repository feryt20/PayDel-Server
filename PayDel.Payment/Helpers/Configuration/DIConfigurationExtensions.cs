using DnsClient;
using Microsoft.Extensions.DependencyInjection;
using PayDel.Repo.Infrastructures;
using PayDel.Services.Site.Admin.Auth.Interface;
using PayDel.Services.Site.Admin.Auth.Service;
using PayDel.Services.Site.Admin.User;
using PayDel.Services.Site.Admin.Util;

namespace PayDel.Payment.Helpers.Configuration
{
    public static class DIConfigurationExtensions
    {
        public static void AddPayDI(this IServiceCollection services)
        {
            services.AddScoped(typeof(IUnitOfWork<>), typeof(UnitOfWork<>));

            services.AddScoped<ISmsService, SmsService>();
            services.AddSingleton<ILookupClient, LookupClient>();
        }
    }
}
