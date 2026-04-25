using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using SharedResources.Data;

namespace PettoV1.ViewModels
{
    public partial class EstadisticasViewModel : ObservableObject
    {
        private readonly DataContext _dataContext;

        [ObservableProperty] private int _totalTareas;
        [ObservableProperty] private int _tareasCompletadas;
        [ObservableProperty] private int _tareasIncompletas;
        [ObservableProperty] private double _porcentajeCompletadas;
        [ObservableProperty] private string _fechaHora = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
        [ObservableProperty] private DateTime _fechaSeleccionada = DateTime.Today;

        public EstadisticasViewModel(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task InicializarAsync()
        {
            await CargarEstadisticasAsync();
        }

        private async Task CargarEstadisticasAsync()
        {
            TotalTareas = await _dataContext.Tareas.CountAsync();
            TareasCompletadas = await _dataContext.Tareas.CountAsync(t => t.Completada);
            TareasIncompletas = TotalTareas - TareasCompletadas;
            PorcentajeCompletadas = TotalTareas > 0
                ? Math.Round((double)TareasCompletadas / TotalTareas * 100, 1)
                : 0;
        }

        [RelayCommand]
        public async Task ActualizarEstadisticas() => await CargarEstadisticasAsync();

        [RelayCommand]
        public void AbrirMenu() => Shell.Current.FlyoutIsPresented = true;

        [RelayCommand]
        public async Task IrAPerfil() => await Shell.Current.GoToAsync("PerfilPage");
    }
}
