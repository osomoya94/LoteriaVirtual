function generarCeldasCarton(patronContenido) {
    const numerosPorColumna = Array.from({ length: 9 }, () => []);
    const grilla = Array.from({ length: 3 }, () => Array(9).fill(null));
    const numerosPorFila = [0, 0, 0];
    const combinacionesPorCantidad = [
        [[]],
        [[0], [1], [2]],
        [[0, 1], [0, 2], [1, 2]],
        [[0, 1, 2]],
    ];

    String(patronContenido ?? "")
        .split("-")
        .map((valor) => parseInt(valor, 10))
        .filter((numero) => Number.isInteger(numero) && numero >= 1 && numero <= 90)
        .forEach((numero) => {
            const columna = numero === 90 ? 8 : Math.floor(numero / 10);
            numerosPorColumna[columna].push(numero);
        });

    numerosPorColumna.forEach((numeros, columna) => {
        const numerosOrdenados = numeros.sort((a, b) => a - b).slice(0, 3);
        const filasElegidas = combinacionesPorCantidad[numerosOrdenados.length]
            .map((filas) => {
                const conteos = [...numerosPorFila];
                filas.forEach((fila) => conteos[fila]++);

                return {
                    filas,
                    exceso: conteos.reduce((total, conteo) => total + Math.max(0, conteo - 5), 0),
                    balance: Math.max(...conteos) - Math.min(...conteos),
                    cargaActual: filas.reduce((total, fila) => total + numerosPorFila[fila], 0),
                };
            })
            .sort((a, b) =>
                a.exceso - b.exceso ||
                a.balance - b.balance ||
                a.cargaActual - b.cargaActual
            )[0].filas;

        filasElegidas.forEach((fila, indice) => {
            grilla[fila][columna] = numerosOrdenados[indice];
            numerosPorFila[fila]++;
        });
    });

    return grilla
        .flat()
        .map((numero) => {
            if (numero === null) {
                return `<div class="carton-cell is-empty" aria-hidden="true"></div>`;
            }

            return `<div class="carton-cell">${numero}</div>`;
        })
        .join("");
}

