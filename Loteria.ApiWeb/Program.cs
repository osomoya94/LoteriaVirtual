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
using System.Text;
using Microsoft.OpenApi;

var builder = WebApplication.CreateBuilder(args);

// 1. Obtenemos el texto de conexión desde el appsettings.json
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
if (string.IsNullOrWhiteSpace(connectionString))
{
    throw new InvalidOperationException("Falta configurar ConnectionStrings:DefaultConnection en appsettings.json.");
}

// 2. Registramos nuestra fábrica en el sistema de Inyección de Dependencias
builder.Services.AddSingleton(new ConnectionFactory(connectionString));

//Iyectamos los repositorios al sistema
builder.Services.AddTransient<UsuarioRepository>();
builder.Services.AddTransient<UsuarioService>();

builder.Services.AddTransient<JugadorRepository>();
builder.Services.AddTransient<JugadorService>();

builder.Services.AddTransient<SorteoRepository>();
builder.Services.AddTransient<SorteoService>();

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

//CONFIGURACIÓN DE SEGURIDAD JWT
var jwtKey = builder.Configuration["Jwt:Key"];
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
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(keyBytes)
        };
    });

// Encendemos el sistema de autorizaciones (para poder usar roles)
builder.Services.AddAuthorization();

var app = builder.Build();

// Configuraciones del entorno HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication(); // 1. Primero verifica que el pasaporte sea original y no esté vencido
app.UseAuthorization();  // 2. Después verifica qué Rol tiene anotado adentro

// Endpoint de prueba para saber que la API responde
app.MapGet("/", () => "¡La API de la Loteria Virtual esta funcionando perfecto!");

//PROBADO
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

//PROBADO
// NUEVO: Endpoint para buscar un usuario específico
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

//PROBADO
//Crear usuario, esto es para los admin, no le pongo para token para q tenga acceso a registrarse
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
    
}).RequireAuthorization(policy => policy.RequireRole("1")); // creamos un adminstrador para que ese cree demas administradores

//PROBAR
//Actualizar Usuario, es para el admin
app.MapPut("/api/usuarios/{id}", async (int id , UsuarioService servicio, Usuario usuarioEditado) => 
{
    if (usuarioEditado.Id != id ) 
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

//PROBAR
//Eliminar Usuario
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

//PROBADO
// Iniciar sesion
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

//Probado
//Crear jugador
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

//PROBADO
//  Endpoint para buscar todos los jugadores
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

//PROBANDO
//  Endpoint para buscar un jugador específico
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
}).RequireAuthorization(policy => policy.RequireRole("1", "2")); // le puse los dos roles para que el admin busque uno espesifico y que el mismo se quiera ver

// PROBAMOS, tenemos que verificar que no se dupliquen los usurname cuando se crean pero lo demas bien,
// Actlizar jugador
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
}).RequireAuthorization(policy => policy.RequireRole("1", "2"));// el jugador se quiera actulizar o el admin necesite hacer cambios

//PROBADO
//Elimina jugador
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

//SORTEOS
//PROBADO
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
}).RequireAuthorization(policy => policy.RequireRole("1"));

//PROBADO
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
});

//PROBADO
app.MapGet("/api/sorteos/{id}", async (int id , SorteoService service) => 
{
    try
    {
        var sorteo = await service.ObtenerPorIdAsync(id);

        if(sorteo == null) 
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
});

//PROBADO
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
    } catch (Exception ex) 
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
}).RequireAuthorization(policy => policy.RequireRole("1")); ;

// PROBADO
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
//PROBADO
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
}).RequireAuthorization(policy => policy.RequireRole("1"));

//CREAR CARTONES A MANO
// PROBADO
app.MapPost("/api/cartones", async (CartonService service,Carton nuevoCarton ) => 
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
//PROBADO
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

    } catch (Exception ex) 
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
//PROBADO
//Aceptar Cartones
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
//PROBADO
//Cancelar Cartones
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

//PROBADO
//Genera todos los cartones deceado hasta 850
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

//Emepzar sorteos
app.MapPost("/api/sorteos/{id}/jugar",async (int id, SorteoService service) => 
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

using (var scope = app.Services.CreateScope())
{
    var connectionFactory = scope.ServiceProvider.GetRequiredService<ConnectionFactory>();

    // Insertamos los roles
    string sqlRoles = "INSERT IGNORE INTO Roles (Id, Nombre) VALUES (1, 'Admin'), (2, 'Jugador');";

    //Preparamos los datos del primer Admin encriptando su clave
    string adminClave = "Admin1234!"; // Esta será tu contraseña real para entrar
    string adminHash = BCrypt.Net.BCrypt.HashPassword(adminClave);

    string sqlAdmin = @"
    INSERT IGNORE INTO Usuarios (RolId, Username, PasswordHash, Activo) 
    VALUES (1, 'admin_super', @Hash, 1);";

    using (var conexion = connectionFactory.CreateConnection())
    {
        await conexion.ExecuteAsync(sqlRoles);
        await conexion.ExecuteAsync(sqlAdmin, new { Hash = adminHash });
    }
}


app.Run();

