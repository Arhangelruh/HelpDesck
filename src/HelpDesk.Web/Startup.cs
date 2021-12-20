using Hangfire;
using Hangfire.PostgreSql;
using HelpDesk.BLL.Interfaces;
using HelpDesk.BLL.Repository;
using HelpDesk.BLL.Services;
using HelpDesk.Common.Interfaces;
using HelpDesk.DAL.Context;
using HelpDesk.DAL.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace HelpDesk.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IProfileService, ProfileService>();
            services.AddScoped<IGetUserFromAD, GetUserFromAD>();
            services.AddScoped<IEventService, EventService>();
            services.AddScoped<IStatusService, StatusService>();
            services.AddScoped<IRequestsService, RequestsService>();
            services.AddScoped<ICommentService, CommentService>();
            services.AddScoped<IFileService, FileService>();

            services.AddHangfire(x => x.UsePostgreSqlStorage(Configuration.GetConnectionString("HelpDeskPostgreSQL")));
            services.AddDbContext<HelpDeskContext>(options =>
                
            options.UseNpgsql(Configuration.GetConnectionString("HelpDeskPostgreSQL")));

            services.AddIdentity<User, IdentityRole>(opt =>
            {
                opt.Password.RequiredLength = 3;
                opt.Password.RequireNonAlphanumeric = false;
                opt.Password.RequireLowercase = false;
                opt.Password.RequireUppercase = false;
                opt.Password.RequireDigit = false;
            })
               .AddEntityFrameworkStores<HelpDeskContext>()
               .AddDefaultTokenProviders();

            services.AddControllersWithViews();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseDeveloperExceptionPage();

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseHangfireDashboard("/hangfire", new DashboardOptions
            {
                Authorization = new[] { new MyHangfireDashbordAutorizationFilter() }
            });
            app.UseHangfireServer();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
