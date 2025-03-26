using JACK.ERP.Aplicacion.Services.Entidades;
using JACK.ERP.Dominio.Interfaces;
using JACK.ERP.Dominio.Entities;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace JACK.ERP.Tests
{
    public class PrestamoServiceTests
    {
        [Fact]
        public async Task RegistrarPrestamo_ClienteEnListaNegra_DebeLanzarExcepcion()
        {
            // Arrange
            var mockPrestamoRepo = new Mock<IPrestamoRepository>();
            var mockClienteRepo = new Mock<IClienteRepository>();
            var mockCopiaRepo = new Mock<ICopiaRepository>();

            // cliente está en lista negra
            mockClienteRepo.Setup(x => x.EstaEnListaNegraAsync(It.IsAny<int>()))
                           .ReturnsAsync(true);

            var prestamoService = new PrestamoService(
                mockPrestamoRepo.Object,
                mockClienteRepo.Object,
                mockCopiaRepo.Object,
                null 
            );

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(async () =>
            {
                await prestamoService.RegistrarPrestamoAsync(1, new List<int> { 2, 3 }, DateTime.Now.AddDays(7));
            });
        }

        [Fact]
        public async Task RegistrarPrestamo_SuperaMaximo3Copias_DebeLanzarExcepcion()
        {
            // Arrange
            var mockPrestamoRepo = new Mock<IPrestamoRepository>();
            var mockClienteRepo = new Mock<IClienteRepository>();
            var mockCopiaRepo = new Mock<ICopiaRepository>();

            //  cliente tiene ya 2 copias activas
            mockPrestamoRepo.Setup(x => x.ObtenerCopiasActivasDeClienteAsync(It.IsAny<int>()))
                            .ReturnsAsync(2);

            var prestamoService = new PrestamoService(
                mockPrestamoRepo.Object,
                mockClienteRepo.Object,
                mockCopiaRepo.Object,
                null
            );

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(async () =>
            {
                // Intenta pedir 2 copias más => 2 + 2 = 4 > 3
                await prestamoService.RegistrarPrestamoAsync(1, new List<int> { 2, 3 }, DateTime.Now.AddDays(7));
            });
        }

        [Fact]
        public async Task RegistrarPrestamo_CopiasNoDisponibles_DebeLanzarExcepcion()
        {
            // Arrange
            var mockPrestamoRepo = new Mock<IPrestamoRepository>();
            var mockClienteRepo = new Mock<IClienteRepository>();
            var mockCopiaRepo = new Mock<ICopiaRepository>();

            //  el cliente NO está en lista negra
            mockClienteRepo.Setup(x => x.EstaEnListaNegraAsync(It.IsAny<int>()))
                           .ReturnsAsync(false);

            //  que no supera 3
            mockPrestamoRepo.Setup(x => x.ObtenerCopiasActivasDeClienteAsync(It.IsAny<int>()))
                            .ReturnsAsync(1);

            //  que 1 de las copias está en "Prestado"
            var copias = new List<Copia>
            {
                new Copia { CopiaId=2, CodigoBarras="C002", Estado = JACK.ERP.Dominio.Enums.CopiaEstado.Disponible },
                new Copia { CopiaId=3, CodigoBarras="C003", Estado = JACK.ERP.Dominio.Enums.CopiaEstado.Prestado }
            };
            mockCopiaRepo.Setup(x => x.ObtenerCopiasPorIdsAsync(It.IsAny<IEnumerable<int>>()))
                         .ReturnsAsync(copias);

            var prestamoService = new PrestamoService(
                mockPrestamoRepo.Object,
                mockClienteRepo.Object,
                mockCopiaRepo.Object,
                null
            );

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(async () =>
            {
                // Pedimos 2 copias (2, 3) pero la #3 está Prestado
                await prestamoService.RegistrarPrestamoAsync(1, new List<int> { 2, 3 }, DateTime.Now.AddDays(7));
            });
        }

        [Fact]
        public async Task RegistrarPrestamo_CondicionesOk_DebeRegistrar()
        {
            // Arrange
            var mockPrestamoRepo = new Mock<IPrestamoRepository>();
            var mockClienteRepo = new Mock<IClienteRepository>();
            var mockCopiaRepo = new Mock<ICopiaRepository>();

            // Ninguna restricción
            mockClienteRepo.Setup(x => x.EstaEnListaNegraAsync(It.IsAny<int>()))
                           .ReturnsAsync(false);
            mockPrestamoRepo.Setup(x => x.ObtenerCopiasActivasDeClienteAsync(It.IsAny<int>()))
                            .ReturnsAsync(1);

            var copias = new List<Copia>
            {
                new Copia { CopiaId=2, CodigoBarras="C002", Estado = JACK.ERP.Dominio.Enums.CopiaEstado.Disponible },
                new Copia { CopiaId=3, CodigoBarras="C003", Estado = JACK.ERP.Dominio.Enums.CopiaEstado.Disponible }
            };
            mockCopiaRepo.Setup(x => x.ObtenerCopiasPorIdsAsync(It.IsAny<IEnumerable<int>>()))
                         .ReturnsAsync(copias);

            // Simulamos que RegistrarAlquilerAsync() funciona sin error
            mockPrestamoRepo.Setup(x => x.RegistrarAlquilerAsync(It.IsAny<Alquiler>(), It.IsAny<List<int>>()))
                            .Returns(Task.CompletedTask);

            var prestamoService = new PrestamoService(
                mockPrestamoRepo.Object,
                mockClienteRepo.Object,
                mockCopiaRepo.Object,
                null
            );

            // Act
            var alquiler = await prestamoService.RegistrarPrestamoAsync(1, new List<int> { 2, 3 }, DateTime.Now.AddDays(7));

            // Assert
            Assert.NotNull(alquiler);
            
        }
    }
}
