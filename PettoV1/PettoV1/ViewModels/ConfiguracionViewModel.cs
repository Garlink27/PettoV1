
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

        // ── Campos contraseña ──────────────────────────────────────────────
        [ObservableProperty] private string _contrasenaActual = string.Empty;
        [ObservableProperty] private string _nuevaContrasena = string.Empty;
        [ObservableProperty] private string _confirmarNuevaContrasena = string.Empty;

        /// <summary>True solo cuando el usuario ya intentó guardar y la contraseña actual fue incorrecta.</summary>
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsFormCambioValido))]
        private bool _contrasenaActualIncorrecta = false;

        /// <summary>
        /// Muestra error de coincidencia SOLO cuando ambos campos tienen texto y no coinciden
        /// (o la nueva contraseña es muy corta).
        /// </summary>
        public bool MostrarErrorCoincidencia =>
            (!string.IsNullOrEmpty(NuevaContrasena) || !string.IsNullOrEmpty(ConfirmarNuevaContrasena)) &&
            (NuevaContrasena != ConfirmarNuevaContrasena || NuevaContrasena.Length < 6);

        /// <summary>
        /// El formulario es válido cuando:
        /// - Hay contraseña actual escrita y no fue marcada como incorrecta
        /// - La nueva contraseña tiene al menos 6 caracteres
        /// - Nueva == Confirmar
        /// </summary>
        public bool IsFormCambioValido =>
            !string.IsNullOrEmpty(ContrasenaActual) &&
            !ContrasenaActualIncorrecta &&
            NuevaContrasena.Length >= 6 &&
            NuevaContrasena == ConfirmarNuevaContrasena;

        public List<string> Idiomas { get; } = new() { "Español", "English", "Français" };

        public ConfiguracionViewModel(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        // Cuando el usuario modifica la contraseña actual, limpiamos el error previo
        partial void OnContrasenaActualChanged(string value)
        {
            ContrasenaActualIncorrecta = false;
            OnPropertyChanged(nameof(IsFormCambioValido));
        }

        partial void OnNuevaContrasenaChanged(string value)
        {
            OnPropertyChanged(nameof(MostrarErrorCoincidencia));
            OnPropertyChanged(nameof(IsFormCambioValido));
        }

        partial void OnConfirmarNuevaContrasenaChanged(string value)
        {
            OnPropertyChanged(nameof(MostrarErrorCoincidencia));
            OnPropertyChanged(nameof(IsFormCambioValido));
        }

        [RelayCommand]
        public async Task CambiarContrasena()
        {
            if (!IsFormCambioValido) return;

            int usuarioId = Preferences.Get("UsuarioId", 0);
            var usuario = await _dataContext.Usuarios.FindAsync(usuarioId);
            if (usuario is null) return;

            // Verificar contraseña actual contra BD
            if (usuario.Contrasena != ContrasenaActual)
            {
                ContrasenaActualIncorrecta = true;
                return;
            }

            // Guardar nueva contraseña
            usuario.Contrasena = NuevaContrasena;
            _dataContext.Usuarios.Update(usuario);
            await _dataContext.SaveChangesAsync();

            // Limpiar campos
            ContrasenaActual = string.Empty;
            NuevaContrasena = string.Empty;
            ConfirmarNuevaContrasena = string.Empty;
            ContrasenaActualIncorrecta = false;

            await Shell.Current.DisplayAlert("✅ Éxito", "Contraseña actualizada correctamente.", "OK");
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

            Preferences.Remove("UsuarioId");
            Preferences.Remove("NombreUsuario");
            Preferences.Remove("Email");

            await Shell.Current.GoToAsync("//Login");
        }

        [RelayCommand]
        public async Task Regresar() => await Shell.Current.GoToAsync("..");
    }
}
