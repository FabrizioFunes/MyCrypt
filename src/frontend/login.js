const apiUrl = "https://localhost:7050/api/Usuarios/login";

function login() {
  const nombre = document.getElementById("nombre").value.trim();
  const contrasenia = document.getElementById("contrasenia").value.trim();

  if (!nombre || !contrasenia) {
    mostrarMensaje("Debe completar ambos campos.", "warning");
    return;
  }

  const tieneLetra = /[a-zA-Z]/.test(contrasenia);
  const tieneNumero = /\d/.test(contrasenia);
  if (!tieneLetra || !tieneNumero) {
    mostrarMensaje("La contraseña debe tener letras y números.", "warning");
    return;
  }

  fetch(apiUrl, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify({ nombre, contrasenia })
  })
    .then(async res => {
      if (!res.ok) {
        mostrarMensaje("Credenciales inválidas o usuario no encontrado.", "danger");
        return;
      }

      const data = await res.json();
      const token = data.token;

      //Decodificar el token para obtener el ID
      const payload = parseJwt(token);
      const usuarioId = payload.UsuarioId;

      if (!usuarioId) {
        mostrarMensaje("No se pudo obtener el ID del usuario.", "danger");
        return;
      }

      //Guardar en localStorage
      localStorage.setItem("token", token);
      localStorage.setItem("usuarioId", usuarioId);

      mostrarMensaje("Login exitoso. Redireccionando...", "success");
      setTimeout(() => {
        window.location.href = "Portfolio.html";
      }, 1500);
    })
    .catch(err => {
      mostrarMensaje("Error al conectar con el servidor.", "danger");
      console.error(err);
    });
}

//Función para mostrar mensajes
function mostrarMensaje(texto, tipo) {
  const div = document.getElementById("mensaje");
  div.innerText = texto;
  div.className = `alert alert-${tipo} mt-3`;
  div.classList.remove("d-none");
}

function parseJwt(token) {
  const base64Url = token.split('.')[1];
  const base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
  const jsonPayload = decodeURIComponent(window
    .atob(base64)
    .split('')
    .map(c => '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2))
    .join(''));
  return JSON.parse(jsonPayload);
}
