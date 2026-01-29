using Frontend.Application.DTOs;

namespace Frontend.Application.Interfaces;

public interface IContratosApiService
{
    Task<List<ContratoDto>> ListarAsync(int? idCliente = null, int? idEstado = null, DateTime? fechaInicio = null, DateTime? fechaFin = null);
    Task<ContratoDto?> ObtenerPorIdAsync(int id);
    Task<ContratoDetalleDto?> ObtenerDetalleAsync(int id);
    Task<ContratoDto> CrearAsync(ContratoDto contrato);
    Task<ContratoDto> ActualizarAsync(int id, ContratoDto contrato);
    Task<VehiculoContratoDto> AgregarVehiculoAsync(int idContrato, int idVehiculo);
    Task<ExtraContratoDto> AgregarExtraAsync(int idContrato, int idExtra, int cantidad);
    Task MarcarVehiculoInspeccionadoAsync(int idVehiculoContrato);
    Task ConfirmarContratoAsync(int idContrato);
    Task IniciarContratoAsync(int idContrato);
}
