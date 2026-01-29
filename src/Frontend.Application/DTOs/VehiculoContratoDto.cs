namespace Frontend.Application.DTOs;

public sealed record VehiculoContratoDto(
    int IdVehiculoContrato,
    int IdContrato,
    int IdVehiculo,
    string DescripcionVehiculo,
    int DiasDeUso,
    decimal CostoDiario,
    decimal Subtotal,
    bool Inspeccionado
);
