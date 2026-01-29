using Frontend.Application.DTOs;
using Frontend.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Frontend.Web.Controllers;

public class ContratosController : Controller
{
    private readonly IContratosApiService _contratosService;
    private readonly IClientesApiService _clientesService;
    private readonly IVehiculosApiService _vehiculosService;
    private readonly IExtrasApiService _extrasService;
    private readonly IUsuariosApiService _usuariosService;
    private readonly ISucursalesApiService _sucursalesService;
    private readonly IEstadosContratoApiService _estadosService;
    private readonly ILogger<ContratosController> _logger;

    public ContratosController(
        IContratosApiService contratosService,
        IClientesApiService clientesService,
        IVehiculosApiService vehiculosService,
        IExtrasApiService extrasService,
        IUsuariosApiService usuariosService,
        ISucursalesApiService sucursalesService,
        IEstadosContratoApiService estadosService,
        ILogger<ContratosController> logger)
    {
        _contratosService = contratosService;
        _clientesService = clientesService;
        _vehiculosService = vehiculosService;
        _extrasService = extrasService;
        _usuariosService = usuariosService;
        _sucursalesService = sucursalesService;
        _estadosService = estadosService;
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> Listar(int? idCliente, int? idEstado, DateTime? fechaInicio, DateTime? fechaFin)
    {
        try
        {
            var contratos = await _contratosService.ListarAsync(idCliente, idEstado, fechaInicio, fechaFin);
            return Json(new { success = true, data = contratos });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al listar contratos");
            return Json(new { success = false, message = "Error al cargar contratos: " + ex.Message });
        }
    }

    [HttpGet]
    public async Task<IActionResult> Obtener(int id)
    {
        try
        {
            var contrato = await _contratosService.ObtenerPorIdAsync(id);
            if (contrato == null)
                return Json(new { success = false, message = "Contrato no encontrado" });

            return Json(new { success = true, data = contrato });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener contrato {Id}", id);
            return Json(new { success = false, message = "Error al obtener contrato: " + ex.Message });
        }
    }

    [HttpGet]
    public async Task<IActionResult> ObtenerDetalle(int id)
    {
        try
        {
            var detalle = await _contratosService.ObtenerDetalleAsync(id);
            if (detalle == null)
                return Json(new { success = false, message = "Contrato no encontrado" });

            return Json(new { success = true, data = detalle });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener detalle del contrato {Id}", id);
            return Json(new { success = false, message = "Error al obtener detalle: " + ex.Message });
        }
    }

    [HttpPost]
    public async Task<IActionResult> Crear([FromBody] ContratoDto contrato)
    {
        try
        {
            var nuevoContrato = await _contratosService.CrearAsync(contrato);
            return Json(new { success = true, data = nuevoContrato });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al crear contrato");
            return Json(new { success = false, message = "Error al crear contrato: " + ex.Message });
        }
    }

    [HttpPut]
    public async Task<IActionResult> Actualizar(int id, [FromBody] ContratoDto contrato)
    {
        try
        {
            var contratoActualizado = await _contratosService.ActualizarAsync(id, contrato);
            return Json(new { success = true, data = contratoActualizado });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al actualizar contrato {Id}", id);
            return Json(new { success = false, message = "Error al actualizar contrato: " + ex.Message });
        }
    }

    [HttpPost]
    public async Task<IActionResult> AgregarVehiculo(int idContrato, int idVehiculo, string descripcionVehiculo, int diasDeUso, decimal costoDiario)
    {
        try
        {
            var vehiculoContrato = await _contratosService.AgregarVehiculoAsync(idContrato, idVehiculo, descripcionVehiculo, diasDeUso, costoDiario);
            return Json(new { success = true, data = vehiculoContrato });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al agregar vehículo al contrato {IdContrato}", idContrato);
            return Json(new { success = false, message = "Error al agregar vehículo: " + ex.Message });
        }
    }

    [HttpPost]
    public async Task<IActionResult> AgregarExtra(int idContrato, int idExtra, string descripcionExtra, int diasDeUso, decimal costoDiario)
    {
        try
        {
            var extraContrato = await _contratosService.AgregarExtraAsync(idContrato, idExtra, descripcionExtra, diasDeUso, costoDiario);
            return Json(new { success = true, data = extraContrato });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al agregar extra al contrato {IdContrato}", idContrato);
            return Json(new { success = false, message = "Error al agregar extra: " + ex.Message });
        }
    }

    [HttpPut]
    public async Task<IActionResult> MarcarVehiculoInspeccionado(int idVehiculoContrato, int idUsuario)
    {
        try
        {
            await _contratosService.MarcarVehiculoInspeccionadoAsync(idVehiculoContrato, idUsuario);
            return Json(new { success = true });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al marcar vehículo como inspeccionado {IdVehiculoContrato}", idVehiculoContrato);
            return Json(new { success = false, message = "Error al marcar vehículo como inspeccionado: " + ex.Message });
        }
    }

    [HttpPut]
    public async Task<IActionResult> Confirmar(int id, int idUsuario)
    {
        try
        {
            await _contratosService.ConfirmarContratoAsync(id, idUsuario);
            return Json(new { success = true });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al confirmar contrato {Id}", id);
            return Json(new { success = false, message = "Error al confirmar contrato: " + ex.Message });
        }
    }

    [HttpPut]
    public async Task<IActionResult> Iniciar(int id, int idUsuario)
    {
        try
        {
            await _contratosService.IniciarContratoAsync(id, idUsuario);
            return Json(new { success = true });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al iniciar contrato {Id}", id);
            return Json(new { success = false, message = "Error al iniciar contrato: " + ex.Message });
        }
    }

    [HttpGet]
    public async Task<IActionResult> ObtenerClientes()
    {
        try
        {
            var clientes = await _clientesService.ListarActivosAsync();
            return Json(new { success = true, data = clientes });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener clientes");
            return Json(new { success = false, message = "Error al cargar clientes: " + ex.Message });
        }
    }

    [HttpGet]
    public async Task<IActionResult> ObtenerVehiculos()
    {
        try
        {
            var vehiculos = await _vehiculosService.ListarDisponiblesAsync();
            return Json(new { success = true, data = vehiculos });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener vehículos");
            return Json(new { success = false, message = "Error al cargar vehículos: " + ex.Message });
        }
    }

    [HttpGet]
    public async Task<IActionResult> ObtenerExtras()
    {
        try
        {
            var extras = await _extrasService.ListarActivosAsync();
            return Json(new { success = true, data = extras });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener extras");
            return Json(new { success = false, message = "Error al cargar extras: " + ex.Message });
        }
    }

    [HttpGet]
    public async Task<IActionResult> ObtenerUsuarios()
    {
        try
        {
            var usuarios = await _usuariosService.ListarActivosAsync();
            return Json(new { success = true, data = usuarios });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener usuarios");
            return Json(new { success = false, message = "Error al cargar usuarios: " + ex.Message });
        }
    }

    [HttpGet]
    public async Task<IActionResult> ObtenerSucursales()
    {
        try
        {
            var sucursales = await _sucursalesService.ListarActivasAsync();
            return Json(new { success = true, data = sucursales });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener sucursales");
            return Json(new { success = false, message = "Error al cargar sucursales: " + ex.Message });
        }
    }

    [HttpGet]
    public async Task<IActionResult> ObtenerEstados()
    {
        try
        {
            var estados = await _estadosService.ListarActivosAsync();
            return Json(new { success = true, data = estados });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener estados");
            return Json(new { success = false, message = "Error al cargar estados: " + ex.Message });
        }
    }
}
