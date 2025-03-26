/* -----------------------------------------------------------------------------------
    1. Creación de la base de datos
----------------------------------------------------------------------------------- */
/*
CREATE DATABASE JackDb;
*/
USE JackDb;
GO

/* -----------------------------------------------------------------------------------
    2. Limpieza de tablas 
----------------------------------------------------------------------------------- */
IF OBJECT_ID('dbo.AlquilerDetalle', 'U') IS NOT NULL DROP TABLE dbo.AlquilerDetalle;
IF OBJECT_ID('dbo.Alquiler', 'U') IS NOT NULL DROP TABLE dbo.Alquiler;
IF OBJECT_ID('dbo.Copia', 'U') IS NOT NULL DROP TABLE dbo.Copia;
IF OBJECT_ID('dbo.ListaNegra', 'U') IS NOT NULL DROP TABLE dbo.ListaNegra;
IF OBJECT_ID('dbo.Cliente', 'U') IS NOT NULL DROP TABLE dbo.Cliente;
GO

/* -----------------------------------------------------------------------------------
    3. Creación de tablas
----------------------------------------------------------------------------------- */

/* ------------------- Tabla: Cliente ------------------- */
CREATE TABLE Cliente (
    ClienteId INT PRIMARY KEY IDENTITY,
    Nombres NVARCHAR(100) NOT NULL,
    DocumentoIdentidad NVARCHAR(20) NOT NULL UNIQUE,
    Telefono NVARCHAR(20),
    Direccion NVARCHAR(200)
);
GO

/* ------------------- Tabla: ListaNegra ------------------- */
CREATE TABLE ListaNegra (
    Id INT PRIMARY KEY IDENTITY,
    ClienteId INT NOT NULL UNIQUE,
    Motivo NVARCHAR(200),
    FechaRegistro DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (ClienteId) REFERENCES Cliente(ClienteId)
);
GO

/* ------------------- Tabla: Copia ------------------- */
CREATE TABLE Copia (
    CopiaId INT PRIMARY KEY IDENTITY,
    CodigoBarras NVARCHAR(50) NOT NULL UNIQUE,
    Estado NVARCHAR(20) NOT NULL
        CHECK (Estado IN ('Disponible', 'Prestado', 'Dañado', 'Extraviado'))
);
GO

/* ------------------- Tabla: Alquiler ------------------- */
CREATE TABLE Alquiler (
    AlquilerId INT PRIMARY KEY IDENTITY,
    ClienteId INT NOT NULL,
    FechaInicio DATETIME NOT NULL DEFAULT GETDATE(),
    FechaFin DATETIME NOT NULL,
    FechaDevolucion DATETIME NULL,
    Penalidad DECIMAL(10,2) DEFAULT 0,
    FOREIGN KEY (ClienteId) REFERENCES Cliente(ClienteId)
);
GO

/* ------------------- Tabla: AlquilerDetalle ------------------- */
CREATE TABLE AlquilerDetalle (
    DetalleId INT PRIMARY KEY IDENTITY,
    AlquilerId INT NOT NULL,
    CopiaId INT NOT NULL,
    FOREIGN KEY (AlquilerId) REFERENCES Alquiler(AlquilerId),
    FOREIGN KEY (CopiaId) REFERENCES Copia(CopiaId),
    CONSTRAINT UQ_Alquiler_Copia UNIQUE (AlquilerId, CopiaId)
);
GO

/* -----------------------------------------------------------------------------------
    4. Insertar datos de prueba
----------------------------------------------------------------------------------- */

/* ---- Insertar clientes ---- */
INSERT INTO Cliente (Nombres, DocumentoIdentidad, Telefono, Direccion)
VALUES
('Jack Aguilar', '12345678', '999111222', 'Av. Principal 123'),
('José Aguilar', '87654321', '999333444', 'Jr. Secundario 456');

/* ---- Marcar a José en ListaNegra ---- */
INSERT INTO ListaNegra (ClienteId, Motivo)
VALUES (
    (SELECT ClienteId FROM Cliente WHERE DocumentoIdentidad = '87654321'),
    'Retraso reiterado en devoluciones'
);

/* ---- Insertar copias ---- 
   Estado se corresponde con los valores: 
   ('Disponible','Prestado','Dañado','Extraviado')
*/
INSERT INTO Copia (CodigoBarras, Estado)
VALUES 
('C001', 'Disponible'),
('C002', 'Prestado'),    -- ya está prestado
('C003', 'Disponible'),
('C004', 'Dañado');

/* ---- Insertar un alquiler previo (para el caso de prueba) ---- */
INSERT INTO Alquiler (ClienteId, FechaInicio, FechaFin)
VALUES (
    (SELECT ClienteId FROM Cliente WHERE DocumentoIdentidad = '12345678'),
    GETDATE(),                          -- FechaInicio
    DATEADD(DAY, 7, GETDATE())          -- FechaFin: 7 días después
);

DECLARE @LastAlquilerId INT = SCOPE_IDENTITY();

INSERT INTO AlquilerDetalle (AlquilerId, CopiaId)
VALUES (
    @LastAlquilerId,
    (SELECT CopiaId FROM Copia WHERE CodigoBarras = 'C002')
);

/*
Casos para probar en tu endpoint:
1. Cliente válido, copia disponible → éxito (p.ej. ClienteId=1, CopiaId=1 or 3)
2. Cliente en lista negra → error (ClienteId=2)
3. Copia ya prestada (C002) → error
4. Copia dañada (C004) → error
*/
