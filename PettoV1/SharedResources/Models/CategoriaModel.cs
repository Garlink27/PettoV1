namespace SharedResources.Models
{
    public class CategoriaModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public ICollection<TareaModel> Tareas { get; set; } = new List<TareaModel>();
    }
}
