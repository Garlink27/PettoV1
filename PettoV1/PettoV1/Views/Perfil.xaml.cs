using PettoV1.ViewModels;

namespace PettoV1.Views
{
    public partial class Perfil : ContentPage
    {
        private readonly PerfilViewModel _vm;

        public Perfil(PerfilViewModel vm)
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
