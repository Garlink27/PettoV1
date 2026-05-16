using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using PettoV1.Charts;
using SharedResources.Data;

namespace PettoV1.ViewModels
{
    public partial class EstadisticasViewModel : ObservableObject
    {
        private readonly DataContext _dataContext;

        // ── Estadísticas ──────────────────────────────────────────────────
        [ObservableProperty] private int _totalTareas;
        [ObservableProperty] private int _tareasCompletadas;
        [ObservableProperty] private int _tareasIncompletas;
        [ObservableProperty] private double _porcentajeCompletadas;

        // ── UI ────────────────────────────────────────────────────────────
        [ObservableProperty] private string _fechaHora = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");

        // ── Rango de fechas (inicio del mes actual → hoy por defecto) ─────
        [ObservableProperty]
        private DateTime _fechaInicio = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);

        [ObservableProperty]
        private DateTime _fechaFin = DateTime.Today;

        /// <summary>Cuando es true, ignora el filtro de fechas y muestra todas las tareas.</summary>
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(TextoBotonTodas))]
        [NotifyPropertyChangedFor(nameof(FechasHabilitadas))]
        private bool _mostrarTodas = false;

        public string TextoBotonTodas => MostrarTodas ? "📅  Filtrar por período" : "📋  Mostrar todas las tareas";
        public bool FechasHabilitadas => !MostrarTodas;

        // ── Gráfica ───────────────────────────────────────────────────────
        [ObservableProperty] private TareasChartDrawable _chartDrawable = new();

        public EstadisticasViewModel(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task InicializarAsync() => await CargarEstadisticasAsync();

        // Se recarga automáticamente cuando cambia cualquier fecha
        // Al cambiar una fecha, desactivar "mostrar todas"
        partial void OnFechaInicioChanged(DateTime value)
        {
            MostrarTodas = false;
            _ = CargarEstadisticasAsync();
        }
        partial void OnFechaFinChanged(DateTime value)
        {
            MostrarTodas = false;
            _ = CargarEstadisticasAsync();
        }

        [RelayCommand]
        public async Task ToggleMostrarTodas()
        {
            MostrarTodas = !MostrarTodas;
            await CargarEstadisticasAsync();
        }

        private async Task CargarEstadisticasAsync()
        {
            int usuarioId = Preferences.Get("UsuarioId", 0);

            IQueryable<SharedResources.Models.TareaModel> query = _dataContext.Tareas
                .Where(t => t.Categoria!.UsuarioId == usuarioId);

            // Aplicar filtro de fechas solo si NO estamos en modo "mostrar todas"
            if (!MostrarTodas)
            {
                var inicio = FechaInicio.Date;
                var fin = FechaFin.Date;
                if (inicio > fin) fin = inicio;
                var finExclusivo = fin.AddDays(1);

                query = query.Where(t => t.FechaLimite >= inicio
                                      && t.FechaLimite < finExclusivo);
            }

            TotalTareas = await query.CountAsync();
            TareasCompletadas = await query.CountAsync(t => t.Completada);
            TareasIncompletas = TotalTareas - TareasCompletadas;

            PorcentajeCompletadas = TotalTareas > 0
                ? Math.Round((double)TareasCompletadas / TotalTareas * 100, 1)
                : 0;

            // Reemplazar el drawable para que el GraphicsView se redibuje
            ChartDrawable = new TareasChartDrawable
            {
                Total = TotalTareas,
                Completadas = TareasCompletadas,
                Pendientes = TareasIncompletas
            };
        }

        [RelayCommand]
        public async Task ActualizarEstadisticas() => await CargarEstadisticasAsync();

        [RelayCommand]
        public void AbrirMenu() => Shell.Current.FlyoutIsPresented = true;

        [RelayCommand]
        public async Task IrAPerfil() => await Shell.Current.GoToAsync("Perfil");
    }
}