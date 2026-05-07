using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using SharedResources.Data;
using SharedResources.Models;
using System.Collections.ObjectModel;

namespace PettoV1.ViewModels
{
    public enum AccionSeleccion { Ninguna, Eliminar, Incompleto, Completo, SeleccionarTodas }

    [QueryProperty(nameof(Categoria), "Categoria")]
    public partial class CategoriaViewModel : ObservableObject
    {
        private readonly DataContext _dataContext;

        [ObservableProperty] private CategoriaModel _categoria = new();
        [ObservableProperty] private ObservableCollection<TareaSeleccionableViewModel> _tareasIncompletas = new();
        [ObservableProperty] private ObservableCollection<TareaSeleccionableViewModel> _tareasCompletadas = new();
        [ObservableProperty] private string _fechaHora = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
        [ObservableProperty] private bool _modoSeleccion;
        [ObservableProperty] private AccionSeleccion _accionActual = AccionSeleccion.Ninguna;

        // Computed props consumed by XAML bindings
        public bool NoModoSeleccion => !ModoSeleccion;
        public bool MostrarBotonAceptar => ModoSeleccion && AccionActual != AccionSeleccion.SeleccionarTodas;
        public bool MostrarBotonesMultiples => ModoSeleccion && AccionActual == AccionSeleccion.SeleccionarTodas;

