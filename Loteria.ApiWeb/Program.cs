using Dapper;
using Loteria.Datos; // Fundamental para que la API conozca tu fábrica
using Loteria.Datos.Repositorios;
using Loteria.Entidades.DTOs;
using Loteria.Entidades.Identity;
using Loteria.Negocio.Servicios;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using MySqlConnector;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// 1. Obtenemos el texto de conexión desde la configuración.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
if (string.IsNullOrWhiteSpace(connectionString))
{
    throw new InvalidOperationException("Falta configurar ConnectionStrings:DefaultConnection.");
}

// 2. Registramos nuestra fábrica en el sistema de Inyección de Dependencias.
builder.Services.AddSingleton(new ConnectionFactory(connectionString));

// Inyectamos los repositorios al sistema.
builder.Services.AddTransient<UsuarioRepository>();
builder.Services.AddTransient<UsuarioService>();

builder.Services.AddTransient<JugadorRepository>();
builder.Services.AddTransient<JugadorService>();

builder.Services.AddTransient<SorteoRepository>();
builder.Services.AddTransient<SorteoService>();

builder.Services.AddTransient<ExtraccionRepository>();

builder.Services.AddTransient<CartonRepository>();
builder.Services.AddTransient<CartonService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityRequirement(document => new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecuritySchemeReference("Bearer", document, null),
            new List<string>()
        }
    });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Escribe: Bearer {tu token}",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    });
});

// CONFIGURACIÓN DE SEGURIDAD JWT
var jwtKey = builder.Configuration["Jwt:Key"];
if (string.IsNullOrWhiteSpace(jwtKey) || Encoding.UTF8.GetByteCount(jwtKey) < 32)
{
    throw new InvalidOperationException(
        "Falta configurar Jwt:Key o la clave es demasiado corta. Debe tener al menos 32 bytes.");
}

var jwtIssuer = builder.Configuration["Jwt:Issuer"];
var jwtAudience = builder.Configuration["Jwt:Audience"];

if (string.IsNullOrWhiteSpace(jwtIssuer) || string.IsNullOrWhiteSpace(jwtAudience))
{
    throw new InvalidOperationException("Falta configurar Jwt:Issuer y/o Jwt:Audience.");
}

var keyBytes = Encoding.UTF8.GetBytes(jwtKey);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtIssuer,
            ValidAudience = jwtAudience,
            IssuerSigningKey = new SymmetricSecurityKey(keyBytes)
        };
    });

// Encendemos el sistema de autorizaciones (para poder usar roles).
builder.Services.AddAuthorization();

var app = builder.Build();

// Configuraciones del entorno HTTP.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();

// Endpoint de prueba para saber que la API responde.
app.MapGet("/", () => "¡La API de la Loteria Virtual esta funcionando perfecto!");

// USUARIOS
app.MapGet("/api/usuarios", async (UsuarioService servicio) =>
{
    try
    {
        var usuarios = await servicio.ObtenerTodosAsync();

        var respuestaExitosa = new ApiResponseDTO
        {
            OK = true,
            Mensaje = "Retornando todo los usuarios",
            Data = usuarios,
            Errores = null
        };

        return Results.Ok(respuestaExitosa);
    }
    catch (Exception ex)
    {
        var respuestaError = new ApiResponseDTO
        {
            OK = false,
            Mensaje = "Error",
            Data = null,
            Errores = ex.Message
        };

        return Results.BadRequest(respuestaError);
    }
}).RequireAuthorization(policy => policy.RequireRole("1"));

app.MapGet("/api/usuarios/{id}", async (int id, UsuarioService servicio) =>
{
    try
    {
        var usuario = await servicio.ObtenerPorIdAsync(id);
        var respuestaExitosa = new ApiResponseDTO
        {
            OK = true,
            Mensaje = "Retornando el usuario encontrado",
            Data = usuario,
            Errores = null
        };

        return Results.Ok(respuestaExitosa);
    }
    catch (Exception ex)
    {
        var respuestaError = new ApiResponseDTO
        {
            OK = false,
            Mensaje = "Error",
            Data = null,
            Errores = ex.Message
        };

        return Results.BadRequest(respuestaError);
    }
}).RequireAuthorization(policy => policy.RequireRole("1"));

