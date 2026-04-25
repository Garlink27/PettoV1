using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PettoV1.Views;

namespace PettoV1.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        [ObservableProperty]
        private string _fechaHora = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");

        [ObservableProperty]
        private int _puntosDeCuidado = 0;

        [ObservableProperty]
        private string _nombreMascota = "Mi Mascota";

        public MainViewModel()
        {
            // Actualizar la hora cada segundo
            var timer = Application.Current?.Dispatcher.CreateTimer();
            if (timer != null)
            {
                timer.Interval = TimeSpan.FromSeconds(1);
                timer.Tick += (s, e) =>
                    FechaHora = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
                timer.Start();
            }
        }

        [RelayCommand]
        public async Task IrAPerfil()
        {
            await Shell.Current.GoToAsync(nameof(Perfil));
        }

        [RelayCommand]
        public void AbrirMenu()
        {
            Shell.Current.FlyoutIsPresented = true;
        }
    }
}
