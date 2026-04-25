using PettoV1.ViewModels;

namespace PettoV1.Views
{
    public partial class Login : ContentPage
    {
        public Login(LoginViewModel vm)
        {
            InitializeComponent();
            BindingContext = vm;
        }
    }
}