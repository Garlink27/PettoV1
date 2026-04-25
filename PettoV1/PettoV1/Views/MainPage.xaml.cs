using PettoV1.ViewModels;

namespace PettoV1.Views
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            BindingContext ??= IPlatformApplication.Current?.Services
                              .GetService<MainViewModel>();
        }
    }
}