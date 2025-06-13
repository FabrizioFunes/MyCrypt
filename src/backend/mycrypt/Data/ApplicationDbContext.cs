using Microsoft.EntityFrameworkCore;
using mycrypt.Models.Entidades;

namespace mycrypt.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options) { }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Cripto> Criptos { get; set; }
        public DbSet<Exchange> Exchanges { get; set; }
        public DbSet<Transaccion> Transacciones { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Usuario>()
                .Property(u => u.TotalPesos)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Transaccion>()
                .Property(t => t.MontoARS)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Transaccion>()
                .Property(t => t.Cantidad)
                .HasPrecision(18, 8);

            // Restricciones CHECK
            modelBuilder.Entity<Transaccion>()
                .HasCheckConstraint("CK_Tipo", "Tipo IN ('Compra', 'Venta')");

            modelBuilder.Entity<Transaccion>()
                .HasCheckConstraint("CK_Monto", "MontoARS >= 0");
        }

    }
}
