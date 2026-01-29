namespace Frontend.Application.DTOs;

public sealed record ClienteDto(
    int IdCliente,
    string Nombre,
    string Apellido,
    string Cedula,
    string? Email,
    string? Telefono,
    bool Activo
);
