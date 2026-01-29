using Frontend.Application.DTOs;

namespace Frontend.Application.Interfaces;

public interface IExtrasApiService
{
    Task<List<ExtraDto>> ListarActivosAsync();
}
