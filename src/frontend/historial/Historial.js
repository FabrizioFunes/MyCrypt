const URL_BASE = "https://localhost:7050/api";
const usuarioId = localStorage.getItem("usuarioId");

if (!usuarioId) {
  alert("Debe iniciar sesión.");
  window.location.href = "login.html";
}

fetch(`${URL_BASE}/Transacciones/usuario/${usuarioId}`)
  .then(res => res.json())
  .then(transacciones => {
    const tbody = document.getElementById("tablaTransacciones");
    tbody.innerHTML = "";

    transacciones.forEach(t => {
      const fila = document.createElement("tr");

      fila.innerHTML = `
        <td>${t.id}</td>
        <td>${t.tipo}</td>
        <td>${new Date(t.fecha).toLocaleString()}</td>
        <td>${t.cripto?.nombre || "Desconocido"}</td>
        <td>${Number(t.cantidad).toLocaleString(undefined, { minimumFractionDigits: 8 })}</td>
        <td>$${Number(t.montoARS).toFixed(2)}</td>
        <td>
          <button 
            class="btn btn-sm btn-primary" 
            data-bs-toggle="modal" 
            data-bs-target="#detalleModal"
            data-id="${t.id}"
            data-tipo="${t.tipo}"
            data-fecha="${t.fecha}"
            data-cripto="${t.cripto?.nombre || 'Desconocido'}"
            data-exchange="${t.exchange?.nombre || 'Desconocido'}"
            data-cantidad="${t.cantidad}"
            data-monto="${t.montoARS}"
            data-usuario="${t.usuario?.nombre || 'Desconocido'}"
            onclick="verDetalle(this)">
            Ver
          </button>
        </td>
      `;

      tbody.appendChild(fila);
    });
  })
  .catch(err => {
    console.error("Error al cargar transacciones:", err);
    alert("No se pudieron cargar las transacciones.");
  });

function verDetalle(boton) {
  const contenido = `
    <p><strong>ID:</strong> ${boton.dataset.id}</p>
    <p><strong>Tipo:</strong> ${boton.dataset.tipo}</p>
    <p><strong>Fecha:</strong> ${new Date(boton.dataset.fecha).toLocaleString()}</p>
    <p><strong>Criptomoneda:</strong> ${boton.dataset.cripto}</p>
    <p><strong>Exchange:</strong> ${boton.dataset.exchange}</p>
    <p><strong>Cantidad:</strong> ${Number(boton.dataset.cantidad).toLocaleString(undefined, { minimumFractionDigits: 8 })}</p>
    <p><strong>Monto ARS:</strong> $${Number(boton.dataset.monto).toFixed(2)}</p>
    <p><strong>Usuario:</strong> ${boton.dataset.usuario}</p>
  `;

  document.getElementById("detalleContenido").innerHTML = contenido;
}
