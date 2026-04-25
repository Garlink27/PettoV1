using PettoV1.ViewModels;

namespace PettoV1.Views
{
    public partial class DetalleTarea : ContentPage
    {
        public DetalleTarea()
        {
            InitializeComponent();

        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            BindingContext ??= IPlatformApplication.Current?.Services
                              .GetService<DetalleTareaViewModel>();
        }
    }
}