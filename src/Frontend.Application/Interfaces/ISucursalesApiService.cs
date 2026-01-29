using Frontend.Application.DTOs;

namespace Frontend.Application.Interfaces;

public interface ISucursalesApiService
{
    Task<List<SucursalDto>> ListarActivasAsync();
}
