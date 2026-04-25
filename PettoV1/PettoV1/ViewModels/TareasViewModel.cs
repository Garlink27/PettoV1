using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using PettoV1.Views;
using SharedResources.Data;
using SharedResources.Models;
using System.Text.RegularExpressions;

namespace PettoV1.ViewModels
{
    public partial class RegistroViewModel : ObservableObject
    {
        private readonly DataContext _dataContext;

        // ──────────────── Propiedades observables ────────────────

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsEmailValid))]
        [NotifyPropertyChangedFor(nameof(IsFormValid))]
        private string _email = string.Empty;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsUsernameValid))]
        [NotifyPropertyChangedFor(nameof(IsFormValid))]
        private string _nombreUsuario = string.Empty;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsPasswordValid))]
        [NotifyPropertyChangedFor(nameof(IsConfirmPasswordValid))]
        [NotifyPropertyChangedFor(nameof(IsFormValid))]
        private string _contrasena = string.Empty;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsConfirmPasswordValid))]
        [NotifyPropertyChangedFor(nameof(IsFormValid))]
        private string _confirmarContrasena = string.Empty;

        // ──────────────── Validaciones ────────────────

        /// <summary>Email válido con formato correcto.</summary>
        public bool IsEmailValid =>
            !string.IsNullOrWhiteSpace(Email) &&
            Regex.IsMatch(Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");

        /// <summary>Nombre de usuario requerido, mínimo 3 caracteres.</summary>
        public bool IsUsernameValid =>
            !string.IsNullOrWhiteSpace(NombreUsuario) && NombreUsuario.Length >= 3;

        /// <summary>Contraseña de mínimo 6 caracteres con al menos un número.</summary>
        public bool IsPasswordValid =>
            !string.IsNullOrWhiteSpace(Contrasena) &&
            Contrasena.Length >= 6 &&
            Regex.IsMatch(Contrasena, @"\d");

        /// <summary>Confirmar contraseña debe coincidir con contraseña.</summary>
        public bool IsConfirmPasswordValid =>
            !string.IsNullOrWhiteSpace(ConfirmarContrasena) &&
            ConfirmarContrasena == Contrasena;

        public bool IsFormValid =>
            IsEmailValid && IsUsernameValid && IsPasswordValid && IsConfirmPasswordValid;

        // ──────────────── Constructor ────────────────

        public RegistroViewModel(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        // ──────────────── Comandos ────────────────

        [RelayCommand]
        public async Task Registrarse()
        {
            bool emailExiste = await _dataContext.Usuarios
                .AnyAsync(u => u.Email == Email);

            if (emailExiste)
            {
                await Shell.Current.DisplayAlert(
                    "Error", "Ya existe una cuenta con ese correo.", "OK");
                return;
            }

            var nuevoUsuario = new UsuarioModel
            {
                Email = Email,
                NombreUsuario = NombreUsuario,
                Contrasena = Contrasena   // En producción: hashear con BCrypt
            };

            await _dataContext.Usuarios.AddAsync(nuevoUsuario);
            await _dataContext.SaveChangesAsync();

            await Shell.Current.DisplayAlert(
                "Éxito", "Cuenta creada exitosamente.", "OK");
            await Shell.Current.GoToAsync("..");
        }

        [RelayCommand]
        public async Task IrALogin()
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}

