namespace Frontend.Application.DTOs;

public sealed record EstadoContratoDto(
    int IdEstado,
    string Descripcion,
    bool Activo
);
