namespace SharedResources.Models
{
    public class MascotaModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public int PuntosDesCuidado { get; set; }
        public int UsuarioId { get; set; }
        public UsuarioModel Usuario { get; set; } = new();
    }
}
