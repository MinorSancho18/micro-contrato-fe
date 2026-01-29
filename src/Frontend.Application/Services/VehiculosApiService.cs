using Frontend.Application.Configuration;
using Frontend.Application.DTOs;
using Frontend.Application.Interfaces;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace Frontend.Application.Services;

public class VehiculosApiService : IVehiculosApiService
{
    private readonly HttpClient _httpClient;
    private readonly ApiSettings _apiSettings;
    private readonly IJwtTokenService _tokenService;

    public VehiculosApiService(HttpClient httpClient, IOptions<ApiSettings> apiSettings, IJwtTokenService tokenService)
    {
        _httpClient = httpClient;
        _apiSettings = apiSettings.Value;
        _tokenService = tokenService;
    }

    public async Task<List<VehiculoDto>> ListarDisponiblesAsync()
    {
        var token = await _tokenService.ObtenerTokenCatalogosApiAsync();
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        
        var response = await _httpClient.GetFromJsonAsync<List<VehiculoDto>>($"{_apiSettings.CatalogosApiBaseUrl}/api/Vehiculos/disponibles");
        return response ?? new List<VehiculoDto>();
    }
}
