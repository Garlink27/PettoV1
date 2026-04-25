using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using SharedResources.Data;
using SharedResources.Models;
using System.Collections.ObjectModel;

namespace PettoV1.ViewModels
{
    public partial class HistorialTareasViewModel : ObservableObject
    {
        private readonly DataContext _dataContext;

        [ObservableProperty]
        private ObservableCollection<TareaModel> _historial = new();

        [ObservableProperty]
        private DateTime _fechaFiltro = DateTime.Today;

        [ObservableProperty]
        private bool _mostrarCalendario;

        [ObservableProperty]
        private string _fechaHora = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");

        public HistorialTareasViewModel(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task InicializarAsync() => await CargarHistorialAsync();

        private async Task CargarHistorialAsync()
        {
            Historial.Clear();
            var tareas = await _dataContext.Tareas
                .AsNoTracking()
                .Include(t => t.Categoria)
                .Where(t => t.Completada)
                .OrderByDescending(t => t.FechaLimite)
                .ToListAsync();
            foreach (var t in tareas) Historial.Add(t);
        }

        [RelayCommand]
        public async Task FiltrarPorFecha()
        {
            Historial.Clear();
            var tareas = await _dataContext.Tareas
                .AsNoTracking()
                .Include(t => t.Categoria)
                .Where(t => t.Completada &&
                            t.FechaLimite.Date == FechaFiltro.Date)
                .OrderByDescending(t => t.FechaLimite)
                .ToListAsync();
            foreach (var t in tareas) Historial.Add(t);
        }

        [RelayCommand]
        public void ToggleCalendario() => MostrarCalendario = !MostrarCalendario;

        [RelayCommand]
        public void AbrirMenu() => Shell.Current.FlyoutIsPresented = true;

        [RelayCommand]
        public async Task IrAPerfil() => await Shell.Current.GoToAsync("PerfilPage");
    }
}
