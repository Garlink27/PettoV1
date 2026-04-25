using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using PettoV1.Views;
using SharedResources.Data;
using SharedResources.Models;
using System.Collections.ObjectModel;

namespace PettoV1.ViewModels
{
    [QueryProperty(nameof(Categoria), "Categoria")]
    public partial class CategoriaViewModel : ObservableObject
    {
        private readonly DataContext _dataContext;

        [ObservableProperty]
        private CategoriaModel _categoria = new();

        [ObservableProperty]
        private ObservableCollection<TareaModel> _tareasIncompletas = new();

        [ObservableProperty]
        private ObservableCollection<TareaModel> _tareasCompletadas = new();

        [ObservableProperty]
        private string _fechaHora = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");

        public CategoriaViewModel(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        partial void OnCategoriaChanged(CategoriaModel value)
        {
            _ = CargarTareasAsync();
        }

        private async Task CargarTareasAsync()
        {
            TareasIncompletas.Clear();
            TareasCompletadas.Clear();

            var tareas = await _dataContext.Tareas
                .AsNoTracking()
                .Where(t => t.CategoriaId == Categoria.Id)
                .ToListAsync();

            foreach (var t in tareas.Where(t => !t.Completada))
                TareasIncompletas.Add(t);
            foreach (var t in tareas.Where(t => t.Completada))
                TareasCompletadas.Add(t);
        }

        [RelayCommand]
        public async Task AgregarTarea()
        {
            // Navegar a DetalleTarea en modo creación
            await Shell.Current.GoToAsync(
                nameof(DetalleTarea),
                new Dictionary<string, object>
                {
                    ["Tarea"] = new TareaModel { CategoriaId = Categoria.Id, Categoria = Categoria }
                });
        }

        [RelayCommand]
        public async Task VerDetalleTarea(TareaModel tarea)
        {
            await Shell.Current.GoToAsync(
                nameof(DetalleTarea),
                new Dictionary<string, object> { ["Tarea"] = tarea });
        }

        [RelayCommand]
        public async Task EliminarTarea(TareaModel tarea)
        {
            string respuesta = await Shell.Current.DisplayActionSheet(
                "¿Eliminar tarea?", "Cancelar", "Eliminar");
            if (respuesta != "Eliminar") return;

            var entidad = await _dataContext.Tareas.FindAsync(tarea.Id);
            if (entidad is not null)
            {
                _dataContext.Tareas.Remove(entidad);
                await _dataContext.SaveChangesAsync();
                await CargarTareasAsync();
            }
        }

        [RelayCommand]
        public async Task ToggleCompletada(TareaModel tarea)
        {
            var entidad = await _dataContext.Tareas.FindAsync(tarea.Id);
            if (entidad is not null)
            {
                entidad.Completada = !entidad.Completada;
                _dataContext.Tareas.Update(entidad);
                await _dataContext.SaveChangesAsync();
                await CargarTareasAsync();
            }
        }

        [RelayCommand]
        public async Task Regresar()
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}