        public CategoriaViewModel(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        // Notify dependents when mode changes
        partial void OnModoSeleccionChanged(bool value)
        {
            OnPropertyChanged(nameof(NoModoSeleccion));
            OnPropertyChanged(nameof(MostrarBotonAceptar));
            OnPropertyChanged(nameof(MostrarBotonesMultiples));
        }

        partial void OnAccionActualChanged(AccionSeleccion value)
        {
            OnPropertyChanged(nameof(MostrarBotonAceptar));
            OnPropertyChanged(nameof(MostrarBotonesMultiples));
        }

        partial void OnCategoriaChanged(CategoriaModel value)
        {
            _ = CargarTareasAsync();
        }

        // â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€
        // Data loading
        // â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€

        public async Task CargarTareasAsync()
        {
            TareasIncompletas.Clear();
            TareasCompletadas.Clear();

            if (Categoria?.Id <= 0) return;

            var tareas = await _dataContext.Tareas
                .AsNoTracking()
                .Where(t => t.CategoriaId == Categoria.Id)
                .ToListAsync();

            foreach (var t in tareas.Where(t => !t.Completada))
                TareasIncompletas.Add(new TareaSeleccionableViewModel(t));
            foreach (var t in tareas.Where(t => t.Completada))
                TareasCompletadas.Add(new TareaSeleccionableViewModel(t));
        }

        // â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€
        // 3-dot menu â†’ enter selection mode
        // â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€

        [RelayCommand]
        public async Task AbrirMenuOpciones()
        {
            string? opcion = await Shell.Current.DisplayActionSheet(
                "Opciones", "Cancelar", null,
                "Eliminar", "Incompleto", "Completado", "Seleccionar todas");

            switch (opcion)
            {
                case "Eliminar":
                    EntrarModoSeleccion(AccionSeleccion.Eliminar);
                    break;
                case "Incompleto":
                    EntrarModoSeleccion(AccionSeleccion.Incompleto);
                    break;
                case "Completado":
                    EntrarModoSeleccion(AccionSeleccion.Completo);
                    break;
                case "Seleccionar todas":
                    EntrarModoSeleccion(AccionSeleccion.SeleccionarTodas);
                    // Pre-select everything
                    foreach (var t in TareasIncompletas) t.EsSeleccionada = true;
                    foreach (var t in TareasCompletadas) t.EsSeleccionada = true;
                    break;
            }
        }

        private void EntrarModoSeleccion(AccionSeleccion accion)
        {
            // Clear any previous selection
            foreach (var t in TareasIncompletas) t.EsSeleccionada = false;
            foreach (var t in TareasCompletadas) t.EsSeleccionada = false;
            AccionActual = accion;
            ModoSeleccion = true;
        }

        // â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€
        // Selection actions
        // â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€

        [RelayCommand]
        public void ToggleSeleccionTarea(TareaSeleccionableViewModel tarea)
        {
            tarea.EsSeleccionada = !tarea.EsSeleccionada;
        }

        [RelayCommand]
        public void CancelarSeleccion()
        {
            ModoSeleccion = false;
            AccionActual = AccionSeleccion.Ninguna;
        }

        /// <summary>Called by the single "Aceptar" button in Eliminar/Incompleto/Completo modes.</summary>
        [RelayCommand]
        public async Task AceptarSeleccion() => await EjecutarAccionAsync(AccionActual);

        /// <summary>Called by the "Eliminar" button in Seleccionar-todas mode.</summary>
        [RelayCommand]
        public async Task EliminarSeleccion() => await EjecutarAccionAsync(AccionSeleccion.Eliminar);

        /// <summary>Called by the "En curso" button in Seleccionar-todas mode.</summary>
        [RelayCommand]
        public async Task IncompletoSeleccion() => await EjecutarAccionAsync(AccionSeleccion.Incompleto);

        /// <summary>Called by the "Completar" button in Seleccionar-todas mode.</summary>
        [RelayCommand]
        public async Task CompletoSeleccion() => await EjecutarAccionAsync(AccionSeleccion.Completo);

        private async Task EjecutarAccionAsync(AccionSeleccion accion)
        {
            var seleccionadas = TareasIncompletas
                .Concat(TareasCompletadas)
                .Where(t => t.EsSeleccionada)
                .ToList();

            if (seleccionadas.Count == 0)
            {
                await Shell.Current.DisplayAlert("Aviso", "No hay tareas seleccionadas.", "OK");
                return;
            }

            foreach (var sel in seleccionadas)
            {
                var entidad = await _dataContext.Tareas.FindAsync(sel.Tarea.Id);
                if (entidad is null) continue;

                switch (accion)
                {
                    case AccionSeleccion.Eliminar:
                        _dataContext.Tareas.Remove(entidad);
                        break;
                    case AccionSeleccion.Incompleto:
                        entidad.Completada = false;
                        _dataContext.Tareas.Update(entidad);
                        break;
                    case AccionSeleccion.Completo:
                        entidad.Completada = true;
                        _dataContext.Tareas.Update(entidad);
                        break;
                }
            }

            await _dataContext.SaveChangesAsync();
            ModoSeleccion = false;
            AccionActual = AccionSeleccion.Ninguna;
            await CargarTareasAsync();
        }

        // â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€
        // Normal (non-selection) commands
        // â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€

        [RelayCommand]
        public async Task AgregarTarea()
        {
            await Shell.Current.GoToAsync(
                "DetalleTarea",
                new Dictionary<string, object>
                {
                    ["Tarea"] = new TareaModel
                    {
                        CategoriaId = Categoria.Id,
                        Categoria = Categoria,
                        FechaLimite = DateTime.Today.AddDays(1)
                    }
                });
        }

        [RelayCommand]
        public async Task VerDetalleTarea(TareaSeleccionableViewModel tarea)
        {
            if (ModoSeleccion)
            {
                tarea.EsSeleccionada = !tarea.EsSeleccionada;
                return;
            }
            await Shell.Current.GoToAsync(
                "DetalleTarea",
                new Dictionary<string, object> { ["Tarea"] = tarea.Tarea });
        }

        [RelayCommand]
        public async Task ToggleCompletada(TareaSeleccionableViewModel tarea)
        {
            if (ModoSeleccion)
            {
                tarea.EsSeleccionada = !tarea.EsSeleccionada;
                return;
            }
            var entidad = await _dataContext.Tareas.FindAsync(tarea.Tarea.Id);
            if (entidad is not null)
            {
                entidad.Completada = !entidad.Completada;
                _dataContext.Tareas.Update(entidad);
                await _dataContext.SaveChangesAsync();
                await CargarTareasAsync();
            }
        }

        [RelayCommand]
        public async Task Regresar() => await Shell.Current.GoToAsync("..");
    }
}
