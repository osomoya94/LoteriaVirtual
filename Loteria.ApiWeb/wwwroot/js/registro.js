document.addEventListener("DOMContentLoaded", function () {
  const formularioRegistro = document.getElementById("registro-form");
  const campoNombre = document.getElementById("nombre");
  const campoApellido = document.getElementById("apellido");
  const campoDni = document.getElementById("dni");
  const campoEmail = document.getElementById("email");
  const campoUsername = document.getElementById("username");
  const campoPassword = document.getElementById("password");
  const campoPasswordConfirm = document.getElementById("password-confirm");
  const btnRegistro = document.getElementById("btn-registro");
  const btnVerPass = document.getElementById("btn-ver-pass");
  const btnVerConfirm = document.getElementById("btn-ver-confirm");
  const divError = document.getElementById("mensaje-error");
  const divExito = document.getElementById("mensaje-exito");

  actualizarBotonPassword(campoPassword, btnVerPass);
  actualizarBotonPassword(campoPasswordConfirm, btnVerConfirm);

  btnVerPass.addEventListener("click", function () {
    cambiarVisibilidadPassword(campoPassword, btnVerPass);
  });

  btnVerConfirm.addEventListener("click", function () {
    cambiarVisibilidadPassword(campoPasswordConfirm, btnVerConfirm);
  });

  formularioRegistro.addEventListener("submit", function (event) {
    event.preventDefault();

    ocultarError();
    ocultarExito();

    const Nombre = campoNombre.value.trim();
    const Apellido = campoApellido.value.trim();
    const Dni = campoDni.value.trim();
    const Email = campoEmail.value.trim();
    const Username = campoUsername.value.trim();
    const Password = campoPassword.value;
    const confirmPassword = campoPasswordConfirm.value;

    if (!Username || !Password || !confirmPassword) {
      mostrarError("Debes ingresar usuario y contrasena.");
      return;
    }

    if (Password !== confirmPassword) {
      mostrarError("Las contrasenas no coinciden.");
      return;
    }

    const tieneLargoMinimo = Password.length >= 8;
    const tieneMayuscula = /[A-Z]/.test(Password);
    const tieneMinuscula = /[a-z]/.test(Password);
    const tieneCaracterEspecial = /[^A-Za-z0-9]/.test(Password);

    if (!tieneLargoMinimo || !tieneMayuscula || !tieneMinuscula || !tieneCaracterEspecial) {
      mostrarError("La contrasena debe tener minimo 8 caracteres, una mayuscula, una minuscula y un caracter especial.");
      return;
    }

    btnRegistro.disabled = true;

    axios
      .post("/api/auth/registro", {
        Nombre,
        Apellido,
        Dni,
        Email,
        Username,
        Password,
      })
      .then(function () {
        ocultarError();
        mostrarExito("Jugador creado con exito. Redirigiendo...");

        setTimeout(function () {
          window.location.href = "login.html";
        }, 2000);
      })
      .catch(function (error) {
        const respuestaApi = error.response && error.response.data;
        const mensajeApi = obtenerMensajeError(respuestaApi);

        mostrarError(mensajeApi || "No se pudo crear la cuenta.");
      })
      .finally(function () {
        btnRegistro.disabled = false;
      });
  });

  function cambiarVisibilidadPassword(campo, boton) {
    if (campo.type === "password") {
      campo.type = "text";
    } else {
      campo.type = "password";
    }

    actualizarBotonPassword(campo, boton);
  }

  function actualizarBotonPassword(campo, boton) {
    if (campo.type === "password") {
      boton.innerHTML = "&#128065;";
      boton.setAttribute("aria-label", "Mostrar contrasena");
      boton.setAttribute("title", "Mostrar contrasena");
    } else {
      boton.innerText = "Ocultar";
      boton.setAttribute("aria-label", "Ocultar contrasena");
      boton.setAttribute("title", "Ocultar contrasena");
    }
  }

  function obtenerMensajeError(respuestaApi) {
    if (!respuestaApi) {
      return null;
    }

    return (
      respuestaApi.errores ||
      respuestaApi.Errores ||
      respuestaApi.mensaje ||
      respuestaApi.Mensaje ||
      null
    );
  }

  function mostrarError(mensaje) {
    divError.innerText = mensaje;
    divError.classList.remove("d-none");
  }

  function ocultarError() {
    divError.innerText = "";
    divError.classList.add("d-none");
  }

  function mostrarExito(mensaje) {
    divExito.innerText = mensaje;
    divExito.classList.remove("d-none");
  }

  function ocultarExito() {
    divExito.innerText = "";
    divExito.classList.add("d-none");
  }
});
