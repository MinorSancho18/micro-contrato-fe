# Sistema de Gestión de Contratos - Frontend

Frontend ASP.NET Core MVC (.NET 8) para el sistema de gestión de contratos de alquiler de vehículos, implementado siguiendo Clean Architecture.

## 🏗️ Arquitectura

El proyecto sigue Clean Architecture con 4 capas:

```
├── Frontend.Domain          # Capa de dominio (entidades base)
├── Frontend.Application     # Lógica de aplicación (DTOs, Interfaces, Services)
├── Frontend.Infrastructure  # Implementación de infraestructura (DI)
└── Frontend.Web            # Presentación (MVC, Views, Controllers)
```

## 📋 Características

- ✅ **Clean Architecture** - Separación clara de responsabilidades
- ✅ **JWT Authentication** - Token caching para ambos APIs
- ✅ **Gestión Completa de Contratos** - CRUD con master-detail
- ✅ **UI Moderna** - Bootstrap 5.3.2, jQuery 3.7.1, DataTables 1.13.7
- ✅ **SweetAlert2** - Alertas y confirmaciones elegantes
- ✅ **Responsive Design** - Adaptable a diferentes dispositivos
- ✅ **Estado del Contrato** - Lógica basada en estados (Pendiente, Confirmado, En Progreso)

## 🚀 Tecnologías

- **.NET 8.0** - Framework principal
- **ASP.NET Core MVC** - Patrón MVC
- **Bootstrap 5.3.2** - Framework CSS
- **jQuery 3.7.1** - Manipulación DOM y AJAX
- **DataTables 1.13.7** - Tablas interactivas
- **SweetAlert2 11.x** - Alertas modales
- **xUnit** - Framework de pruebas

## 📁 Estructura del Proyecto

### Frontend.Application
```
├── Configuration/
│   └── ApiSettings.cs                 # Configuración de APIs
├── DTOs/
│   ├── ContratoDto.cs                 # DTO de contrato
│   ├── ContratoDetalleDto.cs          # DTO detalle con vehículos y extras
│   ├── VehiculoContratoDto.cs         # DTO vehículo en contrato
│   ├── ExtraContratoDto.cs            # DTO extra en contrato
│   └── ...                            # Otros DTOs (Cliente, Vehiculo, etc.)
├── Interfaces/
│   ├── IJwtTokenService.cs            # Servicio de tokens JWT
│   ├── IContratosApiService.cs        # Servicio de contratos
│   └── ...                            # Otras interfaces
└── Services/
    ├── JwtTokenService.cs             # Implementación JWT con cache
    ├── ContratosApiService.cs         # Implementación API contratos
    └── ...                            # Otras implementaciones
```

### Frontend.Web
```
├── Controllers/
│   ├── HomeController.cs              # Controlador principal
│   └── ContratosController.cs         # Controlador de contratos
├── Views/
│   ├── Shared/
│   │   └── _Layout.cshtml             # Layout Bootstrap 5
│   ├── Home/
│   │   └── Index.cshtml               # Página de inicio
│   └── Contratos/
│       └── Index.cshtml               # Vista principal con modales
└── wwwroot/
    ├── css/
    │   └── site.css                   # Estilos personalizados
    └── js/
        ├── site.js                    # JavaScript general
        └── contratos.js               # Lógica de contratos
```

## ⚙️ Configuración

### appsettings.json

```json
{
  "ApiSettings": {
    "ContratoApiBaseUrl": "https://localhost:7002",
    "CatalogosApiBaseUrl": "https://localhost:7001",
    "ContratoAuthCode": "PROCOMER-2024-SECURE-API-TOKEN-XYZ123",
    "CatalogosAuthCode": "PROCOMER-2024-SECURE-API-TOKEN-XYZ123"
  }
}
```

## 🔧 Instalación y Ejecución

### Requisitos Previos

- .NET 8 SDK
- Visual Studio 2022 / VS Code / Rider
- APIs backend funcionando (micro-contrato-api, micro-catalogos-api)

### Pasos

1. **Clonar el repositorio**
   ```bash
   git clone https://github.com/MinorSancho18/micro-contrato-fe.git
   cd micro-contrato-fe
   ```

2. **Restaurar paquetes**
   ```bash
   dotnet restore
   ```

3. **Compilar la solución**
   ```bash
   dotnet build
   ```

4. **Configurar las URLs de las APIs**
   - Editar `src/Frontend.Web/appsettings.json`
   - Actualizar `ContratoApiBaseUrl` y `CatalogosApiBaseUrl`

5. **Ejecutar la aplicación**
   ```bash
   cd src/Frontend.Web
   dotnet run
   ```

6. **Abrir en el navegador**
   - Navegar a `https://localhost:5001` o `http://localhost:5000`

