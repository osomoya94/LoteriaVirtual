document.addEventListener('DOMContentLoaded', function () {
    const formularioLogin = document.getElementById('login-form');
    const campoUsername = document.getElementById('username');
    const campoPassword = document.getElementById('password');
    const btnLogin = document.getElementById('btn-login');
    const divError = document.getElementById('mensaje-error');
    const divExito = document.getElementById("mensaje-exito");

    formularioLogin.addEventListener('submit', function (event) {
        event.preventDefault();

        ocultarError();

        const Username = campoUsername.value.trim();
        const Password = campoPassword.value;

        if (!Username || !Password) {
            mostrarError('Debes ingresar usuario y contrasena.');
            return;
        }

        btnLogin.disabled = true;

        axios.post('/api/usuarios/login', { Username, Password })
            .then(function (respuesta) {
                localStorage.setItem("nombreJugador", Username);
                localStorage.setItem("token",respuesta.data.data.token);
                localStorage.setItem("idJugador",respuesta.data.data.jugadorId);
                mostrarExito("Inicio de sesion exitoso. Redirigiendo..."); 
                setTimeout(function () {
                    window.location.href = "index.html"; 
                }, 2000);
            })
            .catch(function (error) {
                const respuestaApi = error.response && error.response.data;
                const mensajeApi = obtenerMensajeError(respuestaApi);

                mostrarError(mensajeApi || 'Usuario o contrasena incorrectos');
            })
            .finally(function () {
                btnLogin.disabled = false;
            });
    });

    function obtenerMensajeError(respuestaApi) {
        if (!respuestaApi) {
            return null;
        }

        return respuestaApi.errores
            || respuestaApi.Errores
            || respuestaApi.mensaje
            || respuestaApi.Mensaje
            || null;
    }

    function mostrarError(mensaje) {
        divError.innerText = mensaje;
        divError.classList.remove('d-none');
    }

    function ocultarError() {
        divError.innerText = '';
        divError.classList.add('d-none');
    }
    function mostrarExito(mensaje) {
        divExito.innerText = mensaje;
        divExito.classList.remove("d-none");
    }
});
