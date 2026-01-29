# 🔐 Seguridad - Micro Contrato FE

## 📋 Políticas de Seguridad

Este documento describe las medidas de seguridad implementadas y las mejores prácticas que se deben seguir al trabajar con este frontend.

---

## 🛡️ Autenticación y Autorización

### JWT Token Management

#### Flujo de Autenticación
1. El frontend solicita un token JWT al backend usando el código de autenticación (`AuthCode`)
2. El backend valida el código y devuelve un token JWT
3. El frontend almacena el token en memoria (NO en localStorage ni sessionStorage)
4. Todas las peticiones subsecuentes incluyen el token en el header `Authorization: Bearer {token}`

#### Implementación Actual
```csharp
// JwtTokenService.cs
private string? _tokenContrato;    // Token en memoria
private DateTime? _tokenContratoExpiracion;

public async Task<string> ObtenerTokenContratoAsync()
{
    // Reutilizar token si es válido (caché de 5 minutos antes de expiración)
    if (!string.IsNullOrEmpty(_tokenContrato) && 
        _tokenContratoExpiracion.HasValue && 
        _tokenContratoExpiracion.Value > DateTime.UtcNow.AddMinutes(5))
    {
        return _tokenContrato;
    }
    
    // Solicitar nuevo token...
}
```

#### ✅ Buenas Prácticas Implementadas
- ✅ Tokens almacenados en memoria (no en cliente)
- ✅ Tokens con caché inteligente (renovación automática)
- ✅ Validación de expiración con buffer de 5 minutos
- ✅ Tokens separados para cada API (micro-contrato y micro-catalogos)

#### ⚠️ Recomendaciones
- 🔒 **NUNCA** almacenar tokens en localStorage o sessionStorage (vulnerable a XSS)
- 🔒 **NUNCA** exponer tokens en URLs o query strings
- 🔒 **SIEMPRE** usar HTTPS en producción
- 🔒 Implementar refresh tokens para sesiones largas (futuro)
- 🔒 Implementar logout que invalide tokens en el servidor (futuro)

---

## 🔑 Gestión de Credenciales

### Códigos de Autenticación (AuthCode)

#### Configuración Actual (appsettings.json)
```json
{
  "ApiSettings": {
    "ContratoAuthCode": "PROCOMER-2024-SECURE-API-TOKEN-XYZ123",
    "CatalogosAuthCode": "PROCOMER-2024-SECURE-API-TOKEN-XYZ123"
  }
}
```

#### ⚠️ IMPORTANTE - Producción
```json
// appsettings.Production.json (NO COMMITEAR)
{
  "ApiSettings": {
    "ContratoAuthCode": "${CONTRATO_AUTH_CODE}",    // Variable de entorno
    "CatalogosAuthCode": "${CATALOGOS_AUTH_CODE}"   // Variable de entorno
  }
}
```

#### ✅ Buenas Prácticas
- ✅ Usar variables de entorno en producción
- ✅ No commitear appsettings.Production.json con valores reales
- ✅ Rotar códigos periódicamente
- ✅ Usar Azure Key Vault o similar para secretos en producción

#### Configuración con Variables de Entorno
```bash
# Linux/Mac
export CONTRATO_AUTH_CODE="your-real-production-code"
export CATALOGOS_AUTH_CODE="your-real-production-code"

# Windows
set CONTRATO_AUTH_CODE=your-real-production-code
set CATALOGOS_AUTH_CODE=your-real-production-code

# Docker
docker run -e CONTRATO_AUTH_CODE="..." -e CATALOGOS_AUTH_CODE="..." ...
```

---

## 🌐 HTTPS y Comunicación Segura

### Requisitos de Transporte

#### ✅ Implementado
- ✅ URLs configuradas con HTTPS (`https://localhost:7001`, `https://localhost:7002`)
- ✅ Headers de seguridad en respuestas

#### ⚠️ Producción - Requerido
```csharp
// Program.cs - Agregar en producción
app.UseHsts();
app.UseHttpsRedirection();

// Configurar HSTS headers
app.UseHsts(options => options
    .MaxAge(days: 365)
    .IncludeSubdomains()
    .Preload()
);
```

