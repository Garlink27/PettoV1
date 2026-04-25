using PettoV1.ViewModels;
namespace PettoV1.Views;

public partial class Categoria : ContentPage
{
	public Categoria(CategoriaViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
    }
}