app.MapPost("/api/usuarios", async (UsuarioService servicio, Usuario nuevoUsuario) =>
{
    try
    {
        await servicio.CrearUsuarioAsync(nuevoUsuario);

        var respuestaExitosa = new ApiResponseDTO
        {
            OK = true,
            Mensaje = "Usuario creado con exito",
            Data = null,
            Errores = null
        };

        return Results.Ok(respuestaExitosa);
    }
    catch (Exception ex)
    {
        var respuestaError = new ApiResponseDTO
        {
            OK = false,
            Mensaje = "Error",
            Data = null,
            Errores = ex.Message
        };

        return Results.BadRequest(respuestaError);
    }
}).RequireAuthorization(policy => policy.RequireRole("1"));

app.MapPut("/api/usuarios/{id}", async (int id, UsuarioService servicio, Usuario usuarioEditado) =>
{
    if (usuarioEditado.Id != id)
    {
        var respuestaError = new ApiResponseDTO
        {
            OK = false,
            Mensaje = "El ID de la URL no coincide con el del paquete de datos",
            Data = null,
            Errores = null
        };

        return Results.BadRequest(respuestaError);
    }

    var usuario = await servicio.ObtenerPorIdAsync(id);

    if (usuario == null)
    {
        var respuestaError = new ApiResponseDTO
        {
            OK = false,
            Mensaje = $"No se encontro al usuario con el numero {id}",
            Data = null,
            Errores = null
        };

        return Results.NotFound(respuestaError);
    }

    try
    {
        await servicio.ActualizarUsuarioAsync(usuarioEditado);

        var respuestaExitosa = new ApiResponseDTO
        {
            OK = true,
            Mensaje = "Usuario actualizado correctamente",
            Data = null,
            Errores = null
        };

        return Results.Ok(respuestaExitosa);
    }
    catch (Exception ex)
    {
        var respuestaError = new ApiResponseDTO
        {
            OK = false,
            Mensaje = "Error",
            Data = null,
            Errores = ex.Message
        };

        return Results.BadRequest(respuestaError);
    }
}).RequireAuthorization(policy => policy.RequireRole("1"));

app.MapDelete("/api/usuarios/{id}", async (int id, UsuarioService servicio) =>
{
    var usuario = await servicio.ObtenerPorIdAsync(id);

    try
    {
        if (usuario == null)
        {
            var respuestaError = new ApiResponseDTO
            {
                OK = false,
                Mensaje = $"No se encontro al usuario con el numero {id}",
                Data = null,
                Errores = null
            };

            return Results.NotFound(respuestaError);
        }

        await servicio.EliminarUsuarioAsync(id);

        var respuestaExitosa = new ApiResponseDTO
        {
            OK = true,
            Mensaje = "Usuario se eliminó correctamente",
            Data = null,
            Errores = null
        };

        return Results.Ok(respuestaExitosa);
    }
    catch (Exception ex)
    {
        var respuestaError = new ApiResponseDTO
        {
            OK = false,
            Mensaje = "Error",
            Data = null,
            Errores = ex.Message
        };

        return Results.BadRequest(respuestaError);
    }
}).RequireAuthorization(policy => policy.RequireRole("1"));

app.MapPost("/api/usuarios/login", async (UsuarioService service, LoginRequestDTO request) =>
{
    try
    {
        var usuario = await service.LoginAsync(request);

        var respuestaExitosa = new ApiResponseDTO
        {
            OK = true,
            Mensaje = "Inicio de sesión exitoso.",
            Data = usuario,
            Errores = null
        };

        return Results.Ok(respuestaExitosa);
    }
    catch (Exception ex)
    {
        var respuestaError = new ApiResponseDTO
        {
            OK = false,
            Mensaje = "Hubo un problema al iniciar sesión.",
            Data = null,
            Errores = ex.Message
        };

        return Results.BadRequest(respuestaError);
    }
});

