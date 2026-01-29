namespace Frontend.Application.DTOs;

public sealed record UsuarioDto(
    int IdUsuario,
    string Username,
    string Nombre,
    string Apellido,
    string Email,
    int IdRol,
    string NombreRol,
    bool Activo
);
