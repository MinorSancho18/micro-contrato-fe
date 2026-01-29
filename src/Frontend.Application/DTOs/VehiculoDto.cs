namespace Frontend.Application.DTOs;

public sealed record VehiculoDto(
    int IdVehiculo,
    string Placa,
    string Modelo,
    int Anio,
    int IdMarca,
    string NombreMarca,
    int IdTipoVehiculo,
    string DescripcionTipo,
    decimal TarifaDiaria,
    bool Disponible,
    bool Activo
);
