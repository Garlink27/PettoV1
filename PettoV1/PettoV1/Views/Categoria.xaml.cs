using PettoV1.ViewModels;
namespace PettoV1.Views;

public partial class Categoria : ContentPage
{
	public Categoria()
	{
		InitializeComponent();

    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        BindingContext ??= IPlatformApplication.Current?.Services
                          .GetService<CategoriaViewModel>();
    }
}