using Frontend.Application.DTOs;

namespace Frontend.Application.Interfaces;

public interface IVehiculosApiService
{
    Task<List<VehiculoDto>> ListarDisponiblesAsync();
}
