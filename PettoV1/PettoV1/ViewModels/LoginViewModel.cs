using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using SharedResources.Data;
using System.Text.RegularExpressions;

namespace PettoV1.ViewModels
{
    public partial class LoginViewModel : ObservableObject
    {
        private readonly DataContext _dataContext;

        [ObservableProperty]
        private string _email = string.Empty;

        [ObservableProperty]
        private string _contrasena = string.Empty;

        public bool IsEmailValid =>
            !string.IsNullOrWhiteSpace(Email) &&
            Regex.IsMatch(Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");

        public bool IsPasswordValid =>
            !string.IsNullOrWhiteSpace(Contrasena) && Contrasena.Length >= 6;

        public bool IsFormValid => IsEmailValid && IsPasswordValid;

        public LoginViewModel(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        partial void OnEmailChanged(string value)
        {
            OnPropertyChanged(nameof(IsEmailValid));
            OnPropertyChanged(nameof(IsFormValid));
        }

        partial void OnContrasenaChanged(string value)
        {
            OnPropertyChanged(nameof(IsPasswordValid));
            OnPropertyChanged(nameof(IsFormValid));
        }

        [RelayCommand]
        public async Task IniciarSesion()
        {
            var usuario = await _dataContext.Usuarios
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Email == Email && u.Contrasena == Contrasena);

            if (usuario is null)
            {
                await Shell.Current.DisplayAlert("Error", "Correo o contraseña incorrectos.", "OK");
                return;
            }

            await Shell.Current.GoToAsync("//MainPage");
        }

        [RelayCommand]
        public async Task IrARegistro()
        {
            await Shell.Current.GoToAsync(nameof(Views.Registro));
        }
    }
}