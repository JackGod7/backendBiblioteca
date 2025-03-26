# 📚 JACK.ERP - Backend Biblioteca

Este repositorio contiene el backend para el sistema de gestión de biblioteca solicitado en la prueba técnica Full Stack Senior.

## 🚀 Tecnologías

- .NET 8
- C# 12
- Entity Framework Core 9
- SQL Server Express
- Arquitectura por capas (Api, Aplicación, Dominio, Infraestructura)
- MediatR + FluentValidation + DDD

## 📦 Estructura del proyecto

- `Api/`: Endpoints de préstamo, configuración de API
- `Aplicacion/`: Casos de uso (CQRS, Validaciones)
- `Dominio/`: Entidades, lógica de negocio
- `Infraestructura/`: Persistencia con EF Core
- `Migrations/`: Scripts de base de datos generados

## 🧪 Funcionalidad incluida

- Registro de préstamos
- Validación de disponibilidad de copias
- Restricción de 3 libros por cliente
- Control de penalidades por no devolución
- Registro en lista negra de clientes

## 🧰 Instrucciones de uso

1. Crear la base de datos en SQL Server: `JackDb`
2. Verificar `appsettings.json` con tu cadena de conexión local
3. Ejecutar migraciones:
   ```bash
   dotnet ef database update
   ```
4. Ejecutar el proyecto:
   ```bash
   dotnet run --project Api
   ```

## 📜 Licencia

Uso exclusivo para evaluación técnica - Atlantic City
