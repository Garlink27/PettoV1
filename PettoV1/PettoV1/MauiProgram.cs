using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PettoV1.Views;
using SharedResources.Data;

namespace PettoV1
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
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            string dbPath = Path.Combine(FileSystem.AppDataDirectory, "petto.db");
            builder.Services.AddDbContext<DataContext>(options => options.UseSqlite($"Filename={dbPath}"));



#if DEBUG
            builder.Logging.AddDebug();
#endif

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var dataBase = scope.ServiceProvider.GetRequiredService<DataContext>();
                dataBase.Database.Migrate();
            }

            return builder.Build();
        }
    }
}
