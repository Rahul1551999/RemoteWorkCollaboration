using Microsoft.EntityFrameworkCore;
using RemoteWorkCollaboration.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;

namespace RemoteWorkCollaboration
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Test SQL Server connection on startup
            using (var connection = new SqlConnection("Server=RAHUL\\REMOTEWORKCOLLAB;Database=RemoteWorkCollaborationDB;Trusted_Connection=True;MultipleActiveResultSets=true"))
            {
                try
                {
                    connection.Open();
                    Console.WriteLine("Database connection successful!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Database connection failed: {ex.Message}");
                }
            }

            var builder = WebApplication.CreateBuilder(args);

            // Configure logging to include EF Core details
            builder.Logging.ClearProviders();
            builder.Logging.AddConsole(); // Add console logging
            builder.Logging.AddDebug(); // Add debug logging

            // Add services to the container
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
                       .EnableSensitiveDataLogging() // Optional: enables logging of sensitive data (e.g., parameters)
                       .LogTo(Console.WriteLine, LogLevel.Information)); // Logs SQL commands to the console

            // Setup Identity services
            builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
                .AddEntityFrameworkStores<ApplicationDbContext>();

            builder.Services.AddControllersWithViews();

            // Configure CORS policy
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    policyBuilder =>
                    {
                        policyBuilder.AllowAnyOrigin()
                                     .AllowAnyMethod()
                                     .AllowAnyHeader();
                    });
            });

            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            // Ensure CORS is set before Authentication & Authorization
            app.UseCors("AllowAll");

            app.UseAuthentication();
            app.UseAuthorization();

            // Map routes for controllers
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Account}/{action=Login}/{id?}");
            app.MapControllers();

            // Run the application
            app.Run();
            builder.Logging.AddConsole(); 

        }
    }
}
