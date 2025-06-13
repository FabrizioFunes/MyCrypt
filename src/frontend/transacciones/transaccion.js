const apiUrl = "https://localhost:7050/api/Transacciones";
const usuarioId = localStorage.getItem("usuarioId");

if (!usuarioId) {
  alert("Debe iniciar sesión.");
  window.location.href = "login.html";
}

//Cargar criptos y exchanges al cargar la página
window.onload = () => {
  cargarFechaActual();
  cargarCriptos();
  cargarExchanges();
};

function cargarFechaActual() {
  const ahora = new Date().toISOString(); // ISO con zona horaria UTC
  document.getElementById("fechaVisual").value = new Date().toLocaleString();
  localStorage.setItem("fechaActual", ahora); // guardamos ISO para el backend
}

function cargarCriptos() {
  fetch("https://localhost:7050/api/Cripto")
    .then(r => r.json())
    .then(data => {
      const select = document.getElementById("idCripto");
      data.forEach(c => {
        const option = document.createElement("option");
        option.value = c.id;
        option.text = c.nombre;
        select.appendChild(option);
      });
    });
}

function cargarExchanges() {
  fetch("https://localhost:7050/api/Exchange")
    .then(r => r.json())
    .then(data => {
      const select = document.getElementById("idExchange");
      data.forEach(e => {
        const option = document.createElement("option");
        option.value = e.id;
        option.text = e.nombre;
        select.appendChild(option);
      });
    });
}

function enviarTransaccion() {
  const tipo = document.getElementById("tipo").value;
  const cantidad = parseFloat(document.getElementById("cantidad").value);
  const idCripto = parseInt(document.getElementById("idCripto").value);
  const idExchange = parseInt(document.getElementById("idExchange").value);
  const fecha = localStorage.getItem("fechaActual");

  if (!tipo || !cantidad || !idCripto || !idExchange || !fecha) {
    return mostrarMensaje("Todos los campos son obligatorios.", "warning");
  }

  if (cantidad <= 0) {
    return mostrarMensaje("La cantidad debe ser mayor a 0.", "warning");
  }

  const dto = {
    tipo,
    cantidad,
    idCripto,
    idExchange,
    fecha,
    idUsuario: parseInt(usuarioId)
  };

  fetch(apiUrl, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(dto)
  })
    .then(async res => {
      const data = await res.json();
      if (!res.ok) {
        return mostrarMensaje(data?.title || "Error al procesar la transacción", "danger");
      }
      mostrarMensaje("Transacción creada correctamente ✅", "success");
      cargarFechaActual(); // Actualiza hora después de cada envío
    })
    .catch(err => {
      console.error(err);
      mostrarMensaje("Error de conexión con el servidor.", "danger");
    });
}

function mostrarMensaje(texto, tipo) {
  const div = document.getElementById("mensaje");
  div.innerText = texto;
  div.className = `alert alert-${tipo} mt-3`;
  div.classList.remove("d-none");
}

