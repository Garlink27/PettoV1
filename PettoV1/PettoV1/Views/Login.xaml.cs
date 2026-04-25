using PettoV1.ViewModels;

namespace PettoV1.Views
{
    public partial class Login : ContentPage
    {
        public Login()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (BindingContext is null)
            {
                var vm = IPlatformApplication.Current?.Services
                         .GetService<LoginViewModel>();


                BindingContext = vm;
            }
        }
    }
}