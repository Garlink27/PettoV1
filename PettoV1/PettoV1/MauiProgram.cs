using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PettoV1.ViewModels;
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

            // ViewModels
            builder.Services.AddTransient<LoginViewModel>();
            builder.Services.AddTransient<RegistroViewModel>();
            builder.Services.AddTransient<MainViewModel>();
            builder.Services.AddTransient<TareasViewModel>();
            builder.Services.AddTransient<CategoriaViewModel>();
            builder.Services.AddTransient<DetalleTareaViewModel>();
            builder.Services.AddTransient<ConfiguracionViewModel>();
            builder.Services.AddTransient<PerfilViewModel>();
            builder.Services.AddTransient<EstadisticasViewModel>();
            builder.Services.AddTransient<HistorialTareasViewModel>();
            builder.Services.AddTransient<ChatViewModel>();

            // Views
            builder.Services.AddTransient<Login>();
            builder.Services.AddTransient<Registro>();
            builder.Services.AddTransient<MainPage>();
            builder.Services.AddTransient<Tareas>();
            builder.Services.AddTransient<Categoria>();
            builder.Services.AddTransient<DetalleTarea>();
            builder.Services.AddTransient<Configuracion>();
            builder.Services.AddTransient<Perfil>();
            builder.Services.AddTransient<Estadisticas>();
            builder.Services.AddTransient<HistorialTareas>();
            builder.Services.AddTransient<Chat>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {

                var dataBase = scope.ServiceProvider.GetRequiredService<DataContext>();
                dataBase.Database.Migrate();
            }

            return app;
        }
    }
}