// JUGADORES
app.MapPost("/api/auth/registro", async (RegistroJugadorDTO nuevoJugador, UsuarioService service) =>
{
    try
    {
        await service.RegistrarJugadorWebAsync(nuevoJugador);
        var respuestaExitosa = new ApiResponseDTO
        {
            OK = true,
            Mensaje = "Jugador creado con exito",
            Data = null,
            Errores = null
        };
        return Results.Ok(respuestaExitosa);
    }
    catch (Exception ex)
    {
        var respuestaError = new ApiResponseDTO
        {
            OK = false,
            Mensaje = "Error.",
            Data = null,
            Errores = ex.Message
        };

        return Results.BadRequest(respuestaError);
    }
});

app.MapGet("/api/jugadores", async (JugadorService servicio) =>
{
    try
    {
        var jugadores = await servicio.ObtenerTodosAsync();

        var respuestaExitosa = new ApiResponseDTO
        {
            OK = true,
            Mensaje = "Retornando todos los jugadores",
            Data = jugadores,
            Errores = null
        };
        return Results.Ok(respuestaExitosa);
    }
    catch (Exception ex)
    {
        var respuestaError = new ApiResponseDTO
        {
            OK = false,
            Mensaje = "Error.",
            Data = null,
            Errores = ex.Message
        };

        return Results.BadRequest(respuestaError);
    }
}).RequireAuthorization(policy => policy.RequireRole("1"));

app.MapGet("/api/jugadores/{id}", async (int id, JugadorService servicio) =>
{
    try
    {
        var jugador = await servicio.ObtenerPorIdAsync(id);

        if (jugador == null)
        {
            var respuestaError = new ApiResponseDTO
            {
                OK = false,
                Mensaje = $"No se encontro jugador con el numero {id}",
                Data = null,
                Errores = null
            };

            return Results.NotFound(respuestaError);
        }

        var respuestaExitosa = new ApiResponseDTO
        {
            OK = true,
            Mensaje = "Retornando el jugador encontrado",
            Data = jugador,
            Errores = null
        };
        return Results.Ok(respuestaExitosa);
    }
    catch (Exception ex)
    {
        var respuestaError = new ApiResponseDTO
        {
            OK = false,
            Mensaje = "Error.",
            Data = null,
            Errores = ex.Message
        };

        return Results.BadRequest(respuestaError);
    }
}).RequireAuthorization(policy => policy.RequireRole("1", "2"));

app.MapPut("/api/jugadores/{id}", async (int id, JugadorService servicio, Jugador jugadorEditado) =>
{
    if (jugadorEditado.Id != id)
    {
        var respuestaError = new ApiResponseDTO
        {
            OK = false,
            Mensaje = "El ID de la URL no coincide con el del paquete de datos",
            Data = null,
            Errores = null
        };

        return Results.BadRequest(respuestaError);
    }

    var jugador = await servicio.ObtenerPorIdAsync(id);

    if (jugador == null)
    {
        var respuestaError = new ApiResponseDTO
        {
            OK = false,
            Mensaje = $"No se encontro al jugador con el numero {id}",
            Data = null,
            Errores = null
        };

        return Results.NotFound(respuestaError);
    }

    try
    {
        await servicio.ActualizarJugadorAsync(jugadorEditado);

        var respuestaExitosa = new ApiResponseDTO
        {
            OK = true,
            Mensaje = "Jugador actualizado correctamente",
            Data = null,
            Errores = null
        };

        return Results.Ok(respuestaExitosa);
    }
    catch (Exception ex)
    {
        var respuestaError = new ApiResponseDTO
        {
            OK = false,
            Mensaje = "Error.",
            Data = null,
            Errores = ex.Message
        };

        return Results.BadRequest(respuestaError);
    }
}).RequireAuthorization(policy => policy.RequireRole("1", "2"));

