namespace SharedResources.Models
{
    public class UsuarioModel
    {
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string NombreUsuario { get; set; } = string.Empty;
        public string Contrasena { get; set; } = string.Empty;
        public ICollection<MascotaModel> Mascotas { get; set; } = new List<MascotaModel>();
    }
}
