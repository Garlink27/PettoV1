using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using SharedResources.Data;
using SharedResources.Models;
using System.Collections.ObjectModel;

namespace PettoV1.ViewModels
{
    [QueryProperty(nameof(Tarea), "Tarea")]
    public partial class DetalleTareaViewModel : ObservableObject
    {
        private readonly DataContext _dataContext;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsTituloValid))]
        [NotifyPropertyChangedFor(nameof(IsFormValid))]
        private string _titulo = string.Empty;

        [ObservableProperty]
        private string _descripcion = string.Empty;

        [ObservableProperty]
        private DateTime _fechaLimite = DateTime.Today.AddDays(1);

        [ObservableProperty]
        private bool _completada;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsFormValid))]
        private int _categoriaId;

        [ObservableProperty]
        private ObservableCollection<CategoriaModel> _categorias = new();

        private TareaModel? _tareaOriginal;

        public bool IsTituloValid => !string.IsNullOrWhiteSpace(Titulo);
        // El formulario es válido si tiene título Y una categoría seleccionada
        public bool IsFormValid => IsTituloValid && CategoriaId > 0;
        public bool EsModoEdicion => _tareaOriginal?.Id > 0;

        public TareaModel Tarea
        {
            set
            {
                _tareaOriginal = value;
                Titulo = value.Titulo;
                Descripcion = value.Descripcion;
                FechaLimite = value.FechaLimite == default ? DateTime.Today.AddDays(1) : value.FechaLimite;
                Completada = value.Completada;
                CategoriaId = value.CategoriaId;
                OnPropertyChanged(nameof(EsModoEdicion));
            }
        }

        public DetalleTareaViewModel(DataContext dataContext)
        {
            _dataContext = dataContext;
            // Cargamos las categorías al instanciar
            _ = CargarCategorias();
        }

        [ObservableProperty]
        private CategoriaModel? _categoriaSeleccionada;

        // Este método se ejecuta automáticamente cuando cambia la selección
        partial void OnCategoriaSeleccionadaChanged(CategoriaModel? value)
        {
            if (value != null)
            {
                CategoriaId = value.Id;
            }
        }
        public async Task CargarCategorias()
        {
            var lista = await _dataContext.Categorias.ToListAsync();
            Categorias = new ObservableCollection<CategoriaModel>(lista);
        }

        [RelayCommand]
        public async Task GuardarTarea()
        {
            if (!IsFormValid) return;

            try
            {
                if (_tareaOriginal?.Id > 0)
                {
                    var entidad = await _dataContext.Tareas.FindAsync(_tareaOriginal.Id);
                    if (entidad is not null)
                    {
                        entidad.Titulo = Titulo;
                        entidad.Descripcion = Descripcion;
                        entidad.FechaLimite = FechaLimite;
                        entidad.Completada = Completada;
                        entidad.CategoriaId = CategoriaId; // Actualizar categoría
                        _dataContext.Tareas.Update(entidad);
                    }
                }
                else
                {
                    var nueva = new TareaModel
                    {
                        Titulo = Titulo,
                        Descripcion = Descripcion,
                        FechaLimite = FechaLimite,
                        Completada = Completada,
                        CategoriaId = CategoriaId
                    };
                    await _dataContext.Tareas.AddAsync(nueva);
                }

                await _dataContext.SaveChangesAsync();
                await Shell.Current.DisplayAlert("Éxito", "Tarea guardada correctamente.", "OK");
                await Shell.Current.GoToAsync("..");
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", $"No se pudo guardar: {ex.Message}", "OK");
            }
        }

        [RelayCommand]
        public async Task EliminarTarea()
        {
            if (_tareaOriginal?.Id <= 0) return;

            string resp = await Shell.Current.DisplayActionSheet(
                "¿Eliminar esta tarea?", "Cancelar", "Eliminar");

            if (resp != "Eliminar") return;

            var entidad = await _dataContext.Tareas.FindAsync(_tareaOriginal!.Id);
            if (entidad is not null)
            {
                _dataContext.Tareas.Remove(entidad);
                await _dataContext.SaveChangesAsync();
            }
            await Shell.Current.GoToAsync("..");
        }

        [RelayCommand]
        public async Task Regresar() => await Shell.Current.GoToAsync("..");
    }
}