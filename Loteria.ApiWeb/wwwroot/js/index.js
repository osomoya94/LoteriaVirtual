document.addEventListener("DOMContentLoaded", function () {
    const spanNombre = document.getElementById("nombre-jugador");
    const nombreJugador = localStorage.getItem("nombreJugador");
    const btnLogout = document.getElementById("btn-logout");
    const divCerraSesion = document.getElementById("mensaje-logout");
    const btnSorteos = document.getElementById("btn-ir-sorteos");
    const btnJugadas = document.getElementById("btn-ir-jugadas");

    if (!nombreJugador) {
        window.location.href = "login.html"; 
        return;
    }

    spanNombre.innerText = nombreJugador;

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
