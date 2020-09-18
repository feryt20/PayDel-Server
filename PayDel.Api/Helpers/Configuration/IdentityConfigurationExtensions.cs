using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using PayDel.Common.Helpers;
using PayDel.Data.DatabaseContext;
using PayDel.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayDel.Api.Helpers.Configuration
{
    public static class IdentityConfigurationExtensions
    {
        public static void AddPayIdentityInit(this IServiceCollection services)
        {
            IdentityBuilder builder = services.AddIdentityCore<User>(opt =>
            {
                opt.Password.RequireDigit = false;
                opt.Password.RequiredLength = 4;
                opt.Password.RequireNonAlphanumeric = false;
                opt.Password.RequireUppercase = false;
                opt.Password.RequireLowercase = false;
            });
            builder = new IdentityBuilder(builder.UserType, typeof(Role), builder.Services);
            builder.AddEntityFrameworkStores<PayDelDbContext>();
            builder.AddRoleValidator<RoleValidator<Role>>();
            builder.AddRoleManager<RoleManager<Role>>();
            builder.AddSignInManager<SignInManager<User>>();
            builder.AddDefaultTokenProviders();
        }

        public static void AddPayAuth(this IServiceCollection services, IConfiguration configuration)
        {
            //services.Configure<TokenSetting>(Configuration.GetSection("TokenSetting"));

            var tokenSettingSection = configuration.GetSection("TokenSetting");
            var tokenSetting = tokenSettingSection.Get<TokenSetting>();
            var key = Encoding.ASCII.GetBytes(tokenSetting.Secret);
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
               .AddJwtBearer(opt =>
               {
                   opt.TokenValidationParameters = new TokenValidationParameters
                   {
                       ValidateIssuerSigningKey = true,
                       //IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration.GetSection("AppSettings:Token").Value)),
                       //ValidateIssuer = false,
                       //ValidateAudience = false,
                       IssuerSigningKey = new SymmetricSecurityKey(key),
                       ValidateIssuer = true,
                       ValidIssuer = tokenSetting.Site,
                       ValidateAudience = true,
                       ValidAudience = tokenSetting.Audience,
                       ClockSkew = TimeSpan.Zero
                   };
               });

            services.AddAuthorization(opt =>
            {
                opt.AddPolicy("RequiredAdminRole", policy => policy.RequireRole("Admin"));

                opt.AddPolicy("AcceccVip", policy => policy.RequireRole("Admin", "Vip"));
                opt.AddPolicy("AcceccOperator", policy => policy.RequireRole("Admin", "Operator"));

                opt.AddPolicy("RequiredVipRole", policy => policy.RequireRole("Vip"));
                opt.AddPolicy("RequiredOperatorRole", policy => policy.RequireRole("Operator"));

                opt.AddPolicy("RequiredUserRole", policy => policy.RequireRole("User"));
            });

            //services.AddAuthentication("Bearer")
            //    .AddIdentityServerAuthentication(opt =>
            //    {
            //        opt.Authority = "http://localhost:5000";
            //        opt.RequireHttpsMetadata = false;
            //        opt.ApiName = "invoices";
            //    });
        }

        public static void UsePayAuth(this IApplicationBuilder app)
        {
            app.UseCors("CorsPolicy");
            //
            app.UseAuthentication();
            app.UseAuthorization();
        }
    }
}
