using Microsoft.EntityFrameworkCore;
using SharedResources.Models;

namespace SharedResources.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<UsuarioModel> Usuarios { get; set; }
        public DbSet<TareaModel> Tareas { get; set; }
        public DbSet<CategoriaModel> Categorias { get; set; }
        public DbSet<MascotaModel> Mascotas { get; set; }
        public DbSet<MensajeModel> Mensajes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UsuarioModel>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(100);
                entity.Property(e => e.NombreUsuario).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Contrasena).IsRequired();
                entity.HasIndex(e => e.Email).IsUnique();
                entity.HasMany(e => e.Mascotas)
                      .WithOne(m => m.Usuario)
                      .HasForeignKey(m => m.UsuarioId);
            });

            modelBuilder.Entity<CategoriaModel>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Nombre).IsRequired().HasMaxLength(50);
                entity.HasMany(e => e.Tareas)
                      .WithOne(t => t.Categoria)
                      .HasForeignKey(t => t.CategoriaId);
            });

            modelBuilder.Entity<TareaModel>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Titulo).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Descripcion).HasMaxLength(500);
            });

            modelBuilder.Entity<MascotaModel>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Nombre).IsRequired().HasMaxLength(50);
            });

            modelBuilder.Entity<MensajeModel>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Contenido).IsRequired();
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}