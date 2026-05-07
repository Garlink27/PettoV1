namespace SharedResources.Models
{
    public class CategoriaModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;

        // Vínculo con el usuario
        public int UsuarioId { get; set; }
        public UsuarioModel? Usuario { get; set; }

        public ICollection<TareaModel> Tareas { get; set; } = new List<TareaModel>();
    }
}