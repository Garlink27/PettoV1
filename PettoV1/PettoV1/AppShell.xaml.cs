using PettoV1.Views;

namespace PettoV1
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            // Registro de rutas para páginas de detalle (sin TabBar)
            Routing.RegisterRoute(nameof(Registro), typeof(Registro));
            Routing.RegisterRoute(nameof(Perfil), typeof(Perfil));
            Routing.RegisterRoute(nameof(Categoria), typeof(Categoria));
            Routing.RegisterRoute(nameof(DetalleTarea), typeof(DetalleTarea));
        }

        private async void OnCerrarSesionClicked(object sender, EventArgs e)
        {
            bool confirmar = await DisplayAlert(
                "Cerrar sesión",
                "¿Seguro que quieres cerrar sesión?",
                "Sí", "Cancelar");

            if (!confirmar) return;

            // Limpiar datos de sesión guardados en Preferences
            Preferences.Remove("UsuarioId");
            Preferences.Remove("NombreUsuario");
            Preferences.Remove("Email");

            // Cerrar el flyout antes de navegar
            FlyoutIsPresented = false;

            // Navegar al Login (navegación absoluta, limpia la pila)
            await GoToAsync("//Login");
        }
    }
}