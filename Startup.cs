using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Antiforgery.Internal;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;
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

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddSingleton(Configuration);

            services.AddIdentity<User, IdentityRole>(config =>
            {
                config.User.RequireUniqueEmail = true;
                config.Password.RequiredLength = 5;
                config.Cookies.ApplicationCookie.LoginPath = "/Auth/UserName";
                config.Cookies.ApplicationCookie.Events = new CookieAuthenticationEvents()
                {
                    OnRedirectToLogin = async ctx =>
                    {
                        if (ctx.Request.Path.StartsWithSegments("/api")&& ctx.Response.StatusCode == 200)
                        {
                            ctx.Response.StatusCode = 401;
                        }
                        else
                        {
                            ctx.Response.Redirect(ctx.RedirectUri);
                        }
                        await Task.Yield();
                    }
                };
            }).AddEntityFrameworkStores<PayForContext>();

            services.AddEntityFramework().AddEntityFrameworkSqlServer().AddDbContext<PayForContext>();

            services.AddScoped<IPayForRepository, PayForRepository>();

            services.AddTransient<PayForSeedData>();

            services.AddMvc().AddJsonOptions(config =>
            {
                config.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, 
            IHostingEnvironment env, 
            ILoggerFactory loggerFactory,
            PayForSeedData payForSeed)
        {
            Mapper.Initialize(config =>
            {
                config.CreateMap<User, LoginViewModel>().ReverseMap();
                config.CreateMap<User, UserRowViewModel>();
                config.CreateMap<Group, GroupViewModel>()
                    .ForMember(dst => dst.Users,
                        op => op.MapFrom(src => src.UserGroups.Select(x=>x.User)));
                    // .ForMember(dst => dst.AuthorName, 
                    //     op=>op.MapFrom(src=>src.AuthorUser.UserName+" "+src.AuthorUser.LastName));
                config.CreateMap<Group, GroupRowViewModel>()
                    // .ForMember(dst => dst.AuthorName, 
                    //     op=>op.MapFrom(src=>src.AuthorUser.UserName+" "+src.AuthorUser.LastName))
                    .ReverseMap();
                config.CreateMap<Payment, PaymentViewModel>();
                    // .ForMember(src=>src.AddUser,op=>op.MapFrom(src=>src.User.FirstName+" "+ src.User.LastName));
                config.CreateMap<Payment, PaymentCreateViewModel>().ReverseMap();
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

            app.UseIdentity();

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
