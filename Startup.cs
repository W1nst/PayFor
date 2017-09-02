using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Antiforgery.Internal;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Http.Internal;
using Newtonsoft.Json.Serialization;
using System.IdentityModel.Tokens.Jwt;
using PayFor.Context;
using PayFor.Models;
using PayFor.ViewModels;

namespace PayFor
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(Configuration);

            services.AddIdentity<User, IdentityRole>().AddEntityFrameworkStores<PayForContext>().AddDefaultTokenProviders();

            services.AddEntityFrameworkSqlServer().AddDbContext<PayForContext>();

            services.AddScoped<IPayForRepository, PayForRepository>();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(cfg =>
            {
                cfg.RequireHttpsMetadata = false;
                cfg.SaveToken = true;
                
                cfg.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidIssuer = Configuration["Tokens:Issuer"],
                    ValidAudience = Configuration["Tokens:Issuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Tokens:Key"]))
                };
            });

            services.AddTransient<PayForSeedData>();

            services.AddMvc().AddJsonOptions(config =>
            {
                config.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            });
        }

        public void Configure(IApplicationBuilder app, 
            IHostingEnvironment env, 
            ILoggerFactory loggerFactory,
            PayForSeedData payForSeed)
        {
            Mapper.Initialize(config =>
            {
                config.CreateMap<User, LoginViewModel>().ReverseMap();
                config.CreateMap<User, UserRowViewModel>();
                config.CreateMap<User, UserViewModel>().ForMember(dst=> dst.Groups,
                    op=>op.MapFrom(src => src.UserGroups.Select(x=>x.Group)));
                config.CreateMap<Group, GroupViewModel>()
                    .ForMember(dst => dst.Users,
                        op => op.MapFrom(src => src.UserGroups.Select(x=>x.User)));
                    // .ForMember(dst => dst.AuthorName, 
                    //     op=>op.MapFrom(src=>src.AuthorUser.UserName+" "+src.AuthorUser.LastName));
                config.CreateMap<Group, GroupRowViewModel>().ReverseMap();
                config.CreateMap<Payment, PaymentViewModel>();
                config.CreateMap<Payment, PaymentCreateViewModel>().ReverseMap();
                config.CreateMap<Payment,PaymentEditViewModel>().ReverseMap();
                config.CreateMap<Category,CategoryViewModel>().ReverseMap();
            });
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            payForSeed.SeedData().Wait();     
        }
    }
}
