document.addEventListener("DOMContentLoaded", function () {
    const spanNombre = document.getElementById("nombre-jugador");
    const nombreJugador = localStorage.getItem("nombreJugador");
    const tokenAutorizacion = localStorage.getItem("token");
    const btnLogout = document.getElementById("btn-logout");
    const divCerraSesion = document.getElementById("mensaje-logout");
    const btnSorteos = document.getElementById("btn-ir-sorteos");
    const btnJugadas = document.getElementById("btn-ir-jugadas");
    const linkIrVivo = document.getElementById("link-ir-vivo");
    const btnIrVivo = document.getElementById("btn-ir-vivo");
    
    if (!nombreJugador) {
        window.location.href = "login.html"; 
        return;
    }

    spanNombre.innerText = nombreJugador;


    function obtenerValorSorteo(sorteo, propiedad) {
        const propiedadMayuscula = propiedad.charAt(0).toUpperCase() + propiedad.slice(1);
        return sorteo[propiedad] ?? sorteo[propiedadMayuscula];
    }

    function obtenerFechaSorteo(sorteo) {
        const fecha = obtenerValorSorteo(sorteo, "fecha_sorteo");
        const fechaParseada = new Date(fecha).getTime();
        return Number.isNaN(fechaParseada) ? 0 : fechaParseada;
    }

    function elegirSorteoEnVivo(sorteos) {
        return sorteos
            .filter(function (sorteo) {
                const estado = String(obtenerValorSorteo(sorteo, "estado") ?? "").toUpperCase();
                // REGLA ESTRICTA: Solo entramos a salas que estén esperando jugar
                return estado === "ABIERTO";
            })
            .sort(function (a, b) {
                // Ordenamos para que agarre el sorteo más viejo que siga abierto
                return obtenerFechaSorteo(a) - obtenerFechaSorteo(b) ||
                    Number(obtenerValorSorteo(a, "id") ?? 0) - Number(obtenerValorSorteo(b, "id") ?? 0);
            })[0];
    }

    function irASorteoEnVivo(evento) {
        if (evento) {
            evento.preventDefault();
        }

        axios
            .get("/api/sorteos", { headers: { Authorization: "Bearer " + tokenAutorizacion } })
            .then(function (respuesta) {
                const sorteos = respuesta.data.data ?? [];
                const sorteoEnVivo = elegirSorteoEnVivo(sorteos);

                if (!sorteoEnVivo) {
                    alert("No hay sorteos disponibles para ver en vivo.");
                    return;
                }

                const idSorteo = obtenerValorSorteo(sorteoEnVivo, "id");
                window.location.href = `sorteo-envivo.html?id=${idSorteo}`;
            })
            .catch(function (error) {
                console.log(error.response?.data || error);
                alert("No se pudo abrir el sorteo en vivo.");
            });
    }

    if (linkIrVivo) {
        linkIrVivo.addEventListener("click", irASorteoEnVivo);
    }

    if (btnIrVivo) {
        btnIrVivo.addEventListener("click", irASorteoEnVivo);
    }

    btnLogout.addEventListener("click", function () {
        localStorage.removeItem("nombreJugador");
        localStorage.removeItem("token");
        localStorage.removeItem("idJugador");
        mostrarCerrarSesion()
        setTimeout(function () {
            window.location.href = "login.html";
        }, 2000);
    });

    function mostrarCerrarSesion() {
        divCerraSesion.classList.remove("d-none");
    }

    btnSorteos.addEventListener("click", function () {
        window.location.href = "sorteos.html";
        return;
    });

    btnJugadas.addEventListener("click", function () {
        window.location.href = "mis-jugadas.html";
        return;
    });

});
