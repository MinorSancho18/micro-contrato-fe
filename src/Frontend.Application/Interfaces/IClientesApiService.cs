using Frontend.Application.DTOs;

namespace Frontend.Application.Interfaces;

public interface IClientesApiService
{
    Task<List<ClienteDto>> ListarActivosAsync();
}
