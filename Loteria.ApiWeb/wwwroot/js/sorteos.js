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

document.addEventListener("DOMContentLoaded", function () {
    const nombreJugador = localStorage.getItem("nombreJugador");
    const tokenAutorizacion = localStorage.getItem("token");
    const idJugador = localStorage.getItem("idJugador");
    const contenedorSorteos = document.getElementById("contenedor-sorteos");
    const modalCompra = new bootstrap.Modal(document.getElementById("modal-compra"));
    const grillaCartones = document.getElementById("grilla-cartones");
    const btnConfirmarCompra = document.getElementById("btn-confirmar-compra");

    if (!nombreJugador) {
        window.location.href = "login.html";
        return;
    }

    // 1. Pedimos los sorteos
    axios
        .get("/api/sorteos", { headers: { Authorization: "Bearer " + tokenAutorizacion } })
        .then(function (respuesta) {
            const listaDeSorteos = respuesta.data.data;

            const sorteosDisponibles = listaDeSorteos.filter(function (sorteo) {
                return sorteo.estado === 'ABIERTO'
            });

            sorteosDisponibles.forEach((sorteo) => {
                contenedorSorteos.innerHTML += `
                <div class="col-12 col-md-6 col-xl-4">
                <article class="card dashboard-card h-100">
                    <div class="card-body d-flex flex-column p-4">
                        <div class="feature-icon mb-4" aria-hidden="true">
                            <i class="bi bi-stars"></i>
                        </div>
                        <h2 class="h4 card-title">${sorteo.nombre}</h2>
                        <p class="card-text text-secondary mb-2">Fecha: ${sorteo.fecha_sorteo}</p>
                        <p class="card-text fw-semibold mb-4">Precio de carton: $${sorteo.precio_carton}</p>
                        <p class="card-text fw-semibold mb-4">Cantidad de cartones : ${sorteo.cantidad_cartones}</p>
                        <p class="card-text fw-semibold mb-4">Procetanje del ganador : ${sorteo.porcentaje_premio}%</p>
                        <button type="button" class="btn btn-primary mt-auto btn-ver-cartones" data-id-sorteo="${sorteo.id}">
                            Comprar Cartones
                        </button>
                    </div>
                </article>
                </div>
                `;
            });

            const botonesComprar = document.querySelectorAll(".btn-ver-cartones");

            botonesComprar.forEach(function (boton) {
                boton.addEventListener("click", function () {

                    const idSorteoSeleccionado = boton.getAttribute("data-id-sorteo");

                    axios.get(`/api/sorteos/${idSorteoSeleccionado}/cartones`, {
                        headers: { Authorization: "Bearer " + tokenAutorizacion }
                    })
                        .then((respuesta) => {
                            const cartones = respuesta.data.data;

                            const cartonesDisponibles = cartones.filter(function (carton) {
                                return carton.estado === 'DISPONIBLE';
                            });

                            grillaCartones.innerHTML = "";

                            cartonesDisponibles.forEach((carton) => {
                                grillaCartones.innerHTML += `
                                <article class="carton-wrapper overflow-hidden mb-4 border rounded">
                                    <div class="carton-switch d-flex align-items-center justify-content-between gap-3 p-3 bg-light">
                                        <label class="form-check-label fw-semibold fs-5" for="check-${carton.id}">
                                            Seleccionar este cartón
                                        </label>
                                        <div class="form-check form-switch m-0">
                                            <input class="form-check-input check-carton" type="checkbox" role="switch" id="check-${carton.id}" value="${carton.id}">
                                        </div>
                                    </div>
                                    <div class="p-3 text-center overflow-auto">
                                        <div class="carton-grid mt-3 mx-auto" aria-label="Carton ${carton.id}">
                                            ${generarCeldasCarton(carton.patron_contenido)}
                                        </div>
                                        <p class="text-muted mb-0">Grilla de números del cartón (ID: ${carton.id})</p>
                                    </div>
                                </article>
                                `;
                            });

                            modalCompra.show();
                        })
                        .catch((error) => {
                            console.log(error.response?.data || error);
                        });
                });
            });

        })
        .catch(function (error) {
            console.log(error.response?.data || error)
        });


    btnConfirmarCompra.addEventListener("click", function () {
        const cartonesSeleccionados = document.querySelectorAll(".check-carton:checked");
        const idsCartones = [];

        cartonesSeleccionados.forEach((checkbox) => {
            const id = Number(checkbox.value);
            idsCartones.push(id);
        });

        console.log("Cartones listos para comprar:", idsCartones);

        const datosParaEnviar = {
            JugadorId: Number(idJugador),
            CartonesIds: idsCartones
        };

        axios
            .post("/api/cartones/comprar", datosParaEnviar , { headers: { Authorization: "Bearer " + tokenAutorizacion }})
            .then(function (respuesta) {
                console.log("Peticion exitosa");
                modalCompra.hide();
                alert("¡Cartones comprados con éxito, a la espera de autorización!");
            })
            .catch(function (error) {
                console.log(error.response?.data || error)
            });
    });

});
