using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using HestiaLink.Data;
using HestiaLink.Services;

namespace HestiaIT13Final
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            builder.Services.AddMauiBlazorWebView();

            // Add UserSession service for authentication
            builder.Services.AddScoped<UserSession>();
            
            // Add Authentication Service
            builder.Services.AddScoped<AuthenticationService>();

            // Add Attendance Service
            builder.Services.AddScoped<AttendanceService>();

            // Add Inventory Service
            builder.Services.AddScoped<InventoryService>();

            // Add database context with connection string
            var connectionString = "Data Source=.\\SQLEXPRESS;Initial Catalog=IT13;Integrated Security=True;TrustServerCertificate=True;MultipleActiveResultSets=true";
            
            builder.Services.AddDbContext<HestiaLinkContext>(options =>
                options.UseSqlServer(connectionString,
                sqlServerOptions => sqlServerOptions.EnableRetryOnFailure(
                    maxRetryCount: 5,
                    maxRetryDelay: TimeSpan.FromSeconds(10),
                    errorNumbersToAdd: null
                )));

#if DEBUG
    		builder.Services.AddBlazorWebViewDeveloperTools();
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
