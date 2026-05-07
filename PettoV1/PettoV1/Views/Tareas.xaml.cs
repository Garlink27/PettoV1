using PettoV1.ViewModels;

namespace PettoV1.Views
{
    public partial class Tareas : ContentPage
    {
        public Tareas()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            BindingContext ??= IPlatformApplication.Current?.Services
                              .GetService<TareasViewModel>();

            if (BindingContext is TareasViewModel vm)
                await vm.InicializarAsync();
        }
    }
}