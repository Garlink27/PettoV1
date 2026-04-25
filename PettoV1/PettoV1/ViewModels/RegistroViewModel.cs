using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using SharedResources.Data;
using SharedResources.Models;
using System.Text.RegularExpressions;

namespace PettoV1.ViewModels
{
    public partial class RegistroViewModel : ObservableObject
    {
        private readonly DataContext _dataContext;

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

        public bool IsEmailValid =>
            !string.IsNullOrWhiteSpace(Email) &&
            Regex.IsMatch(Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");

        public bool IsUsernameValid =>
            !string.IsNullOrWhiteSpace(NombreUsuario) && NombreUsuario.Length >= 3;

        public bool IsPasswordValid =>
            !string.IsNullOrWhiteSpace(Contrasena) &&
            Contrasena.Length >= 6 &&
            Regex.IsMatch(Contrasena, @"\d");

        public bool IsConfirmPasswordValid =>
            !string.IsNullOrWhiteSpace(ConfirmarContrasena) &&
            ConfirmarContrasena == Contrasena;

        public bool IsFormValid =>
            IsEmailValid && IsUsernameValid && IsPasswordValid && IsConfirmPasswordValid;

        public RegistroViewModel(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

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
                Contrasena = Contrasena
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