using Frontend.Application.Interfaces;
using Frontend.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Frontend.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddHttpClient<IJwtTokenService, JwtTokenService>();
        services.AddHttpClient<IContratosApiService, ContratosApiService>();
        services.AddHttpClient<IClientesApiService, ClientesApiService>();
        services.AddHttpClient<IVehiculosApiService, VehiculosApiService>();
        services.AddHttpClient<IExtrasApiService, ExtrasApiService>();
        services.AddHttpClient<IUsuariosApiService, UsuariosApiService>();
        services.AddHttpClient<ISucursalesApiService, SucursalesApiService>();
        services.AddHttpClient<IEstadosContratoApiService, EstadosContratoApiService>();

        return services;
    }
}
