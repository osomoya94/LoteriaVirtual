using Loteria.Datos; // Fundamental para que la API conozca tu fábrica
using Loteria.Datos.Repositorios;
using Loteria.Entidades.Identity;
using Loteria.Negocio.Servicios;

var builder = WebApplication.CreateBuilder(args);

// 1. Obtenemos el texto de conexión desde el appsettings.json
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

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

// Configuraciones estándar de la API
builder.Services.AddOpenApi();

var app = builder.Build();

// Configuraciones del entorno HTTP
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

// Aquí irán nuestros futuros endpoints (rutas) de la lotería
// Endpoint de prueba para saber que la API responde
app.MapGet("/", () => "¡La API de la Loteria Virtual esta funcionando perfecto!");

app.MapGet("/api/usuarios", async (UsuarioService servicio) =>
{
    var usuarios = await servicio.ObtenerTodosAsync();
    return Results.Ok(usuarios);
});

// NUEVO: Endpoint para buscar un usuario específico
app.MapGet("/api/usuarios/{id}", async (int id, UsuarioService servicio) =>
{
    var usuario = await servicio.ObtenerPorIdAsync(id);

    // Si el usuario es "null" (no existe en la base de datos), devolvemos un error 404 Not Found
    if (usuario == null)
    {
        return Results.NotFound(new { mensaje = $"No se encontró al jugador con el número {id}" });
    }

    // Si lo encontró, lo devolvemos con éxito (200 OK)
    return Results.Ok(usuario);
});

//Crear usuario
app.MapPost("/api/usuarios", async (UsuarioService servicio, Usuario nuevoUsuario) => 
{
    try
    {
        await servicio.CrearUsuarioAsync(nuevoUsuario);

        return Results.Ok(new { mensaje = "Usuario creado con exito" });
    }
    catch (Exception ex)
    {
        return Results.BadRequest(new { mensaje = ex.Message });
    }
    
});

//Actualizar Usuario
app.MapPut("/api/usuarios/{id}", async (int id , UsuarioService servicio, Usuario usuarioEditado) => 
{
    if (usuarioEditado.Id != id ) 
    {
        return Results.BadRequest(new { mensaje = "El ID de la URL no coincide con el del paquete de datos" });
    }

    var usuario = await servicio.ObtenerPorIdAsync(id);

    if (usuario == null)
    {
        return Results.NotFound(new { mensaje = $"No se encontró al usuario con el número {id}" });
    }

    try
    {
        await servicio.ActualizarUsuarioAsync(usuarioEditado);
        return Results.Ok(new { mensaje = "Usuario actualizado correctamente" });
    }
    catch (Exception ex)
    {
        return Results.BadRequest(new { mensaje = ex.Message });
    }
});

//Eliminar Usuario
app.MapDelete("/api/usuarios/{id}", async (int id, UsuarioService servicio) =>
{
    var usuario = await servicio.ObtenerPorIdAsync(id);

    if (usuario == null)
    {
        return Results.NotFound(new { mensaje = $"No se encontró al usuario con el número {id}" });
    }

    await servicio.EliminarUsuarioAsync(id);

    return Results.Ok(new { mensaje = "Usuario se eliminó correctamente" });
});

//Crear jugador
app.MapPost("/api/jugadores", async (JugadorService service, Jugador nuevoJugador) => 
{
    try 
    {
        await service.CrearJugadorAsync(nuevoJugador);

        return Results.Ok(new { mensaje = "Jugador creado con exito" });
    }
    catch (Exception ex)
    {
        return Results.BadRequest(new { mensaje = ex.Message });
    }
});
//  Endpoint para buscar todos los jugadores
app.MapGet("/api/jugadores", async (JugadorService servicio) =>
{
    var jugadores = await servicio.ObtenerTodosAsync();
    return Results.Ok(jugadores);
});

//  Endpoint para buscar un jugador específico
app.MapGet("/api/jugadores/{id}", async (int id, JugadorService servicio) =>
{
    var jugador = await servicio.ObtenerPorIdAsync(id);

    if (jugador == null)
    {
        return Results.NotFound(new { mensaje = $"No se encontró al jugador con el número {id}" });
    }

    return Results.Ok(jugador);
});

