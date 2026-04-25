using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using SharedResources.Data;

namespace PettoV1.ViewModels
{
    public partial class ConfiguracionViewModel : ObservableObject
    {
        private readonly DataContext _dataContext;

        [ObservableProperty] private string _idiomaSeleccionado = "Español";
        [ObservableProperty] private string _contrasenaActual = string.Empty;
        [ObservableProperty] private string _nuevaContrasena = string.Empty;
        [ObservableProperty] private string _confirmarNuevaContrasena = string.Empty;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsNuevaContrasenaValida))]
        [NotifyPropertyChangedFor(nameof(IsFormCambioValido))]
        private bool _isContrasenaActualValida = true;

        public bool IsNuevaContrasenaValida =>
            !string.IsNullOrWhiteSpace(NuevaContrasena) &&
            NuevaContrasena.Length >= 6 &&
            NuevaContrasena == ConfirmarNuevaContrasena;

        public bool IsFormCambioValido => IsContrasenaActualValida && IsNuevaContrasenaValida;

        public List<string> Idiomas { get; } = new() { "Español", "English", "Français" };

        public ConfiguracionViewModel(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        partial void OnNuevaContrasenaChanged(string value) =>
            OnPropertyChanged(nameof(IsNuevaContrasenaValida));

        partial void OnConfirmarNuevaContrasenaChanged(string value) =>
            OnPropertyChanged(nameof(IsNuevaContrasenaValida));

        [RelayCommand]
        public async Task CambiarContrasena()
        {
            var usuario = await _dataContext.Usuarios.FirstOrDefaultAsync();
            if (usuario is null) return;

            if (usuario.Contrasena != ContrasenaActual)
            {
                IsContrasenaActualValida = false;
                await Shell.Current.DisplayAlert(
                    "Error", "La contraseña actual es incorrecta.", "OK");
                return;
            }

            IsContrasenaActualValida = true;
            usuario.Contrasena = NuevaContrasena;
            _dataContext.Usuarios.Update(usuario);
            await _dataContext.SaveChangesAsync();

            ContrasenaActual = string.Empty;
            NuevaContrasena = string.Empty;
            ConfirmarNuevaContrasena = string.Empty;

            await Shell.Current.DisplayAlert("Éxito", "Contraseña actualizada.", "OK");
        }

        [RelayCommand]
        public async Task GuardarIdioma()
        {
            await Shell.Current.DisplayAlert(
                "Idioma", $"Idioma cambiado a: {IdiomaSeleccionado}", "OK");
        }

        [RelayCommand]
        public async Task CerrarSesion()
        {
            string resp = await Shell.Current.DisplayActionSheet(
                "¿Cerrar sesión?", "Cancelar", "Cerrar sesión");
            if (resp != "Cerrar sesión") return;
            await Shell.Current.GoToAsync("//LoginPage");
        }

        [RelayCommand]
        public async Task Regresar() => await Shell.Current.GoToAsync("..");
    }
}