function escaparHtml(valor) {
    const reemplazos = {
        "&": "&amp;",
        "<": "&lt;",
        ">": "&gt;",
        '"': "&quot;",
        "'": "&#39;",
    };

    return String(valor ?? "").replace(/[&<>"']/g, (caracter) => reemplazos[caracter]);
}

function normalizarJugada(jugada) {
    return {
        id: jugada.id ?? jugada.Id,
        codigoUnico: jugada.codigoUnico ?? jugada.CodigoUnico ?? jugada.codigo_unico,
        patronContenido: jugada.patronContenido ?? jugada.PatronContenido ?? jugada.patron_contenido,
        estado: jugada.estado ?? jugada.Estado,
        sorteoNombre: jugada.sorteoNombre ?? jugada.SorteoNombre,
        sorteoFecha: jugada.sorteoFecha ?? jugada.SorteoFecha,
    };
}

function formatearFecha(fecha) {
    const fechaSorteo = new Date(fecha);

    if (Number.isNaN(fechaSorteo.getTime())) {
        return escaparHtml(fecha);
    }

    return fechaSorteo.toLocaleString("es-AR", {
        day: "2-digit",
        month: "2-digit",
        year: "numeric",
        hour: "2-digit",
        minute: "2-digit",
    });
}

function obtenerClaseEstado(estado) {
    if (estado === "RESERVADO") {
        return "text-bg-warning";
    }

    if (estado === "VENDIDO") {
        return "text-bg-success";
    }

    return "text-bg-secondary";
}

document.addEventListener("DOMContentLoaded", function () {
    const tokenAutorizacion = localStorage.getItem("token");
    const idJugador = localStorage.getItem("idJugador");
    const contenedorJugadas = document.getElementById("contenedor-mis-jugadas");
    const mensajeVacio = document.getElementById("mensaje-vacio");
    const mensajeError = document.getElementById("mensaje-error");
    const btnLogout = document.getElementById("btn-logout");

    if (!tokenAutorizacion || !idJugador) {
        window.location.href = "login.html";
        return;
    }

    btnLogout.addEventListener("click", function () {
        localStorage.removeItem("nombreJugador");
        localStorage.removeItem("token");
        localStorage.removeItem("idJugador");
        window.location.href = "login.html";
    });

    function mostrarError(mensaje) {
        mensajeError.textContent = mensaje;
        mensajeError.classList.remove("d-none");
    }

    function ocultarMensajes() {
        mensajeError.classList.add("d-none");
        mensajeError.textContent = "";
        mensajeVacio.classList.add("d-none");
    }

    function crearTarjetaJugada(jugada) {
        const estado = jugada.estado ?? "";
        const estaReservado = estado === "RESERVADO";
        const botonCancelar = estaReservado
            ? `<button type="button" class="btn btn-danger btn-cancelar-jugada" data-id-carton="${jugada.id}">
                    <i class="bi bi-x-circle me-1" aria-hidden="true"></i>
                    Cancelar Jugada
                </button>`
            : "";

        return `
            <div class="col-12 col-xl-6">
                <article class="card jugada-card h-100">
                    <div class="card-body p-4">
                        <div class="d-flex flex-column flex-md-row align-items-md-start justify-content-between gap-3">
                            <div>
                                <p class="text-uppercase text-secondary small fw-semibold mb-1">Carton #${jugada.id}</p>
                                <h2 class="h5 mb-1">${escaparHtml(jugada.sorteoNombre)}</h2>
                                <p class="text-secondary mb-0">Fecha: ${formatearFecha(jugada.sorteoFecha)}</p>
                            </div>
                            <span class="badge ${obtenerClaseEstado(estado)} align-self-start">${escaparHtml(estado)}</span>
                        </div>

                        <div class="carton-scroll mt-4">
                            <div class="carton-grid mx-auto" aria-label="Carton ${jugada.id}">
                                ${generarCeldasCarton(jugada.patronContenido)}
                            </div>
                        </div>

                        <div class="d-flex flex-column flex-md-row align-items-md-center justify-content-between gap-3 mt-4">
                            <span class="small text-secondary">Codigo: ${escaparHtml(jugada.codigoUnico)}</span>
                            ${botonCancelar}
                        </div>
                    </div>
                </article>
            </div>
        `;
    }

    function cargarMisJugadas() {
        ocultarMensajes();
        contenedorJugadas.innerHTML = "";

        axios
            .get(`/api/jugadores/${idJugador}/mis-jugadas`, {
                headers: { Authorization: "Bearer " + tokenAutorizacion }
            })
            .then(function (respuesta) {
                const jugadas = Array.isArray(respuesta.data.data)
                    ? respuesta.data.data.map(normalizarJugada)
                    : [];

                if (jugadas.length === 0) {
                    mensajeVacio.classList.remove("d-none");
                    return;
                }

                contenedorJugadas.innerHTML = jugadas.map(crearTarjetaJugada).join("");
            })
            .catch(function (error) {
                console.log(error.response?.data || error);
                mostrarError("No se pudieron cargar tus jugadas.");
            });
    }

    contenedorJugadas.addEventListener("click", function (event) {
        const botonCancelar = event.target.closest(".btn-cancelar-jugada");

        if (!botonCancelar) {
            return;
        }

        const idCarton = Number(botonCancelar.getAttribute("data-id-carton"));

        if (!idCarton || !confirm("Cancelar esta jugada?")) {
            return;
        }

        botonCancelar.disabled = true;

        axios
            .put(`/api/jugadores/${idJugador}/mis-jugadas/${idCarton}/cancelar`, null, {
                headers: { Authorization: "Bearer " + tokenAutorizacion }
            })
            .then(function () {
                cargarMisJugadas();
            })
            .catch(function (error) {
                console.log(error.response?.data || error);
                botonCancelar.disabled = false;
                mostrarError("No se pudo cancelar la jugada.");
            });
    });

    cargarMisJugadas();
});
