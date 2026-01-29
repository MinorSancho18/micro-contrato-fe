namespace Frontend.Application.DTOs;

public sealed record ContratoDto(
    int IdContrato,
    int IdCliente,
    string NombreCliente,
    DateTime FechaRecogida,
    DateTime FechaDevolucion,
    int IdSucursal,
    int IdEstado,
    string DescripcionEstado,
    int IdUsuario,
    decimal MontoTotal,
    bool Confirmado,
    bool GarantiaAprobada,
    decimal Saldo
);
