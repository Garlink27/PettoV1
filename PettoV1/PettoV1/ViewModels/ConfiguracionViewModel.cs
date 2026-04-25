using PettoV1.ViewModels;

namespace PettoV1.Views
{
    public partial class ConfiguracionPage : ContentPage
    {
        public ConfiguracionPage(ConfiguracionViewModel vm)
        {
            InitializeComponent();
            BindingContext = vm;
        }
    }
}