
CREATE TABLE [dbo].[AuditoriaLog] (
    [IdLog]         BIGINT          NOT NULL    IDENTITY(1,1),
    [UsuarioId]     INT             NOT NULL,
    [Modulo]        NVARCHAR(50)    NULL,
    [Servicio]      NVARCHAR(100)   NOT NULL,
    [TipoAccion]    NVARCHAR(20)    NOT NULL,
    [Tabla]         NVARCHAR(100)   NULL,
    [RegistroId]    NVARCHAR(50)    NULL,
    [Peticion]      NVARCHAR(2000)  NULL,
    [Respuesta]     NVARCHAR(2000)  NULL,
    [ValorAnterior] NVARCHAR(MAX)   NULL,
    [ValorNuevo]    NVARCHAR(MAX)   NULL,
    [Referencia]    NVARCHAR(500)   NULL,
    [FechaEvento]   DATETIME2       NOT NULL    CONSTRAINT [DF_AuditoriaLog_FechaEvento] DEFAULT (GETUTCDATE()),
    [DireccionIP]   NVARCHAR(45)    NOT NULL,

    CONSTRAINT [PK_AuditoriaLog] PRIMARY KEY CLUSTERED ([IdLog] ASC)
);
GO

-- Búsquedas 
--por usuario
CREATE NONCLUSTERED INDEX [IX_AuditoriaLog_UsuarioId]
    ON [dbo].[AuditoriaLog] ([UsuarioId] ASC);
GO

-- por fecha (reportes por rango de fechas)
CREATE NONCLUSTERED INDEX [IX_AuditoriaLog_FechaEvento]
    ON [dbo].[AuditoriaLog] ([FechaEvento] DESC);
GO

-- por módulo y acción
CREATE NONCLUSTERED INDEX [IX_AuditoriaLog_Modulo_TipoAccion]
    ON [dbo].[AuditoriaLog] ([Modulo] ASC, [TipoAccion] ASC);
GO

-- por tabla y registro específico
CREATE NONCLUSTERED INDEX [IX_AuditoriaLog_Tabla_RegistroId]
    ON [dbo].[AuditoriaLog] ([Tabla] ASC, [RegistroId] ASC);
GO