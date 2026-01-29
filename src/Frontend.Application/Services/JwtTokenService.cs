using Frontend.Application.Configuration;
using Frontend.Application.DTOs;
using Frontend.Application.Interfaces;
using Microsoft.Extensions.Options;
using System.Net.Http.Json;

namespace Frontend.Application.Services;

public class JwtTokenService : IJwtTokenService
{
    private readonly HttpClient _httpClient;
    private readonly ApiSettings _apiSettings;
    private TokenResponse? _contratoToken;
    private TokenResponse? _catalogosToken;

    public JwtTokenService(HttpClient httpClient, IOptions<ApiSettings> apiSettings)
    {
        _httpClient = httpClient;
        _apiSettings = apiSettings.Value;
    }

    public async Task<string> ObtenerTokenContratoApiAsync()
    {
        if (_contratoToken != null && _contratoToken.ExpiresAt > DateTime.UtcNow.AddMinutes(5))
        {
            return _contratoToken.Token;
        }

        var request = new { AuthCode = _apiSettings.ContratoAuthCode };
        var response = await _httpClient.PostAsJsonAsync($"{_apiSettings.ContratoApiBaseUrl}/api/Auth/token", request);
        response.EnsureSuccessStatusCode();

        _contratoToken = await response.Content.ReadFromJsonAsync<TokenResponse>();
        return _contratoToken?.Token ?? throw new InvalidOperationException("No se pudo obtener el token");
    }

    public async Task<string> ObtenerTokenCatalogosApiAsync()
    {
        if (_catalogosToken != null && _catalogosToken.ExpiresAt > DateTime.UtcNow.AddMinutes(5))
        {
            return _catalogosToken.Token;
        }

        var request = new { AuthCode = _apiSettings.CatalogosAuthCode };
        var response = await _httpClient.PostAsJsonAsync($"{_apiSettings.CatalogosApiBaseUrl}/api/Auth/token", request);
        response.EnsureSuccessStatusCode();

        _catalogosToken = await response.Content.ReadFromJsonAsync<TokenResponse>();
        return _catalogosToken?.Token ?? throw new InvalidOperationException("No se pudo obtener el token");
    }
}
