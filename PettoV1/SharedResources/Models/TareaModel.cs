namespace SharedResources.Models
{
    public class TareaModel
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public DateTime FechaLimite { get; set; }
        public bool Completada { get; set; }
        public int CategoriaId { get; set; }
        public CategoriaModel? Categoria { get; set; }
    }
}
