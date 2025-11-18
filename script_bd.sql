CREATE DATABASE TrabajadoresPrueba;
GO
USE TrabajadoresPrueba;
GO

CREATE TABLE Trabajador (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Nombres VARCHAR(100),
    Apellidos VARCHAR(100),
    TipoDocumento VARCHAR(20),
    NumeroDocumento VARCHAR(20) UNIQUE,
    Sexo CHAR(1),
    FechaNacimiento DATE,
    Foto VARCHAR(200),
    Direccion VARCHAR(200)
);
GO

CREATE PROCEDURE sp_ListarTrabajadores
AS
BEGIN
    SELECT * FROM Trabajador;
END
GO

CREATE PROCEDURE sp_ObtenerTrabajador
    @Id INT
AS
BEGIN
    SELECT * FROM Trabajador WHERE Id = @Id;
END
GO

CREATE PROCEDURE sp_RegistrarTrabajador
    @Nombres VARCHAR(100),
    @Apellidos VARCHAR(100),
    @TipoDocumento VARCHAR(20),
    @NumeroDocumento VARCHAR(20),
    @Sexo CHAR(1),
    @FechaNacimiento DATE,
    @Foto VARCHAR(200),
    @Direccion VARCHAR(200)
AS
BEGIN
    INSERT INTO Trabajador (Nombres, Apellidos, TipoDocumento, NumeroDocumento, Sexo, FechaNacimiento, Foto, Direccion)
    VALUES (@Nombres, @Apellidos, @TipoDocumento, @NumeroDocumento, @Sexo, @FechaNacimiento, @Foto, @Direccion);
END
GO
CREATE PROCEDURE sp_ActualizarTrabajador
    @Id INT,
    @Nombres VARCHAR(100),
    @Apellidos VARCHAR(100),
    @TipoDocumento VARCHAR(20),
    @NumeroDocumento VARCHAR(20),
    @Sexo CHAR(1),
    @FechaNacimiento DATE,
    @Foto VARCHAR(200),
    @Direccion VARCHAR(200)
AS
BEGIN
    UPDATE Trabajador
    SET 
        Nombres = @Nombres,
        Apellidos = @Apellidos,
        TipoDocumento = @TipoDocumento,
        NumeroDocumento = @NumeroDocumento,
        Sexo = @Sexo,
        FechaNacimiento = @FechaNacimiento,
        Foto = @Foto,
        Direccion = @Direccion
    WHERE Id = @Id;
END
GO

CREATE PROCEDURE sp_EliminarTrabajador
    @Id INT
AS
BEGIN
    DELETE FROM Trabajador WHERE Id = @Id;
END
GO

CREATE PROCEDURE sp_ListarTrabajadoresFiltrado
    @Sexo CHAR(1) = NULL
AS
BEGIN
    IF @Sexo IS NULL OR @Sexo = ''
    BEGIN
        SELECT * FROM Trabajador;
    END
    ELSE
    BEGIN
        SELECT * FROM Trabajador WHERE Sexo = @Sexo;
    END
END
GO

SELECT 
    name AS NombreProcedimiento,
    create_date AS FechaCreacion,
    modify_date AS UltimaModificacion
FROM sys.procedures
ORDER BY name;

EXEC sp_ListarTrabajadores;
EXEC sp_ObtenerTrabajador @Id = 7;

EXEC sp_ListarTrabajadoresFiltrado @Sexo = 'M';
EXEC sp_ListarTrabajadoresFiltrado @Sexo = 'F';