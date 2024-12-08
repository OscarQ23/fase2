using Microsoft.EntityFrameworkCore;

namespace Fase1PED.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Categorias> Categorias { get; set; }
        public DbSet<Tarjeta> Tarjetas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Usuario>()
                .HasOne(u => u.Categoria)
                .WithMany(c => c.Usuarios)
                .HasForeignKey(u => u.CategoriaId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Usuario>()
                .HasOne(u => u.Tarjeta)
                .WithMany(t => t.Usuarios)
                .HasForeignKey(u => u.TarjetaId)
                .OnDelete(DeleteBehavior.SetNull);

            base.OnModelCreating(modelBuilder);
        }
    }
}