namespace Frontend.Application.DTOs;

public sealed record ContratoDetalleDto(
    int IdContrato,
    int IdCliente,
    string NombreCliente,
    DateTime FechaRecogida,
    DateTime FechaDevolucion,
    int IdSucursal,
    string NombreSucursal,
    int IdEstado,
    string DescripcionEstado,
    int IdUsuario,
    string NombreUsuario,
    decimal MontoTotal,
    bool Confirmado,
    bool GarantiaAprobada,
    decimal Saldo,
    List<VehiculoContratoDto> Vehiculos,
    List<ExtraContratoDto> Extras
);
