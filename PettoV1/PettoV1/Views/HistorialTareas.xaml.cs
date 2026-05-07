using PettoV1.ViewModels;

namespace PettoV1.Views
{
    public partial class HistorialTareas : ContentPage
    {
        private readonly HistorialTareasViewModel _vm;

        public HistorialTareas(HistorialTareasViewModel vm)
        {
            InitializeComponent();
            _vm = vm;
            BindingContext = vm;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await _vm.InicializarAsync();
        }
    }
}