using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;

using Petz.Dal;
using Petz.Dal.Entities;
using Petz.Dal.Services;

using Petz.Web.Services;

namespace Petz.Web
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
            services.AddDbContext<ApplicationDbContext>(
                options => options.UseSqlServer(Configuration.GetConnectionString(nameof(ApplicationDbContext)))
            );

            services.AddDefaultIdentity<User>().AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddScoped<FamilyService>();
            services.AddScoped<UserService>();
            services.AddScoped<PetService>();
            services.AddScoped<RecordService>();

            services.AddTransient<IEmailSender, EmailSender>();
            services.Configure<AuthMessageSenderOptions>(authMessageSenderOptions => 
            {
                authMessageSenderOptions.SendGridUser = Configuration["SendGridUser"];
                authMessageSenderOptions.SendGridKey = Configuration["SendGridKey"];
            });

            services.AddRazorPages();

            services.AddAuthentication()
                .AddGoogle(googleOptions => {
                    IConfigurationSection googleAuthSection = Configuration.GetSection("Authentication:Google");
                    googleOptions.ClientId = googleAuthSection["ClientId"];
                    googleOptions.ClientSecret = googleAuthSection["ClientSecret"];
                })
                .AddFacebook(facebookOptions => 
                {
                    IConfigurationSection facebookAuthSection = Configuration.GetSection("Authentication:Facebook");
                    facebookOptions.AppId = facebookAuthSection["AppId"];
                    facebookOptions.AppSecret = facebookAuthSection["AppSecret"];
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }
    }
}
