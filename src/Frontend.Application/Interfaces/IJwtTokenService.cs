namespace Frontend.Application.Interfaces;

public interface IJwtTokenService
{
    Task<string> ObtenerTokenContratoApiAsync();
    Task<string> ObtenerTokenCatalogosApiAsync();
}
