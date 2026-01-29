using Frontend.Application.DTOs;

namespace Frontend.Application.Interfaces;

public interface IUsuariosApiService
{
    Task<List<UsuarioDto>> ListarActivosAsync();
}
