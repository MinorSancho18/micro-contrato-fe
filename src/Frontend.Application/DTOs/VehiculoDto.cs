namespace Frontend.Application.DTOs;

public sealed record VehiculoDto(
    int IdVehiculo,
    string Descripcion,
    int IdCategoria,
    string CategoriaDescripcion,
    decimal Costo
);