## 📝 Funcionalidades Principales

### Gestión de Contratos

- **Listar Contratos** - DataTable con paginación, búsqueda y ordenamiento
- **Crear Contrato** - Modal con validación de formularios
- **Editar Contrato** - Modificación de contratos no confirmados
- **Ver Detalle** - Modal con información completa del contrato
- **Agregar Vehículos** - Asociar vehículos disponibles al contrato
- **Agregar Extras** - Agregar extras con cantidad
- **Inspeccionar Vehículos** - Marcar vehículos como inspeccionados
- **Confirmar Contrato** - Confirmar cuando todos los vehículos están inspeccionados
- **Iniciar Contrato** - Cambiar estado a "En Progreso"

### Reglas de Negocio

- ✅ Solo se pueden agregar vehículos/extras si el contrato no está confirmado
- ✅ Solo se puede confirmar si todos los vehículos están inspeccionados
- ✅ Solo se puede iniciar un contrato confirmado en estado "Pendiente"
- ✅ Cálculo automático de subtotales y totales
- ✅ Validación de fechas (devolución debe ser posterior a recogida)

## 🔐 Autenticación

El sistema implementa autenticación JWT con las siguientes características:

- **Token Caching** - Los tokens se cachean y reutilizan hasta 5 minutos antes de expirar
- **Dos APIs** - Manejo separado de tokens para API de Contratos y Catálogos
- **Auto-renovación** - Los tokens se renuevan automáticamente cuando expiran

## 🎨 UI/UX

### Modales

1. **Modal Crear/Editar Contrato** - Formulario completo con combos
2. **Modal Detalle** - Vista detallada con tablas de vehículos y extras
3. **Modal Agregar Vehículo** - Selector de vehículos disponibles
4. **Modal Agregar Extra** - Selector de extras con cantidad

### Características

- Diseño responsive
- DataTables en español
- Validación en cliente y servidor
- Alertas con SweetAlert2
- Botones deshabilitados según estado del contrato

## 🧪 Pruebas

El proyecto incluye proyectos de pruebas vacíos preparados para xUnit:

- `Frontend.Application.Tests` - Pruebas de lógica de aplicación
- `Frontend.Infrastructure.Tests` - Pruebas de infraestructura
- `Frontend.Web.Tests` - Pruebas de controladores y vistas

Para ejecutar las pruebas:
```bash
dotnet test
```

## 📦 Dependencias Principales

### Paquetes NuGet

- `Microsoft.Extensions.Http` (8.0.0)
- `Microsoft.Extensions.Options.ConfigurationExtensions` (8.0.0)
- `Microsoft.Extensions.DependencyInjection.Abstractions` (8.0.0)

### CDN (Frontend)

- Bootstrap 5.3.2
- jQuery 3.7.1
- DataTables 1.13.7
- SweetAlert2 11.x

## 🔄 Integración con APIs

### API de Contratos (puerto 7002)

- POST `/api/Auth/token` - Obtener token JWT
- GET `/api/Contratos` - Listar contratos
- GET `/api/Contratos/{id}` - Obtener contrato
- GET `/api/Contratos/{id}/detalle` - Obtener detalle
- POST `/api/Contratos` - Crear contrato
- PUT `/api/Contratos/{id}` - Actualizar contrato
- POST `/api/Contratos/{id}/vehiculos` - Agregar vehículo
- POST `/api/Contratos/{id}/extras` - Agregar extra
- PUT `/api/Contratos/vehiculos/{id}/inspeccionar` - Marcar inspeccionado
- PUT `/api/Contratos/{id}/confirmar` - Confirmar contrato
- PUT `/api/Contratos/{id}/iniciar` - Iniciar contrato

### API de Catálogos (puerto 7001)

- POST `/api/Auth/token` - Obtener token JWT
- GET `/api/Clientes` - Listar clientes activos
- GET `/api/Vehiculos/disponibles` - Listar vehículos disponibles
- GET `/api/Extras` - Listar extras activos
- GET `/api/Usuarios` - Listar usuarios activos
- GET `/api/Sucursales` - Listar sucursales activas

## 👥 Contribuir

1. Fork el proyecto
2. Crear una rama feature (`git checkout -b feature/AmazingFeature`)
3. Commit los cambios (`git commit -m 'Add some AmazingFeature'`)
4. Push a la rama (`git push origin feature/AmazingFeature`)
5. Abrir un Pull Request

## 📄 Licencia

Este proyecto está bajo la Licencia MIT.

## 👤 Autor

**MinorSancho18**

## 🙏 Agradecimientos

- Basado en el patrón de [micro-catalogos-fe](https://github.com/MinorSancho18/micro-catalogos-fe)
- Clean Architecture por Jason Taylor
- Bootstrap Team
- DataTables Team