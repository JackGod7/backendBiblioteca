using JACK.ERP.Dominio.Entities;
using Microsoft.EntityFrameworkCore;

namespace JACK.ERP.Infraestructura.Data
{
    public class JackContext : DbContext
    {
        public JackContext(DbContextOptions<JackContext> options) : base(options)
        {
        }

        // DbSets
        public DbSet<Copia> Copias { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Alquiler> Alquileres { get; set; }
        public DbSet<AlquilerDetalle> AlquilerDetalles { get; set; }
        public DbSet<ListaNegra> ListaNegra { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // =======================
            //  Mapeos de las entidades
            // =======================

            // Tabla: Copia
            modelBuilder.Entity<Copia>(entity =>
            {
                entity.ToTable("Copia");
                entity.HasKey(e => e.CopiaId);

                entity.Property(e => e.CodigoBarras)
                      .HasMaxLength(50)
                      .IsRequired();

                // El CHECK de Estado se hace en la DB; en EF lo puedes reflejar con validaciones, enumeraciones, etc.
                entity.Property(e => e.Estado)
                      .HasMaxLength(20)
                      .IsRequired();
            });

            // Tabla: Cliente
            modelBuilder.Entity<Cliente>(entity =>
            {
                entity.ToTable("Cliente");
                entity.HasKey(e => e.ClienteId);

                entity.Property(e => e.Nombres)
                      .HasMaxLength(100)
                      .IsRequired();

                entity.Property(e => e.DocumentoIdentidad)
                      .HasMaxLength(20)
                      .IsRequired();

                entity.Property(e => e.Telefono)
                      .HasMaxLength(20);

                entity.Property(e => e.Direccion)
                      .HasMaxLength(200);

                // Si quieres reflejar la UNIQUE constraint de DocumentoIdentidad
                entity.HasIndex(e => e.DocumentoIdentidad).IsUnique();
            });

            // Tabla: ListaNegra
            modelBuilder.Entity<ListaNegra>(entity =>
            {
                entity.ToTable("ListaNegra");
                entity.HasKey(e => e.Id);

                // Relación 1:1 con Cliente (ClienteId único)
                entity.HasIndex(e => e.ClienteId).IsUnique();

                entity.Property(e => e.Motivo)
                      .HasMaxLength(200);

                entity.Property(e => e.FechaRegistro)
                      .HasDefaultValueSql("GETDATE()");

                // Relación foránea
                entity.HasOne(e => e.Cliente)
                      .WithOne() // o .WithMany() si deseas otro diseño
                      .HasForeignKey<ListaNegra>(e => e.ClienteId);
            });

            // Tabla: Alquiler
            modelBuilder.Entity<Alquiler>(entity =>
            {
                entity.ToTable("Alquiler");
                entity.HasKey(e => e.AlquilerId);

                entity.Property(e => e.FechaInicio)
                      .IsRequired()
                      .HasDefaultValueSql("GETDATE()");

                entity.Property(e => e.FechaFin)
                      .IsRequired();

                entity.Property(e => e.Penalidad)
                      .HasColumnType("decimal(10,2)")
                      .HasDefaultValue(0);

                // Relación foránea con Cliente
                entity.HasOne(e => e.Cliente)
                      .WithMany() // o .WithMany(c => c.Alquileres) si tienes la colección en Cliente
                      .HasForeignKey(e => e.ClienteId);
            });

            // Tabla: AlquilerDetalle
            modelBuilder.Entity<AlquilerDetalle>(entity =>
            {
                entity.ToTable("AlquilerDetalle");
                entity.HasKey(e => e.DetalleId);

                // Relación foránea con Alquiler
                entity.HasOne(e => e.Alquiler)
                      .WithMany(a => a.Detalles)
                      .HasForeignKey(e => e.AlquilerId);

                // Relación foránea con Copia
                entity.HasOne(e => e.Copia)
                      .WithMany() // o .WithMany(c => c.AlquilerDetalles) si decides modelarlo así
                      .HasForeignKey(e => e.CopiaId);

                // UNIQUE constraint: (AlquilerId, CopiaId)
                entity.HasIndex(e => new { e.AlquilerId, e.CopiaId })
                      .IsUnique();
            });

            // =======================
            //  Otras configuraciones
            // =======================

            base.OnModelCreating(modelBuilder);
        }
    }
}
