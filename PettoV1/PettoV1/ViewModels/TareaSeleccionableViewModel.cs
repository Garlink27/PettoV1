using CommunityToolkit.Mvvm.ComponentModel;
using SharedResources.Models;

namespace PettoV1.ViewModels
{
    /// <summary>
    /// Wrapper around TareaModel that adds UI-only selection state
    /// for the multi-select mode in CategoriaViewModel.
    /// </summary>
    public partial class TareaSeleccionableViewModel : ObservableObject
    {
        public TareaModel Tarea { get; }

        [ObservableProperty]
        private bool _esSeleccionada;

        public string Titulo => Tarea.Titulo;
        public bool Completada => Tarea.Completada;

        public TareaSeleccionableViewModel(TareaModel tarea)
        {
            Tarea = tarea;
        }
    }
}
