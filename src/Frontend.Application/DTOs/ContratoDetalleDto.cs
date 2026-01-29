namespace Frontend.Application.DTOs;

public sealed record ContratoDetalleDto(
    ContratoDto Contrato,
    List<VehiculoContratoDto> Vehiculos,
    List<ExtraContratoDto> Extras
);