// Actlizar jugador
app.MapPut("/api/jugadores/{id}", async (int id, JugadorService servicio, Jugador jugadorEditado) =>
{
    if (jugadorEditado.Id != id)
    {
        return Results.BadRequest(new { mensaje = "El ID de la URL no coincide con el del paquete de datos" });
    }

    var jugador = await servicio.ObtenerPorIdAsync(id);

    if (jugador == null)
    {
        return Results.NotFound(new { mensaje = $"No se encontró al jugador con el número {id}" });
    }

    try
    {
        await servicio.ActualizarJugadorAsync(jugadorEditado);
        return Results.Ok(new { mensaje = "Jugador actualizado correctamente" });
    }
    catch (Exception ex)
    {
        return Results.BadRequest(new { mensaje = ex.Message });
    }
});

//Elimina jugador
app.MapDelete("/api/jugadores/{id}", async (int id, JugadorService servicio) =>
{
    var jugador = await servicio.ObtenerPorIdAsync(id);

    if (jugador == null)
    {
        return Results.NotFound(new { mensaje = $"No se encontró al jugador con el número {id}" });
    }

    await servicio.EliminarJugadorAsync(id);

    return Results.Ok(new { mensaje = "Jugador se eliminó correctamente" });
});

//SORTEOS
app.MapGet("/api/sorteos", async (SorteoService service) => 
{
    var sorteos = await service.ObtenerTodosAsync();
    return Results.Ok(sorteos);
});

app.MapPost("/api/sorteos", async (SorteoService service, Sorteo nuevoSorteo) => 
{
    try
    {
        await service.CrearSorteoAsync(nuevoSorteo);

        return Results.Ok(new { mensaje = "Sorteo creado con exito" });
    }
    catch (Exception ex) 
    {
        return Results.BadRequest(new { mensaje = ex.Message });
    }
});

app.MapGet("/api/sorteos/{id}", async (int id , SorteoService service) => 
{
    var sorteo = await service.ObtenerPorIdAsync(id);

    if(sorteo == null) 
    {
        return Results.BadRequest(new { mensaje = $"No se encotro sorte con el numero de {id}" });
    }

    return Results.Ok(sorteo);
});

app.MapPut("/api/sorteos/{id}", async (int id, SorteoService service, Sorteo sorteoModificado) => 
{
    if (sorteoModificado.Id != id)
    {
        return Results.BadRequest(new { mensaje = "El ID de la URL no coincide con el del paquete de datos" });
    }

    var sorteo = await service.ObtenerPorIdAsync(id);

    if (sorteo == null)
    {
        return Results.BadRequest(new { mensaje = $"No se encotro sorte con el numero de {id}" });
    }

    try 
    {
        await service.ActualizarSorteoAsync(sorteoModificado);
        return Results.Ok(new { mensaje = "Sorteo actualizado correctamente" });
    } catch (Exception ex) 
    {
        return Results.BadRequest(new { mensaje = ex.Message });
    }
});

app.MapDelete("/api/sorteos/{id}/cancelar", async (int id, SorteoService sorteoService) =>
{
    var sorteo = await sorteoService.ObtenerPorIdAsync(id);

    if (sorteo == null)
    {
        return Results.NotFound(new { mensaje = $"No se encontró sorteo con el número {id}" });
    }

    try
    {
        await sorteoService.CancelarSorteoAsync(id);
        return Results.Ok(new { mensaje = "Cancelo sorteo con exito" });
    }
    catch (Exception ex)
    {
        return Results.BadRequest(new { mensaje = ex.Message });
    }
});


// CARTONES
app.MapGet("/api/sorteos/{idSorteo}/cartones", async (int idSorteo, CartonService service) =>
{
    try
    {
        var cartones = await service.ObtenerCartonesPorSorteoAsync(idSorteo);
        return Results.Ok(cartones); 
    }
    catch (Exception ex)
    {
        return Results.BadRequest(new { mensaje = ex.Message });
    }
});

app.MapPost("/api/cartones", async (CartonService service,Carton nuevoCarton ) => 
{
    try
    {
        await service.CrearCartonAsync(nuevoCarton);

        return Results.Ok(new { mensaje = "Cartones creados con exito" });
    }
    catch (Exception ex)
    {
        return Results.BadRequest(new { mensaje = ex.Message });
    }
});



app.Run();

