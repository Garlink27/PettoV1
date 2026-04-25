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
    }
}