app.MapDelete("/api/jugadores/{id}", async (int id, JugadorService servicio) =>
{
    try
    {
        var jugador = await servicio.ObtenerPorIdAsync(id);

        if (jugador == null)
        {
            var respuestaError = new ApiResponseDTO
            {
                OK = false,
                Mensaje = $"No se encontro jugador con el numero {id}",
                Data = null,
                Errores = null
            };

            return Results.NotFound(respuestaError);
        }
        await servicio.EliminarJugadorAsync(id);

        var respuestaExitosa = new ApiResponseDTO
        {
            OK = true,
            Mensaje = "Jugador se elimino correctamente",
            Data = null,
            Errores = null
        };

        return Results.Ok(respuestaExitosa);
    }
    catch (Exception ex)
    {
        var respuestaError = new ApiResponseDTO
        {
            OK = false,
            Mensaje = "Error",
            Data = null,
            Errores = ex.Message
        };

        return Results.BadRequest(respuestaError);
    }
}).RequireAuthorization(policy => policy.RequireRole("1", "2"));

// SORTEOS
app.MapGet("/api/sorteos", async (SorteoService service) =>
{
    try
    {
        var sorteos = await service.ObtenerTodosAsync();

        var respuestaExitosa = new ApiResponseDTO
        {
            OK = true,
            Mensaje = "Retornando todos los sorteos",
            Data = sorteos,
            Errores = null
        };

        return Results.Ok(respuestaExitosa);
    }
    catch (Exception ex)
    {
        var respuestaError = new ApiResponseDTO
        {
            OK = false,
            Mensaje = "Error",
            Data = null,
            Errores = ex.Message
        };

        return Results.BadRequest(respuestaError);
    }
}).RequireAuthorization(policy => policy.RequireRole("1", "2"));

app.MapPost("/api/sorteos", async (SorteoService service, Sorteo nuevoSorteo) =>
{
    try
    {
        await service.CrearSorteoAsync(nuevoSorteo);

        var respuestaExitosa = new ApiResponseDTO
        {
            OK = true,
            Mensaje = "Sorteo creado con exito",
            Data = null,
            Errores = null
        };

        return Results.Ok(respuestaExitosa);
    }
    catch (Exception ex)
    {
        var respuestaError = new ApiResponseDTO
        {
            OK = false,
            Mensaje = "Error",
            Data = null,
            Errores = ex.Message
        };

        return Results.BadRequest(respuestaError);
    }
}).RequireAuthorization(policy => policy.RequireRole("1"));

app.MapGet("/api/sorteos/{id}", async (int id, SorteoService service) =>
{
    try
    {
        var sorteo = await service.ObtenerPorIdAsync(id);

        if (sorteo == null)
        {
            var respuestaError = new ApiResponseDTO
            {
                OK = false,
                Mensaje = $"No se encontro sorteo con el numero {id}",
                Data = null,
                Errores = null
            };

            return Results.NotFound(respuestaError);
        }

        var respuestaExitosa = new ApiResponseDTO
        {
            OK = true,
            Mensaje = "Retornando el sorteo encontrado",
            Data = sorteo,
            Errores = null
        };

        return Results.Ok(respuestaExitosa);
    }
    catch (Exception ex)
    {
        var respuestaError = new ApiResponseDTO
        {
            OK = false,
            Mensaje = "Error",
            Data = null,
            Errores = ex.Message
        };

        return Results.BadRequest(respuestaError);
    }
}).RequireAuthorization(policy => policy.RequireRole("1", "2"));

