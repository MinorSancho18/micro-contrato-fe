# 📁 Estructura del Proyecto - Micro Contrato FE

## 🏗️ Arquitectura Clean Architecture

Este proyecto sigue el patrón **Clean Architecture** con 4 capas claramente separadas:

```
Frontend.sln
├── .gitignore
├── README.md
├── PROJECT_STRUCTURE.md
├── SECURITY.md
├── src/
│   ├── Frontend.Domain/
│   │   └── Frontend.Domain.csproj
│   │
│   ├── Frontend.Application/
│   │   ├── Configuration/
│   │   │   └── ApiSettings.cs                  # Configuración de URLs y tokens de APIs
│   │   ├── DTOs/
│   │   │   ├── ContratoDto.cs                  # DTO del contrato maestro
│   │   │   ├── ContratoDetalleDto.cs           # DTO maestro-detalle completo
│   │   │   ├── VehiculoContratoDto.cs          # DTO de vehículos del contrato
│   │   │   ├── ExtraContratoDto.cs             # DTO de extras del contrato
│   │   │   ├── ClienteDto.cs                   # DTO de cliente (micro-catalogos)
│   │   │   ├── VehiculoDto.cs                  # DTO de vehículo (micro-catalogos)
│   │   │   ├── ExtraDto.cs                     # DTO de extra (micro-catalogos)
│   │   │   ├── UsuarioDto.cs                   # DTO de usuario interno
│   │   │   ├── SucursalDto.cs                  # DTO de sucursal interna
│   │   │   ├── EstadoContratoDto.cs            # DTO de estado de contrato
│   │   │   └── TokenResponse.cs                # DTO de respuesta JWT
│   │   ├── Interfaces/
│   │   │   ├── IJwtTokenService.cs             # Servicio de autenticación JWT
│   │   │   ├── IContratosApiService.cs         # Servicio de contratos (CRUD completo)
│   │   │   ├── IClientesApiService.cs          # Servicio de clientes externos
│   │   │   ├── IVehiculosApiService.cs         # Servicio de vehículos externos
│   │   │   ├── IExtrasApiService.cs            # Servicio de extras externos
│   │   │   ├── IUsuariosApiService.cs          # Servicio de usuarios internos
│   │   │   ├── ISucursalesApiService.cs        # Servicio de sucursales internas
│   │   │   └── IEstadosContratoApiService.cs   # Servicio de estados internos
│   │   ├── Services/
│   │   │   ├── JwtTokenService.cs              # Implementación JWT con caché
│   │   │   ├── ContratosApiService.cs          # Implementación API contratos
│   │   │   ├── ClientesApiService.cs           # Implementación API clientes
│   │   │   ├── VehiculosApiService.cs          # Implementación API vehículos
│   │   │   ├── ExtrasApiService.cs             # Implementación API extras
│   │   │   ├── UsuariosApiService.cs           # Implementación API usuarios
│   │   │   ├── SucursalesApiService.cs         # Implementación API sucursales
│   │   │   └── EstadosContratoApiService.cs    # Implementación API estados
│   │   └── Frontend.Application.csproj
│   │
│   ├── Frontend.Infrastructure/
│   │   ├── DependencyInjection.cs              # Registro de servicios en DI container
│   │   └── Frontend.Infrastructure.csproj
│   │
│   └── Frontend.Web/
│       ├── Controllers/
│       │   ├── HomeController.cs               # Controlador página principal
│       │   └── ContratosController.cs          # Controlador CRUD contratos + JSON API
│       ├── Views/
│       │   ├── Shared/
│       │   │   ├── _Layout.cshtml              # Layout principal con navbar
│       │   │   ├── Error.cshtml                # Vista de error
│       │   │   └── _ValidationScriptsPartial.cshtml
│       │   ├── Home/
│       │   │   └── Index.cshtml                # Página de inicio
│       │   ├── Contratos/
│       │   │   └── Index.cshtml                # Vista maestro-detalle con modales
│       │   ├── _ViewStart.cshtml               # Configuración de layout por defecto
│       │   └── _ViewImports.cshtml             # Imports globales de namespaces
│       ├── wwwroot/
│       │   ├── css/
│       │   │   └── site.css                    # Estilos personalizados
│       │   ├── js/
│       │   │   ├── site.js                     # JavaScript general
│       │   │   └── contratos.js                # JavaScript CRUD contratos (1200+ líneas)
│       │   └── lib/                            # Librerías (se usa CDN)
│       ├── Models/
│       │   └── ErrorViewModel.cs               # ViewModel para errores
│       ├── Program.cs                          # Punto de entrada y configuración
│       ├── appsettings.json                    # Configuración producción
│       ├── appsettings.Development.json        # Configuración desarrollo
│       └── Frontend.Web.csproj
│
└── tests/
    ├── Frontend.Application.Tests/
    │   └── Frontend.Application.Tests.csproj   # Tests de capa de aplicación
    ├── Frontend.Web.Tests/
    │   └── Frontend.Web.Tests.csproj           # Tests de capa web
    └── Frontend.Infrastructure.Tests/
        └── Frontend.Infrastructure.Tests.csproj # Tests de infraestructura
```

