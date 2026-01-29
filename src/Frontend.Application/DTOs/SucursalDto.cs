namespace Frontend.Application.DTOs;

public sealed record SucursalDto(
    int IdSucursal,
    string Nombre,
    string Direccion,
    string? Telefono,
    bool Activo
);
