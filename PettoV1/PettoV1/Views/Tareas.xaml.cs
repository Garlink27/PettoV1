using PettoV1.ViewModels;

namespace PettoV1.Views
{
    public partial class Tareas : ContentPage
    {
        private readonly TareasViewModel _vm;

        public Tareas(TareasViewModel vm)
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