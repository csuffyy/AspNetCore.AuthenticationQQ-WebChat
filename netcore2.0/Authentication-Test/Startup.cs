﻿using Authentication_Test.Data;
using Authentication_Test.Models;
using Authentication_Test.Services;
using Microsoft.AspNetCore.Authentication.MultiOAuth.Stores;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;

namespace Authentication_Test
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
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            // Add application services.
            services.AddTransient<IEmailSender, EmailSender>();

            services.AddAuthentication() 
                .AddWeixinAuthenticationStore<WeixinClientStore>();

            services.AddMvc();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }


    public class WeixinClientStore : IClientStore
    {
        List<StoreModel> _stores = new List<StoreModel>() {
            new StoreModel()
            {
                ClientId = "wxa60caxxxxxxx",
                ClientSecret = "4f141xxxxxxxxx",
                SubjectId = "client1"
            },
            new StoreModel()
            {
                ClientId = "wx4498dxxxxxxxx",
                ClientSecret = "de9478axxxxxxxxx",
                SubjectId = "client2"
            },
        };

        public StoreModel FindBySubjectId(string subjectId)
        {
            return _stores.FirstOrDefault(t => t.SubjectId == subjectId);
        }
    }
}
