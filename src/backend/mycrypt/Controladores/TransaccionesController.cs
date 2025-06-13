using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using mycrypt.Data;
using mycrypt.Models.DTOs;
using mycrypt.Models.Entidades;
using mycrypt.Servicios;

[ApiController]
[Route("api/[controller]")]
public class TransaccionesController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly ICriptoYaService _criptoYaService;

    public TransaccionesController(ApplicationDbContext context, ICriptoYaService criptoYaService)
    {
        _context = context;
        _criptoYaService = criptoYaService;
    }


    [HttpPost]
    public async Task<ActionResult> Crear([FromBody] TransaccionCreateDto dto)
    {
        // Validar que la cantidad sea mayor a 0
        if (dto.Cantidad <= 0)
            return BadRequest("La cantidad debe ser mayor a 0.");

        var cripto = await _context.Criptos.FindAsync(dto.IdCripto);
        var exchange = await _context.Exchanges.FindAsync(dto.IdExchange);
        var usuario = await _context.Usuarios.FindAsync(dto.IdUsuario);

        if (cripto == null || exchange == null || usuario == null)
            return BadRequest("Datos inválidos.");

        // Consultar precio actual de la cripto
        var precioActual = await _criptoYaService.ObtenerPrecioAsync(cripto.Codigo.ToLower(), exchange.Nombre.ToLower());
        if (precioActual == null)
            return StatusCode(503, "No se pudo obtener el precio de CriptoYa.");

        decimal monto = dto.Cantidad * precioActual.Value;

        if (dto.Tipo == "Compra")
        {
            // Validar que tenga suficiente dinero
            if (usuario.TotalPesos < monto)
                return BadRequest("Fondos insuficientes en pesos para realizar la compra.");

            // Restar pesos
            usuario.TotalPesos -= monto;
        }
        else if (dto.Tipo == "Venta")
        {
            // Calcular criptos disponibles
            var saldoCripto = await _context.Transacciones
                .Where(t => t.IdUsuario == dto.IdUsuario && t.IdCripto == dto.IdCripto)
                .SumAsync(t => t.Tipo == "Compra" ? t.Cantidad : -t.Cantidad);

            if (saldoCripto < dto.Cantidad)
                return BadRequest("No posee suficientes criptomonedas para vender.");

            // Sumar pesos
            usuario.TotalPesos += monto;
        }
        else
        {
            return BadRequest("El tipo de transacción debe ser 'Compra' o 'Venta'.");
        }

        var transaccion = new Transaccion
        {
            Tipo = dto.Tipo,
            Fecha = dto.Fecha,
            Cantidad = dto.Cantidad,
            MontoARS = monto,
            IdCripto = dto.IdCripto,
            IdExchange = dto.IdExchange,
            IdUsuario = dto.IdUsuario
        };

        _context.Transacciones.Add(transaccion);
        await _context.SaveChangesAsync();

        return Ok(new { transaccion.Id });
    }


    [HttpGet]
    public async Task<ActionResult> GetTodas()
    {
        var transacciones = await _context.Transacciones
            .Include(t => t.Cripto)
            .Include(t => t.Exchange)
            .Include(t => t.Usuario)
            .OrderByDescending(t => t.Fecha)
            .Select(t => new TransaccionResumenDto
            {
                Id = t.Id,
                Tipo = t.Tipo,
                Cripto = t.Cripto.Nombre,
                Cantidad = t.Cantidad,
                MontoARS = t.MontoARS,
                Fecha = t.Fecha
            })
            .ToListAsync();

        return Ok(transacciones);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> GetPorId(int id)
    {
        var t = await _context.Transacciones
            .Include(t => t.Cripto)
            .Include(t => t.Exchange)
            .Include(t => t.Usuario)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (t == null) return NotFound();

        var dto = new TransaccionDetalleDto
        {
            Id = t.Id,
            Tipo = t.Tipo,
            Fecha = t.Fecha,
            Cantidad = t.Cantidad,
            MontoARS = t.MontoARS,
            CriptoNombre = t.Cripto.Nombre,
            ExchangeNombre = t.Exchange.Nombre,
            UsuarioNombre = t.Usuario.Nombre
        };

        return Ok(dto);
    }

[HttpGet("usuario/{idUsuario}")]
public async Task<ActionResult> GetByUsuario(int idUsuario)
{
    var transacciones = await _context.Transacciones
        .Where(t => t.IdUsuario == idUsuario)
        .Include(t => t.Cripto)
        .Include(t => t.Exchange)
        .Include(t => t.Usuario)
        .Select(t => new {
            t.Id,
            t.Tipo,
            t.Fecha,
            t.Cantidad,
            t.MontoARS,
            t.IdExchange,
            Exchange = t.Exchange != null ? new { t.Exchange.Id, t.Exchange.Nombre } : null,
            t.IdCripto,
            Cripto = t.Cripto != null ? new { t.Cripto.Id, t.Cripto.Nombre } : null,
            t.IdUsuario,
            Usuario = t.Usuario != null ? new { t.Usuario.Id, t.Usuario.Nombre } : null
        })
        .ToListAsync();

    return Ok(transacciones);
}




}
