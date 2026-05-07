using PettoV1.ViewModels;

namespace PettoV1.Views
{
    public partial class Registro : ContentPage
    {
        public Registro()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            BindingContext ??= IPlatformApplication.Current?.Services
                              .GetService<RegistroViewModel>();
        }
    }
}