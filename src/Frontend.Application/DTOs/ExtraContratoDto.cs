namespace Frontend.Application.DTOs;

public sealed record ExtraContratoDto(
    int IdExtraContrato,
    int IdContrato,
    int IdExtra,
    string DescripcionExtra,
    int DiasDeUso,
    decimal CostoDiario,
    decimal Subtotal
);
