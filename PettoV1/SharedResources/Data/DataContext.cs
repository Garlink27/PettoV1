using Microsoft.EntityFrameworkCore;
using SharedResources.Models;

namespace SharedResources.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<UsuarioModel> Users { get; set; }
        public DbSet<TareaModel> TaskItems { get; set; }
    }
}
