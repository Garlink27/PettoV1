using PettoV1.ViewModels;

namespace PettoV1.Views
{
    public partial class Configuracion : ContentPage
    {
        public Configuracion(ConfiguracionViewModel vm)
        {
            InitializeComponent();
            BindingContext = vm;
        }
    }
}