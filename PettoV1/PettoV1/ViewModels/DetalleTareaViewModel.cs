using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using SharedResources.Data;
using SharedResources.Models;


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
        [NotifyPropertyChangedFor(nameof(IsFormValid))]
        private DateTime _fechaLimite = DateTime.Today.AddDays(1);

        [ObservableProperty]
        private bool _completada;

        [ObservableProperty]
        private int _categoriaId;

        private TareaModel? _tareaOriginal;

        public bool IsTituloValid => !string.IsNullOrWhiteSpace(Titulo);
        public bool IsFormValid => IsTituloValid;
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
        }

        [RelayCommand]
        public async Task GuardarTarea()
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
