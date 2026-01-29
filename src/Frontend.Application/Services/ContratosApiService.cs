using Frontend.Application.Configuration;
using Frontend.Application.DTOs;
using Frontend.Application.Interfaces;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace Frontend.Application.Services;

public class ContratosApiService : IContratosApiService
{
    private readonly HttpClient _httpClient;
    private readonly ApiSettings _apiSettings;
    private readonly IJwtTokenService _tokenService;

    public ContratosApiService(HttpClient httpClient, IOptions<ApiSettings> apiSettings, IJwtTokenService tokenService)
    {
        _httpClient = httpClient;
        _apiSettings = apiSettings.Value;
        _tokenService = tokenService;
    }

    private async Task ConfigurarAutenticacionAsync()
    {
        var token = await _tokenService.ObtenerTokenContratoApiAsync();
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }

    public async Task<List<ContratoDto>> ListarAsync(int? idCliente = null, int? idEstado = null, DateTime? fechaInicio = null, DateTime? fechaFin = null)
    {
        await ConfigurarAutenticacionAsync();
        
        var query = new List<string>();
        if (idCliente.HasValue) query.Add($"idCliente={idCliente}");
        if (idEstado.HasValue) query.Add($"idEstado={idEstado}");
        if (fechaInicio.HasValue) query.Add($"fechaInicio={fechaInicio:yyyy-MM-dd}");
        if (fechaFin.HasValue) query.Add($"fechaFin={fechaFin:yyyy-MM-dd}");
        
        var queryString = query.Count > 0 ? "?" + string.Join("&", query) : "";
        var response = await _httpClient.GetFromJsonAsync<List<ContratoDto>>($"{_apiSettings.ContratoApiBaseUrl}/api/Contratos{queryString}");
        return response ?? new List<ContratoDto>();
    }

    public async Task<ContratoDto?> ObtenerPorIdAsync(int id)
    {
        await ConfigurarAutenticacionAsync();
        return await _httpClient.GetFromJsonAsync<ContratoDto>($"{_apiSettings.ContratoApiBaseUrl}/api/Contratos/{id}");
    }

    public async Task<ContratoDetalleDto?> ObtenerDetalleAsync(int id)
    {
        await ConfigurarAutenticacionAsync();
        return await _httpClient.GetFromJsonAsync<ContratoDetalleDto>($"{_apiSettings.ContratoApiBaseUrl}/api/Contratos/{id}/detalle");
    }

    public async Task<ContratoDto> CrearAsync(ContratoDto contrato)
    {
        await ConfigurarAutenticacionAsync();
        var response = await _httpClient.PostAsJsonAsync($"{_apiSettings.ContratoApiBaseUrl}/api/Contratos", contrato);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<ContratoDto>() ?? throw new InvalidOperationException("Error al crear contrato");
    }

    public async Task<ContratoDto> ActualizarAsync(int id, ContratoDto contrato)
    {
        await ConfigurarAutenticacionAsync();
        var response = await _httpClient.PutAsJsonAsync($"{_apiSettings.ContratoApiBaseUrl}/api/Contratos/{id}", contrato);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<ContratoDto>() ?? throw new InvalidOperationException("Error al actualizar contrato");
    }

    public async Task<VehiculoContratoDto> AgregarVehiculoAsync(int idContrato, int idVehiculo, string descripcionVehiculo, int diasDeUso, decimal costoDiario)
    {
        await ConfigurarAutenticacionAsync();
        var request = new 
        { 
            IdContrato = idContrato,
            IdVehiculo = idVehiculo, 
            DescripcionVehiculo = descripcionVehiculo,
            DiasDeUso = diasDeUso,
            CostoDiario = costoDiario
        };
        var response = await _httpClient.PostAsJsonAsync($"{_apiSettings.ContratoApiBaseUrl}/api/Contratos/{idContrato}/vehiculos", request);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<VehiculoContratoDto>() ?? throw new InvalidOperationException("Error al agregar vehículo");
    }

    public async Task<ExtraContratoDto> AgregarExtraAsync(int idContrato, int idExtra, string descripcionExtra, int diasDeUso, decimal costoDiario)
    {
        await ConfigurarAutenticacionAsync();
        var request = new 
        { 
            IdContrato = idContrato,
            IdExtra = idExtra, 
            DescripcionExtra = descripcionExtra,
            DiasDeUso = diasDeUso,
            CostoDiario = costoDiario
        };
        var response = await _httpClient.PostAsJsonAsync($"{_apiSettings.ContratoApiBaseUrl}/api/Contratos/{idContrato}/extras", request);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<ExtraContratoDto>() ?? throw new InvalidOperationException("Error al agregar extra");
    }

    public async Task MarcarVehiculoInspeccionadoAsync(int idVehiculoContrato, int idUsuario)
    {
        await ConfigurarAutenticacionAsync();
        var request = new { IdVehiculoContrato = idVehiculoContrato, IdUsuario = idUsuario };
        var response = await _httpClient.PostAsJsonAsync($"{_apiSettings.ContratoApiBaseUrl}/api/Contratos/vehiculos/{idVehiculoContrato}/marcar-inspeccionado", request);
        response.EnsureSuccessStatusCode();
    }

    public async Task ConfirmarContratoAsync(int idContrato, int idUsuario)
    {
        await ConfigurarAutenticacionAsync();
        var request = new { IdContrato = idContrato, IdUsuario = idUsuario };
        var response = await _httpClient.PostAsJsonAsync($"{_apiSettings.ContratoApiBaseUrl}/api/Contratos/{idContrato}/confirmar", request);
        response.EnsureSuccessStatusCode();
    }

    public async Task IniciarContratoAsync(int idContrato, int idUsuario)
    {
        await ConfigurarAutenticacionAsync();
        var request = new { IdContrato = idContrato, IdUsuario = idUsuario };
        var response = await _httpClient.PostAsJsonAsync($"{_apiSettings.ContratoApiBaseUrl}/api/Contratos/{idContrato}/iniciar", request);
        response.EnsureSuccessStatusCode();
    }
}
