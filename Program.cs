using Flower_site.DAL;
using FlowerSite.Areas.admin.Data;
using Microsoft.EntityFrameworkCore;

namespace FlowerSite
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddMvc();
            builder.Services.AddControllersWithViews().AddNewtonsoftJson(opt => opt.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
            builder.Services.AddDbContext<AppDbContext>(
                opt => opt.UseSqlServer(builder.Configuration
                .GetConnectionString("DefaultConnection")));
            builder.Services.AddSession(x => x.IdleTimeout = TimeSpan.FromMinutes(10));

            Constans.RootPath = builder.Environment.WebRootPath;
            var app = builder.Build();

            app.UseStaticFiles();
            app.UseSession();
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                name: "areas",
                pattern: "{area:exists}/{controller=dashboard}/{action=Index}/{id?}");

                app.MapControllerRoute(
                "default",
                "{controller=home}/{action=index}/{id?}"
                );
            });

            app.Run();
        }
    }
}