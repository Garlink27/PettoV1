using PettoV1.ViewModels;

namespace PettoV1.Views
{
    public partial class DetalleTarea : ContentPage
    {
        public DetalleTarea(DetalleTareaViewModel vm)
        {
            InitializeComponent();
            BindingContext = vm;
        }
    }
}