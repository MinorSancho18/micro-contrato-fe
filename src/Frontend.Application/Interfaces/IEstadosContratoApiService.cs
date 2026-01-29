using Frontend.Application.DTOs;

namespace Frontend.Application.Interfaces;

public interface IEstadosContratoApiService
{
    Task<List<EstadoContratoDto>> ListarActivosAsync();
}
