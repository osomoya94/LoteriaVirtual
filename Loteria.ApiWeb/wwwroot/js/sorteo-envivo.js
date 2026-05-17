document.addEventListener("DOMContentLoaded", function () {

    // 1. Conseguimos el token y el ID del sorteo
    const tokenAutorizacion = localStorage.getItem("token");
    const parametrosUrl = new URLSearchParams(window.location.search);
    const idSorteo = parametrosUrl.get("id");

    if (!idSorteo) {
        alert("No se especificó qué sorteo ver.");
        window.location.href = "sorteos.html";
        return;
    }

    console.log("¡Estamos en la sala del sorteo número:", idSorteo, "!");

    // ==========================================
    // FUNCIÓN: Muestra a todos los ganadores 🏆
    // ==========================================
    function mostrarGanadorFinal(cartonesGanadores) {
        const contenedorGanador = document.getElementById("contenedor-ganador");
        const elementoNombre = contenedorGanador.querySelector(".ganador-nombre");

        // Si hay elementos en la lista de ganadores
        if (cartonesGanadores && cartonesGanadores.length > 0) {

            // Usamos .map para armar un texto lindo por cada cartón ganador que exista
            const textosGanadores = cartonesGanadores.map(function(carton) {
                // Soportamos la variante cartonId que descubrió Codex
                const idCarton = carton.id || carton.Id || carton.cartonId || carton.CartonId;
                const idJugador = carton.jugadorId || carton.JugadorId || carton.jugador_id || "Desconocido";
                return `Cartón Ganador Nº #${idCarton} (ID Jugador: ${idJugador})`;
            });

            // Los juntamos separados por un salto de línea para que se vean uno abajo del otro
            elementoNombre.innerHTML = textosGanadores.join("<br>");

        } else {
            elementoNombre.textContent = "El sorteo finalizó sin cartones ganadores.";
        }

        // Hacemos aparecer el panel verde de Bootstrap
        contenedorGanador.classList.remove("d-none");
        console.log("¡Cartel de ganadores mostrado con éxito!");
    }
    // ==========================================
    // FUNCIÓN: Maneja la animación de bolillas 🎱
    // ==========================================
    function mostrarAnimacionBolillas(listaNumeros, cartonesGanadores) {
        const contenedorBolillero = document.getElementById("contenedor-bolillero");
        contenedorBolillero.classList.remove("d-none");

        const elementoBolillaActual = document.getElementById("bolilla-actual");
        const elementoHistorial = document.getElementById("numeros-historial");

        let indice = 0;

        const animacion = setInterval(function () {
            if (indice >= listaNumeros.length) {
                clearInterval(animacion);
                console.log("Terminaron de salir las bolillas. Anunciando ganador...");
                mostrarGanadorFinal(cartonesGanadores);
                return;
            }

            const numeroSacado = listaNumeros[indice];
            elementoBolillaActual.textContent = numeroSacado;
            elementoHistorial.innerHTML += `<div class="bolilla-pequena" style="display:inline-block; margin:5px; padding:10px; background:#0d6efd; color:white; border-radius:50%; width:40px; height:40px; text-align:center; font-weight:bold;">${numeroSacado}</div>`;

            indice++;
        }, 1500); // 1.5 segundos entre bolillas para que no sea tan largo de probar!
    }

    // ==========================================
    // MOTOR: El Polling que pregunta a la API ⏱️
    // ==========================================
    const temporizador = setInterval(function () {

        axios.get(`/api/sorteos/${idSorteo}/resultados`, {
            headers: { Authorization: "Bearer " + tokenAutorizacion }
        })
            .then(function (respuesta) {
                const datosSorteo = respuesta.data.data;

                // Soportamos mayúsculas o minúsculas de la API de forma segura
                const estado = datosSorteo.estado || datosSorteo.Estado;

                console.log("Revisando la base de datos... Estado actual: " + estado);
            
            // Leemos la lista cruda que manda el servidor
                const listaCruda = datosSorteo.numerosExtraidos || datosSorteo.ListaNumeros || [];
            
                // TRUCO JUNIOR: Extraemos solo el número de cada objeto usando .map()
                const numeros = listaCruda.map(item => item.numeroExtraido || item.NumeroExtraido || item);
            
                const ganadores = datosSorteo.cartonesGanadores || datosSorteo.CartonGanadores || [];

                // REGLA JUNIOR: Si el sorteo está FINALIZADO y ya tenemos los números guardados, ¡arranca el show!
                if (estado === "FINALIZADO" && numeros.length > 0) {

                    clearInterval(temporizador); // Frenamos el Polling para no saturar el servidor
                    console.log("¡Resultados encontrados! Iniciando animación de bolillas...");

                    // Ocultamos el cartel de carga giratorio
                    document.getElementById("mensaje-espera").classList.add("d-none");

                    // Lanzamos la animación pasándole la lista completa de números y los ganadores
                    mostrarAnimacionBolillas(numeros, ganadores);
                }
            })
            .catch(function (error) {
                console.log("Esperando que se realice el sorteo en el backend...", error);
            });

    }, 3000); // Revisa la API cada 3 segundos

});