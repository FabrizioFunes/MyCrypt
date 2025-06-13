const colores = {
  "Bitcoin": "btc",
  "Ethereum": "eth",
  "USD Coin": "usdc",
  "USDT": "usdt",
  "Solana": "sol"
};

const usuarioId = localStorage.getItem("usuarioId");

if (!usuarioId) {
  alert("Debe iniciar sesión.");
  window.location.href = "login.html";
}

fetch(`https://localhost:7050/api/Portfolio/${usuarioId}`)
  .then(r => r.json())
  .then(data => {
    const contenedor = document.getElementById("criptoCards");
    const total = data.reduce((acc, c) => acc + c.valorEnPesos, 0);

    document.getElementById("totalValor").innerText = `$${total.toLocaleString(undefined, { minimumFractionDigits: 2 })}`;

    data.forEach(item => {
      const color = colores[item.cripto] || "bg-secondary";
      const card = document.createElement("div");
      card.className = `cripto-card ${color}`;

      card.innerHTML = `
        <h5>${item.cripto}</h5>
        <p>${item.cantidad} ${obtenerSimbolo(item.cripto)}</p>
        <div class="text-end fw-bold fs-5">$${item.valorEnPesos.toLocaleString(undefined, { minimumFractionDigits: 2 })}</div>
      `;

      contenedor.appendChild(card);
    });
  })
  .catch(err => {
    console.error(err);
    alert("Error al cargar el portafolio.");
  });
  
// Obtener pesos disponibles del usuario
fetch(`https://localhost:7050/api/Usuarios/${usuarioId}/pesos`)
  .then(r => r.json())
  .then(pesos => {
    document.getElementById("pesosDisponibles").innerText =
      `$${pesos.toLocaleString(undefined, { minimumFractionDigits: 2 })}`;
  })
  .catch(err => {
    console.error("Error al obtener pesos:", err);
    document.getElementById("pesosDisponibles").innerText = "Error";
  });



function obtenerSimbolo(nombre) {
  const mapa = {
    "Bitcoin": "BTC",
    "Ethereum": "ETH",
    "USD Coin": "USDC",
    "USDT": "USDT",
    "Solana": "SOL"
  };
  return mapa[nombre] || nombre;
}
