using PettoV1.ViewModels;

namespace PettoV1.Views
{
    public partial class Configuracion : ContentPage
    {
        public Configuracion( )
        {
            InitializeComponent();        
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            BindingContext ??= IPlatformApplication.Current?.Services
                              .GetService<ConfiguracionViewModel>();
        }

    }
}