app.MapPut("/api/sorteos/{id}", async (int id, SorteoService service, Sorteo sorteoModificado) =>
{
    if (sorteoModificado.Id != id)
    {
        var respuestaError = new ApiResponseDTO
        {
            OK = false,
            Mensaje = "El ID de la URL no coincide con el del paquete de datos",
            Data = null,
            Errores = null
        };

        return Results.BadRequest(respuestaError);
    }

    var sorteo = await service.ObtenerPorIdAsync(id);

    if (sorteo == null)
    {
        var respuestaError = new ApiResponseDTO
        {
            OK = false,
            Mensaje = $"No se encontro sorteo con el numero {id}",
            Data = null,
            Errores = null
        };

        return Results.NotFound(respuestaError);
    }

    try
    {
        await service.ActualizarSorteoAsync(sorteoModificado);

        var respuestaExitosa = new ApiResponseDTO
        {
            OK = true,
            Mensaje = "Sorteo actualizado correctamente",
            Data = null,
            Errores = null
        };

        return Results.Ok(respuestaExitosa);
    }
    catch (Exception ex)
    {
        var respuestaError = new ApiResponseDTO
        {
            OK = false,
            Mensaje = "Error",
            Data = null,
            Errores = ex.Message
        };

        return Results.BadRequest(respuestaError);
    }
}).RequireAuthorization(policy => policy.RequireRole("1"));

app.MapDelete("/api/sorteos/{id}/cancelar", async (int id, SorteoService sorteoService) =>
{
    var sorteo = await sorteoService.ObtenerPorIdAsync(id);

    if (sorteo == null)
    {
        var respuestaError = new ApiResponseDTO
        {
            OK = false,
            Mensaje = $"No se encontro sorteo con el numero {id}",
            Data = null,
            Errores = null
        };

        return Results.NotFound(respuestaError);
    }

    try
    {
        await sorteoService.CancelarSorteoAsync(id);

        var respuestaExitosa = new ApiResponseDTO
        {
            OK = true,
            Mensaje = "Sorteo cancelado con exito",
            Data = null,
            Errores = null
        };

        return Results.Ok(respuestaExitosa);
    }
    catch (Exception ex)
    {
        var respuestaError = new ApiResponseDTO
        {
            OK = false,
            Mensaje = "Error",
            Data = null,
            Errores = ex.Message
        };

        return Results.BadRequest(respuestaError);
    }
}).RequireAuthorization(policy => policy.RequireRole("1"));

// CARTONES
app.MapGet("/api/sorteos/{idSorteo}/cartones", async (int idSorteo, CartonService service) =>
{
    try
    {
        var cartones = await service.ObtenerCartonesPorSorteoAsync(idSorteo);

        var respuestaExitosa = new ApiResponseDTO
        {
            OK = true,
            Mensaje = "Retornando los cartones del sorteo",
            Data = cartones,
            Errores = null
        };

        return Results.Ok(respuestaExitosa);
    }
    catch (Exception ex)
    {
        var respuestaError = new ApiResponseDTO
        {
            OK = false,
            Mensaje = "Error",
            Data = null,
            Errores = ex.Message
        };

        return Results.BadRequest(respuestaError);
    }
}).RequireAuthorization(policy => policy.RequireRole("1", "2"));

app.MapGet("/api/jugadores/{idJugador}/mis-jugadas", async (int idJugador, CartonService service) =>
{
    try
    {
        var jugadas = await service.ObtenerMisJugadasAsync(idJugador);

        var respuestaExitosa = new ApiResponseDTO
        {
            OK = true,
            Mensaje = "Retornando las jugadas del jugador",
            Data = jugadas,
            Errores = null
        };

        return Results.Ok(respuestaExitosa);
    }
    catch (Exception ex)
    {
        var respuestaError = new ApiResponseDTO
        {
            OK = false,
            Mensaje = "Error",
            Data = null,
            Errores = ex.Message
        };

        return Results.BadRequest(respuestaError);
    }
}).RequireAuthorization(policy => policy.RequireRole("2"));

