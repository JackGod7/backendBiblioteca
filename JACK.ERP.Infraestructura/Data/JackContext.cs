// JACK.ERP.Infraestructura\Data\JackContext.cs
using JACK.ERP.Dominio.Entities;
using Microsoft.EntityFrameworkCore;

namespace JACK.ERP.Infraestructura.Data
{
    public class JackContext : DbContext
    {
        public JackContext(DbContextOptions<JackContext> options) : base(options) { }

        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<ListaNegra> ListaNegra { get; set; }
        public DbSet<Copia> Copias { get; set; }
        public DbSet<Alquiler> Alquileres { get; set; }
        public DbSet<AlquilerDetalle> AlquilerDetalles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configs de Copia con enum => string
            modelBuilder.Entity<Copia>(entity =>
            {
                entity.ToTable("Copia");
                entity.HasKey(e => e.CopiaId);

                entity.Property(e => e.CodigoBarras)
                    .HasMaxLength(50)
                    .IsRequired();

                // Mapeo de enum a string
                entity.Property(e => e.Estado)
                    .HasConversion<string>()
                    .HasMaxLength(20)
                    .IsRequired();
            });

            // Resto de entidades, en sintonía con la BD
            modelBuilder.Entity<Cliente>(entity =>
            {
                entity.ToTable("Cliente");
                entity.HasKey(e => e.ClienteId);
                entity.Property(e => e.Nombres).HasMaxLength(100).IsRequired();
                entity.Property(e => e.DocumentoIdentidad).HasMaxLength(20).IsRequired();
            });

            modelBuilder.Entity<ListaNegra>(entity =>
            {
                entity.ToTable("ListaNegra");
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.ClienteId).IsUnique();
                entity.HasOne(e => e.Cliente).WithOne().HasForeignKey<ListaNegra>(x => x.ClienteId);
            });

            modelBuilder.Entity<Alquiler>(entity =>
            {
                entity.ToTable("Alquiler");
                entity.HasKey(e => e.AlquilerId);
                entity.Property(e => e.Penalidad).HasColumnType("decimal(10,2)");
                entity.HasOne(e => e.Cliente).WithMany().HasForeignKey(e => e.ClienteId);
            });

            modelBuilder.Entity<AlquilerDetalle>(entity =>
            {
                entity.ToTable("AlquilerDetalle");
                entity.HasKey(e => e.DetalleId);

                entity.HasOne(d => d.Alquiler)
                      .WithMany(a => a.Detalles)
                      .HasForeignKey(d => d.AlquilerId);

                entity.HasOne(d => d.Copia)
                      .WithMany()
                      .HasForeignKey(d => d.CopiaId);

                entity.HasIndex(d => new { d.AlquilerId, d.CopiaId }).IsUnique();
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
