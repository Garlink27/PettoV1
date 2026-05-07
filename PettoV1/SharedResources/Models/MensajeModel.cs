namespace SharedResources.Models
{
    public class MensajeModel
    {
        public int Id { get; set; }
        public string Contenido { get; set; } = string.Empty;
        public bool EsRespuestaIA { get; set; }
        public DateTime FechaHora { get; set; } = DateTime.Now;

        // Vínculo con el usuario
        public int UsuarioId { get; set; }
        public UsuarioModel? Usuario { get; set; }
    }
}