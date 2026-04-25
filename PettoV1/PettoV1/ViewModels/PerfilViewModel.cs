using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using SharedResources.Data;
using SharedResources.Models;

namespace PettoV1.ViewModels
{
    public partial class PerfilViewModel : ObservableObject
    {
        private readonly DataContext _dataContext;
        private UsuarioModel? _usuarioActual;

        [ObservableProperty] private string _nombreUsuario = string.Empty;
        [ObservableProperty] private string _email = string.Empty;
        [ObservableProperty] private string _telefono = string.Empty;
        [ObservableProperty] private string _contrasena = string.Empty;

        public PerfilViewModel(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task InicializarAsync()
        {
            // En producción se usa el ID del usuario en sesión
            _usuarioActual = await _dataContext.Usuarios
                .AsNoTracking()
                .FirstOrDefaultAsync();

            if (_usuarioActual is not null)
            {
                NombreUsuario = _usuarioActual.NombreUsuario;
                Email = _usuarioActual.Email;
            }
        }

        [RelayCommand]
        public async Task GuardarPerfil()
        {
            if (_usuarioActual is null) return;

            var entidad = await _dataContext.Usuarios.FindAsync(_usuarioActual.Id);
            if (entidad is not null)
            {
                entidad.NombreUsuario = NombreUsuario;
                entidad.Email = Email;
                _dataContext.Usuarios.Update(entidad);
                await _dataContext.SaveChangesAsync();
                await Shell.Current.DisplayAlert("Éxito", "Perfil actualizado.", "OK");
            }
        }

        [RelayCommand]
        public async Task Regresar() => await Shell.Current.GoToAsync("..");

        [RelayCommand]
        public async Task IrAConfiguracion() =>
            await Shell.Current.GoToAsync("ConfiguracionPage");
    }
}
