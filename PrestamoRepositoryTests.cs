using JACK.ERP.Dominio.Entities;
using JACK.ERP.Dominio.Enums;
using JACK.ERP.Infraestructura.Data;
using JACK.ERP.Infraestructura.Repositories.Entidades;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace JACK.ERP.IntegrationTests.Repositories
{
    public class PrestamoRepositoryTests
    {
        private JackContext GetInMemoryContext(string dbName)
        {
            var options = new DbContextOptionsBuilder<JackContext>()
                .UseInMemoryDatabase(dbName)
                .Options;
            return new JackContext(options);
        }

        [Fact]
        public async Task RegistrarAlquilerAsync_InsertaAlquilerYCambiaEstadoCopia()
        {
            // Arrange
            var context = GetInMemoryContext("RegistrarAlquilerTest1");
            var repository = new PrestamoRepository(context);

            // Insertar datos de prueba en la BD in-memory
            context.Clientes.Add(new Cliente
            {
                ClienteId = 1,
                Nombres = "Jack Aguilar",
                DocumentoIdentidad = "12345678"
            });
            context.Copias.Add(new Copia
            {
                CopiaId = 10,
                CodigoBarras = "C010",
                Estado = CopiaEstado.Disponible
            });
            await context.SaveChangesAsync();

            // Un alquiler nuevo
            var alquiler = new Alquiler
            {
                ClienteId = 1,
                FechaInicio = DateTime.Now,
                FechaFin = DateTime.Now.AddDays(7)
            };
            var copiasId = new List<int> { 10 };

            // Act
            await repository.RegistrarAlquilerAsync(alquiler, copiasId);

            // Assert
            // Verificar que se haya insertado el alquiler
            Assert.NotEqual(0, alquiler.AlquilerId);

            // Revisar que la copia cambió a “Prestado”
            var copia = await context.Copias.FindAsync(10);
            Assert.Equal(CopiaEstado.Prestado, copia.Estado);

            // Verificar detalle
            var detalle = await context.AlquilerDetalles.FindAsync(1); // ID 1 en in-memory
            Assert.NotNull(detalle);
            Assert.Equal(alquiler.AlquilerId, detalle.AlquilerId);
            Assert.Equal(10, detalle.CopiaId);
        }

        [Fact]
        public async Task ObtenerPrestamosAsync_RetornaListaDeAlquileresConDetalles()
        {
            // Arrange
            var context = GetInMemoryContext("ListarAlquilerTest");
            var repository = new PrestamoRepository(context);

            // Sembramos datos
            var cliente = new Cliente { ClienteId = 2, Nombres = "José", DocumentoIdentidad = "87654321" };
            var copia = new Copia { CopiaId = 20, CodigoBarras = "C020", Estado = CopiaEstado.Prestado };
            context.Clientes.Add(cliente);
            context.Copias.Add(copia);

            var alquiler = new Alquiler
            {
                AlquilerId = 100,
                ClienteId = cliente.ClienteId,
                FechaInicio = DateTime.Now,
                FechaFin = DateTime.Now.AddDays(7)
            };
            context.Alquileres.Add(alquiler);
            context.AlquilerDetalles.Add(new AlquilerDetalle
            {
                DetalleId = 300,
                AlquilerId = alquiler.AlquilerId,
                CopiaId = copia.CopiaId
            });

            await context.SaveChangesAsync();

            // Act
            var result = await repository.ObtenerPrestamosAsync();

            // Assert
            Assert.NotEmpty(result); // hay 1
            foreach (var a in result)
            {
                Assert.NotNull(a.Detalles);
            }
        }
    }
}