#### Certificados SSL
- **Desarrollo:** Certificado auto-firmado de .NET (`dotnet dev-certs https --trust`)
- **Producción:** Certificado válido de CA reconocida (Let's Encrypt, DigiCert, etc.)

---

## 🚫 Protección CSRF (Cross-Site Request Forgery)

### Estado Actual
- El frontend es una aplicación de renderizado del lado del servidor (MVC)
- Todas las peticiones críticas (POST, PUT, DELETE) deben incluir token anti-falsificación

### ⚠️ Implementación Recomendada

#### En el Controller
```csharp
[ValidateAntiForgeryToken]  // Agregar a acciones POST/PUT/DELETE
public async Task<IActionResult> CrearContrato([FromBody] ContratoDto dto)
{
    // ...
}
```

#### En las Vistas
```html
@Html.AntiForgeryToken()
```

#### En JavaScript (AJAX)
```javascript
// Incluir token en headers
$.ajax({
    url: '/Contratos/Crear',
    type: 'POST',
    headers: {
        'RequestVerificationToken': $('input[name="__RequestVerificationToken"]').val()
    },
    data: JSON.stringify(data),
    contentType: 'application/json'
});
```

---

## 🛡️ Validación de Entrada

### Validación del Lado del Cliente
```javascript
// contratos.js
function validarFechas(fechaRecogida, fechaDevolucion) {
    if (new Date(fechaDevolucion) <= new Date(fechaRecogida)) {
        Swal.fire({
            icon: 'error',
            title: 'Error de Validación',
            text: 'La fecha de devolución debe ser posterior a la de recogida'
        });
        return false;
    }
    return true;
}
```

### ⚠️ Validación del Lado del Servidor (Recomendado)
```csharp
// ContratosController.cs
public async Task<IActionResult> Crear([FromBody] ContratoDto dto)
{
    // SIEMPRE validar en servidor
    if (dto.FechaDevolucion <= dto.FechaRecogida)
    {
        return BadRequest("Fecha de devolución inválida");
    }
    
    // Validar rangos
    if (dto.IdCliente <= 0 || dto.IdSucursal <= 0)
    {
        return BadRequest("IDs inválidos");
    }
    
    // ...
}
```

### ✅ Reglas de Validación
- ✅ Validar tipos de datos (int, decimal, DateTime)
- ✅ Validar rangos (IDs > 0, montos >= 0)
- ✅ Validar fechas (no en pasado, orden lógico)
- ✅ Sanitizar entradas de texto (prevenir XSS)
- ⚠️ Implementar Data Annotations en DTOs (futuro)

---

## 🔍 Manejo de Errores Seguro

### ⚠️ NO Exponer Detalles Internos

#### ❌ MAL - Expone información sensible
```csharp
catch (Exception ex)
{
    return StatusCode(500, ex.Message);  // Puede exponer rutas, DB, etc.
}
```

#### ✅ BIEN - Mensaje genérico
```csharp
catch (Exception ex)
{
    _logger.LogError(ex, "Error al crear contrato");
    return StatusCode(500, "Error interno del servidor");
}
```

### Logging Seguro
```csharp
// NO loguear información sensible
_logger.LogInformation("Usuario {UserId} creó contrato", userId);  // ✅ OK

_logger.LogInformation("Token: {Token}", token);  // ❌ MAL - nunca loguear tokens
_logger.LogInformation("Password: {Password}", password);  // ❌ MAL
```

---

## 🔒 Headers de Seguridad

### ⚠️ Implementación Recomendada (Program.cs)
```csharp
app.Use(async (context, next) =>
{
    // Prevenir clickjacking
    context.Response.Headers.Add("X-Frame-Options", "DENY");
    
    // Prevenir MIME sniffing
    context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
    
    // XSS Protection
    context.Response.Headers.Add("X-XSS-Protection", "1; mode=block");
    
    // Content Security Policy
    context.Response.Headers.Add("Content-Security-Policy", 
        "default-src 'self'; script-src 'self' 'unsafe-inline' cdn.jsdelivr.net; style-src 'self' 'unsafe-inline' cdn.jsdelivr.net;");
    
    await next();
});
```

---

## 🚨 Vulnerabilidades Comunes a Evitar

### 1. XSS (Cross-Site Scripting)
- ✅ Razor automáticamente escapa HTML (`@Model.Nombre`)
- ⚠️ Usar `@Html.Raw()` SOLO con contenido confiable
- ✅ Validar y sanitizar entradas en JavaScript

### 2. SQL Injection
- ✅ N/A - Este frontend NO accede directamente a BD
- ✅ El backend (micro-contrato) debe usar consultas parametrizadas

### 3. IDOR (Insecure Direct Object Reference)
- ⚠️ Validar permisos en el backend antes de modificar recursos
- ⚠️ No confiar en IDs del cliente

### 4. Sensitive Data Exposure
- ✅ No exponer tokens en logs
- ✅ No exponer códigos de autenticación en respuestas
- ⚠️ Usar HTTPS para todo tráfico en producción

### 5. Security Misconfiguration
- ⚠️ Deshabilitar mensajes de error detallados en producción
- ⚠️ Eliminar headers que revelen versiones (Server, X-Powered-By)

---

## 📦 Dependencias y Actualizaciones

### Gestión de Paquetes NuGet
```bash
# Auditoría de vulnerabilidades
dotnet list package --vulnerable

# Actualizar paquetes
dotnet add package <nombre> --version <version>
```

### ✅ Política de Actualizaciones
- ✅ Revisar vulnerabilidades mensualmente
- ✅ Actualizar a versiones de parche automáticamente
- ⚠️ Probar actualizaciones mayores en ambiente de pruebas

---

## 🔐 Checklist de Seguridad Pre-Producción

Antes de desplegar a producción, verificar:

- [ ] Códigos de autenticación en variables de entorno
- [ ] HTTPS habilitado y forzado
- [ ] Certificado SSL válido instalado
- [ ] Tokens anti-falsificación implementados
- [ ] Validación de entrada en servidor
- [ ] Headers de seguridad configurados
- [ ] Logging sin información sensible
- [ ] Mensajes de error genéricos
- [ ] Paquetes NuGet actualizados
- [ ] Auditoría de vulnerabilidades realizada
- [ ] Configuración de CORS restrictiva
- [ ] Rate limiting implementado (opcional)
- [ ] Monitoreo y alertas configurados

---

## 📞 Reporte de Vulnerabilidades

Si descubres una vulnerabilidad de seguridad, por favor:

1. **NO** abras un issue público
2. Envía un email a: security@procomer.com
3. Incluye descripción detallada y pasos para reproducir
4. Espera confirmación antes de divulgar públicamente

---

## 📚 Recursos Adicionales

- [OWASP Top 10](https://owasp.org/www-project-top-ten/)
- [ASP.NET Core Security Best Practices](https://docs.microsoft.com/en-us/aspnet/core/security/)
- [JWT Best Practices](https://tools.ietf.org/html/rfc8725)
- [Content Security Policy Guide](https://content-security-policy.com/)

---

**Última actualización:** Enero 2026  
**Versión:** 1.0
