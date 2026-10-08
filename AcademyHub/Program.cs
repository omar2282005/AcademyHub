using AcademyHub.Data;
using AcademyHub.Identity;
using AcademyHub.Mapping;
using AcademyHub.Repositories.Implementation;
using AcademyHub.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace AcademyHub
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddRazorPages(); 
            builder.Services.AddScoped<ICourseRepository, CourseRepository>();
            builder.Services.AddDbContext<TrainingDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("TrainingConnection")));
            builder.Services.AddDbContext<IdentityAppDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection")));

            builder.Services
              .AddDefaultIdentity<ApplicationUser>(options =>
              {
                  options.SignIn.RequireConfirmedAccount = false;
              })
              .AddEntityFrameworkStores<IdentityAppDbContext>();
            builder.Services.AddAutoMapper(
                configuration => { },
                typeof(MappingProfile));


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapStaticAssets();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();

            app.MapRazorPages();

            app.Run();
        }
    }
}
