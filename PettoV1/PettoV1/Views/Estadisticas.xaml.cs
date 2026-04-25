using PettoV1.ViewModels;

namespace PettoV1.Views
{
    public partial class Estadisticas : ContentPage
    {
        private readonly EstadisticasViewModel _vm;

        public Estadisticas(EstadisticasViewModel vm)
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