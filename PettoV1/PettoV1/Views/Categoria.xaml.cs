using PettoV1.ViewModels;

namespace PettoV1.Views
{
    public partial class Categoria : ContentPage
    {
        // Declaramos el campo para tener acceso al ViewModel
        private readonly CategoriaViewModel _viewModel;

        // Inyectamos el ViewModel directamente en el constructor
        public Categoria(CategoriaViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = _viewModel = viewModel;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            // 1. Esperamos un instante breve para que Shell termine de 
            // inyectar la 'Categoria' en el ViewModel a través de [QueryProperty]
            await Task.Delay(50);

            // 2. Recargamos las tareas
            if (_viewModel != null)
            {
                await _viewModel.CargarTareasAsync();
            }
        }
    }
}