app.MapPut("/api/jugadores/{idJugador}/mis-jugadas/{idCarton}/cancelar", async (int idJugador, int idCarton, CartonService service, JugadorRepository jugadorRepository, ClaimsPrincipal usuarioActual) =>
{
    try
    {
        var usuarioIdTexto = usuarioActual.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!int.TryParse(usuarioIdTexto, out int usuarioId))
        {
            return Results.Forbid();
        }

        var jugador = await jugadorRepository.ObtenerPorUsuarioIdAsync(usuarioId);

        if (jugador == null || jugador.Id != idJugador)
        {
            return Results.Forbid();
        }

        var pedido = new CompraCartonesDTO
        {
            JugadorId = idJugador,
            CartonesIds = new List<int> { idCarton }
        };

        await service.CancelarCartonReservaAsync(pedido);

        var respuestaExitosa = new ApiResponseDTO
        {
            OK = true,
            Mensaje = "Jugada cancelada con exito",
            Data = null,
            Errores = null
        };

        return Results.Ok(respuestaExitosa);
    }
    catch (Exception ex)
    {
        var respuestaError = new ApiResponseDTO
        {
            OK = false,
            Mensaje = "Error",
            Data = null,
            Errores = ex.Message
        };

        return Results.BadRequest(respuestaError);
    }
}).RequireAuthorization(policy => policy.RequireRole("2"));

app.MapPost("/api/cartones", async (CartonService service, Carton nuevoCarton) =>
{
    try
    {
        await service.CrearCartonAsync(nuevoCarton);

        var respuestaExitosa = new ApiResponseDTO
        {
            OK = true,
            Mensaje = "Carton creados con exito",
            Data = null,
            Errores = null
        };

        return Results.Ok(respuestaExitosa);
    }
    catch (Exception ex)
    {
        var respuestaError = new ApiResponseDTO
        {
            OK = false,
            Mensaje = "Error",
            Data = null,
            Errores = ex.Message
        };

        return Results.BadRequest(respuestaError);
    }
}).RequireAuthorization(policy => policy.RequireRole("1"));

app.MapPost("/api/cartones/comprar", async (CartonService service, CompraCartonesDTO pedido) =>
{
    try
    {
        await service.ComprarCartonesAsync(pedido);

        var respuestaExitosa = new ApiResponseDTO
        {
            OK = true,
            Mensaje = "Cartones seleccionados con exito",
            Data = null,
            Errores = null
        };

        return Results.Ok(respuestaExitosa);
    }
    catch (Exception ex)
    {
        var respuestaError = new ApiResponseDTO
        {
            OK = false,
            Mensaje = "Error",
            Data = null,
            Errores = ex.Message
        };

        return Results.BadRequest(respuestaError);
    }
}).RequireAuthorization(policy => policy.RequireRole("2"));

app.MapPut("/api/cartones/aprobar", async (CartonService service, CompraCartonesDTO pedido) =>
{
    try
    {
        await service.AprobacionCartonPagoAsync(pedido);

        var respuestaExitosa = new ApiResponseDTO
        {
            OK = true,
            Mensaje = "Pagos aprobados y cartones vendidos con éxito",
            Data = null,
            Errores = null
        };

        return Results.Ok(respuestaExitosa);
    }
    catch (Exception ex)
    {
        var respuestaError = new ApiResponseDTO
        {
            OK = false,
            Mensaje = "Error",
            Data = null,
            Errores = ex.Message
        };

        return Results.BadRequest(respuestaError);
    }
}).RequireAuthorization(policy => policy.RequireRole("1"));

app.MapPut("/api/cartones/cancelar", async (CartonService service, CompraCartonesDTO pedido) =>
{
    try
    {
        await service.CancelarCartonReservaAsync(pedido);

        var respuestaExitosa = new ApiResponseDTO
        {
            OK = true,
            Mensaje = "Cartones anulados con éxito",
            Data = null,
            Errores = null
        };

        return Results.Ok(respuestaExitosa);
    }
    catch (Exception ex)
    {
        var respuestaError = new ApiResponseDTO
        {
            OK = false,
            Mensaje = "Error",
            Data = null,
            Errores = ex.Message
        };

        return Results.BadRequest(respuestaError);
    }
}).RequireAuthorization(policy => policy.RequireRole("1"));

