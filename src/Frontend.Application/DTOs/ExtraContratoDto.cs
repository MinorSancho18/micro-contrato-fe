namespace Frontend.Application.DTOs;

public sealed record ExtraContratoDto(
    int IdExtraContrato,
    int IdContrato,
    int IdExtra,
    string Descripcion,
    decimal Costo,
    int Cantidad,
    decimal Subtotal
);
