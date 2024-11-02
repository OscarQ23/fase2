using Microsoft.EntityFrameworkCore;

namespace Fase1PED.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Categorias> Categorias { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configurar la clave foránea para asegurarse de que Entity Framework la entienda correctamente
            modelBuilder.Entity<Usuario>()
                .HasOne(u => u.Categoria)
                .WithMany()
                .HasForeignKey(u => u.CategoriaId)
                .OnDelete(DeleteBehavior.Restrict); // Configurar el comportamiento de eliminación

            base.OnModelCreating(modelBuilder);
        }
    }
}