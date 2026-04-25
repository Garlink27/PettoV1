using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using PettoV1.Pages;
using PettoV1.Views;
using SharedResources.Data;
using System.Text.RegularExpressions;

namespace PettoV1.ViewModels
{
    public partial class LoginViewModel : ObservableObject
    {
        private readonly DataContext _dataContext;

        // ──────────────── Propiedades observables ────────────────

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsEmailValid))]
        [NotifyPropertyChangedFor(nameof(IsFormValid))]
        private string _email = string.Empty;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsPasswordValid))]
        [NotifyPropertyChangedFor(nameof(IsFormValid))]
        private string _contrasena = string.Empty;


        /// <summary>Correo válido si tiene formato email.</summary>
        public bool IsEmailValid =>
            !string.IsNullOrWhiteSpace(Email) &&
            Regex.IsMatch(Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");

        /// <summary>Contraseña válida si tiene al menos 6 caracteres.</summary>
        public bool IsPasswordValid =>
            !string.IsNullOrWhiteSpace(Contrasena) && Contrasena.Length >= 6;

        /// <summary>Formulario válido cuando email y contraseña son válidos.</summary>
        public bool IsFormValid => IsEmailValid && IsPasswordValid;


        public LoginViewModel(DataContext dataContext)
        {
            _dataContext = dataContext;
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

            // Navegar al Home y limpiar el historial de navegación
            await Shell.Current.GoToAsync($"//{nameof(MainPage)}");
        }

        [RelayCommand]
        public async Task IrARegistro()
        {
            await Shell.Current.GoToAsync(nameof(Registro));
        }
    }
}
