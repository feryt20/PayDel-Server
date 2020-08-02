using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using PayDel.Common.Helpers;
using PayDel.Data.DatabaseContext;
using PayDel.Presentation.Helpers;
using PayDel.Repo.Infrastructures;
using PayDel.Services.Seed.Interface;
using PayDel.Services.Seed.Service;
using PayDel.Services.Site.Admin.Auth.Interface;
using PayDel.Services.Site.Admin.Auth.Service;

namespace PayDel.Presentation
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
            services.AddMvc(opt =>
            {
                opt.EnableEndpointRouting = false;
                opt.ReturnHttpNotAcceptable = true;

                //var jsonFormatter = opt.OutputFormatters.OfType<SystemTextJsonOutputFormatter>().Single();
                //opt.OutputFormatters.Remove(jsonFormatter);
                //opt.OutputFormatters.Add(new IonOutputFormatter(jsonFormatter));

                //opt.OutputFormatters.Add(new XmlDataContractSerializerOutputFormatter());
                //opt.InputFormatters.Add(new XmlSerializerInputFormatter(opt));
            });

            //services.AddHsts(opt =>
            //{
            //    opt.MaxAge = TimeSpan.FromDays(180);
            //    opt.IncludeSubDomains = true;
            //    opt.Preload = true;
            //});

            services.AddRouting(opt => opt.LowercaseUrls = true);
            services.AddAutoMapper(typeof(Startup));
            services.AddHttpContextAccessor();
            services.AddTransient<ISeedService, SeedService>();
            services.AddCors();

            services.Configure<SomeSetting>(Configuration.GetSection("SomeSetting"));


            //////services.AddTransient();//false to use --> create instance from db for each request 
            //////services.AddSingleton();//single instance of database
            services.AddScoped<IUnitOfWork<PayDelDbContext>, UnitOfWork<PayDelDbContext>>(); //normal between Singleton and Transiant
            services.AddScoped<IAuthService, AuthService>(); //normal between Singleton and Transiant

            services.AddScoped<UserCheckIdFilter>();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(opt =>
                {
                    opt.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration.GetSection("AppSettings:Token").Value)),
                        ValidateIssuer = false,
                        ValidateAudience = false,

                    };
                });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API - Site", Version = "v1" });
                c.SwaggerDoc("v2", new OpenApiInfo { Title = "My API - Api", Version = "v2" });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please insert JWT with Bearer into field",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                   {
                     new OpenApiSecurityScheme
                     {
                       Reference = new OpenApiReference
                       {
                         Type = ReferenceType.SecurityScheme,
                         Id = "Bearer"
                       }
                      },
                      new string[] { }
                    }
                  });
            });

        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ISeedService seed)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler(builder =>
                {
                    builder.Run(async context =>
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                        var error = context.Features.Get<IExceptionHandlerFeature>();
                        if (error != null)
                        {
                            context.Response.AddAppError(error.Error.Message);
                            await context.Response.WriteAsync(error.Error.Message);
                        }
                    });
                });
                //app.UseHsts();
            }
            

            seed.SeedUsers();

            app.UseCors(p => p.WithOrigins("http://localhost:4200").AllowAnyMethod().AllowAnyHeader().AllowCredentials());

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My Site");
                c.SwaggerEndpoint("/swagger/v2/swagger.json", "My API");
            });


            app.UseStaticFiles(new StaticFileOptions
            {
                RequestPath = new PathString("/wwwroot")
            });
            
            app.UseMvc();
        }
    }
}