app.MapPost("/api/sorteos/{id}/abrir", async (int id, AbrirSorteoDTO datos, SorteoService service) =>
{
    try
    {
        await service.AbrirSorteoAsync(id, datos.Cantidad);

        var respuestaExitosa = new ApiResponseDTO
        {
            OK = true,
            Mensaje = "Sorteo abierto y cartones generados exitosamente",
            Data = null,
            Errores = null
        };

        return Results.Ok(respuestaExitosa);
    }
    catch (Exception ex)
    {
        var respuestaError = new ApiResponseDTO
        {
            OK = false,
            Mensaje = "Error al abrir el sorteo",
            Data = null,
            Errores = ex.Message
        };

        return Results.BadRequest(respuestaError);
    }
}).RequireAuthorization(policy => policy.RequireRole("1"));

// Solo un administrador puede ejecutar el sorteo.
app.MapPost("/api/sorteos/{id}/jugar", async (int id, SorteoService service) =>
{
    try
    {
        var cartonGanador = await service.RealizarSorteoAsync(id);

        var respuestaExitosa = new ApiResponseDTO
        {
            OK = true,
            Mensaje = "Sorteo Realizado con exito",
            Data = cartonGanador,
            Errores = null
        };

        return Results.Ok(respuestaExitosa);
    }
    catch (Exception ex)
    {
        var respuestaError = new ApiResponseDTO
        {
            OK = false,
            Mensaje = "Error",
            Data = null,
            Errores = ex.Message
        };

        return Results.BadRequest(respuestaError);
    }
}).RequireAuthorization(policy => policy.RequireRole("1"));

app.MapGet("/api/sorteos/{id}/resultados", async (int id, SorteoService service) =>
{
    try
    {
        var resultados = await service.ObtenerResultadosAsync(id);

        if (resultados == null)
        {
            var respuestaError = new ApiResponseDTO
            {
                OK = false,
                Mensaje = $"No se encontro sorteo con el numero {id}",
                Data = null,
                Errores = null
            };

            return Results.NotFound(respuestaError);
        }

        var respuestaExitosa = new ApiResponseDTO
        {
            OK = true,
            Mensaje = "Retornando resultados del sorteo",
            Data = resultados,
            Errores = null
        };

        return Results.Ok(respuestaExitosa);
    }
    catch (Exception ex)
    {
        var respuestaError = new ApiResponseDTO
        {
            OK = false,
            Mensaje = "Error",
            Data = null,
            Errores = ex.Message
        };

        return Results.BadRequest(respuestaError);
    }
}).RequireAuthorization(policy => policy.RequireRole("1", "2"));

// Bootstrap de datos mínimos del sistema.
using (var scope = app.Services.CreateScope())
{
    var connectionFactory = scope.ServiceProvider.GetRequiredService<ConnectionFactory>();

    const string sqlRoles = "INSERT IGNORE INTO Roles (Id, Nombre) VALUES (1, 'Admin'), (2, 'Jugador');";

    using var conexion = connectionFactory.CreateConnection();
    await conexion.ExecuteAsync(sqlRoles);

    var adminUsername = builder.Configuration["BootstrapAdmin:Username"];
    var adminPassword = builder.Configuration["BootstrapAdmin:Password"];

    var tieneUsername = !string.IsNullOrWhiteSpace(adminUsername);
    var tienePassword = !string.IsNullOrWhiteSpace(adminPassword);

    if (tieneUsername != tienePassword)
    {
        throw new InvalidOperationException(
            "Para crear el administrador inicial deben configurarse BootstrapAdmin:Username y BootstrapAdmin:Password juntos.");
    }

    if (tieneUsername && tienePassword)
    {
        if (adminPassword!.Length < 12)
        {
            throw new InvalidOperationException(
                "BootstrapAdmin:Password debe tener al menos 12 caracteres.");
        }

        var adminHash = BCrypt.Net.BCrypt.HashPassword(adminPassword);

        const string sqlAdmin = @"
            INSERT IGNORE INTO Usuarios (RolId, Username, PasswordHash, Activo)
            VALUES (1, @Username, @Hash, 1);";

        await conexion.ExecuteAsync(sqlAdmin, new
        {
            Username = adminUsername,
            Hash = adminHash
        });
    }
}

app.Run();