---

## 📊 Dependencias entre Capas

```
┌─────────────────────┐
│   Frontend.Web      │  ← Capa de Presentación (MVC)
└──────────┬──────────┘
           │
           ↓
┌─────────────────────┐
│ Frontend.Infra-     │  ← Registro de Dependencias
│ structure           │
└──────────┬──────────┘
           │
           ↓
┌─────────────────────┐
│ Frontend.Application│  ← Lógica de Negocio + DTOs + Servicios
└──────────┬──────────┘
           │
           ↓
┌─────────────────────┐
│  Frontend.Domain    │  ← Entidades de Dominio (actualmente vacío)
└─────────────────────┘
```

---

## 🔌 Integración con APIs Externas

### API 1: micro-contrato (Principal)
- **Base URL:** https://localhost:7002
- **Autenticación:** JWT Bearer Token
- **Endpoints:** Contratos, Vehículos Contrato, Extras Contrato, Usuarios, Sucursales, Estados

### API 2: micro-catalogos (Catálogos)
- **Base URL:** https://localhost:7001
- **Autenticación:** JWT Bearer Token
- **Endpoints:** Clientes, Vehículos, Categorías, Extras

---

## 🎨 Frontend Stack

### Backend (.NET 8)
- **Framework:** ASP.NET Core MVC 8.0
- **Patrón:** Clean Architecture
- **DI:** Microsoft.Extensions.DependencyInjection
- **HTTP:** HttpClient con IHttpClientFactory
- **Config:** IOptions<ApiSettings>

### Frontend (UI)
- **CSS Framework:** Bootstrap 5.3.2 (CDN)
- **JavaScript:** jQuery 3.7.1 (CDN)
- **Tablas:** DataTables 1.13.7 con Bootstrap 5 theme (CDN)
- **Alertas:** SweetAlert2 11.x (CDN)
- **Iconos:** Bootstrap Icons 1.11.2 (CDN)
- **Fechas:** Bootstrap DatePicker (CDN)

### Testing
- **Framework:** xUnit 2.6.2
- **Test SDK:** Microsoft.NET.Test.Sdk 17.8.0

---

## 📝 Convenciones de Código

### Naming
- **Variables/Métodos:** Español (camelCase)
- **Clases/Interfaces:** Español (PascalCase)
- **DTOs:** Sufijo "Dto"
- **Interfaces:** Prefijo "I"
- **Servicios:** Sufijo "Service" o "ApiService"

### Archivos
- **Controllers:** Sufijo "Controller.cs"
- **Services:** Sufijo "Service.cs"
- **DTOs:** Sufijo "Dto.cs"
- **Interfaces:** Prefijo "I" + nombre del servicio

---

## 🚀 Comandos Útiles

### Build
```bash
dotnet build Frontend.sln
```

### Ejecutar
```bash
cd src/Frontend.Web
dotnet run
```

### Tests
```bash
dotnet test Frontend.sln
```

### Restore
```bash
dotnet restore Frontend.sln
```

---

## 📦 Paquetes NuGet por Proyecto

### Frontend.Domain
- Ninguno (proyecto base)

### Frontend.Application
- Microsoft.Extensions.Options.ConfigurationExtensions (8.0.0)

### Frontend.Infrastructure
- Microsoft.Extensions.DependencyInjection.Abstractions (8.0.0)
- Microsoft.Extensions.Http (8.0.0)

### Frontend.Web
- Microsoft.Extensions.Http (8.0.0)

### Tests (todos)
- Microsoft.NET.Test.Sdk (17.8.0)
- xunit (2.6.2)
- xunit.runner.visualstudio (2.5.4)

---

## 🔐 Seguridad

Ver [SECURITY.md](SECURITY.md) para detalles sobre:
- Gestión de tokens JWT
- Manejo de credenciales
- HTTPS obligatorio
- Validación de entrada
- Protección CSRF

---

## 📄 Licencia

Este proyecto es parte del ecosistema de microservicios PROCOMER 2024.

---

**Última actualización:** Enero 2026
