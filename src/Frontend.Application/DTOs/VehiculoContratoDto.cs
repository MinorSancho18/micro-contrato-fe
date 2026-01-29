namespace Frontend.Application.DTOs;

public sealed record VehiculoContratoDto(
    int IdVehiculoContrato,
    int IdContrato,
    int IdVehiculo,
    string Placa,
    string Modelo,
    decimal TarifaDiaria,
    decimal Subtotal,
    bool Inspeccionado
);
