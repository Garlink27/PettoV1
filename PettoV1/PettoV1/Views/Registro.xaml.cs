using PettoV1.ViewModels;

namespace PettoV1.Views
{
    public partial class Registro : ContentPage
    {
        public Registro(RegistroViewModel vm)
        {
            InitializeComponent();
            BindingContext = vm;
        }
    }
}