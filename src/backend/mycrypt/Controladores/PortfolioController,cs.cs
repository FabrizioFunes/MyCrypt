using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using mycrypt.Data;
using mycrypt.Models.DTOs;
using mycrypt.Servicios;

[ApiController]
[Route("api/[controller]")]
public class PortfolioController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly ICriptoYaService _criptoYaService;

    public PortfolioController(ApplicationDbContext context, ICriptoYaService criptoYaService)
    {
        _context = context;
        _criptoYaService = criptoYaService;
    }


    [HttpGet("{usuarioId}")]
    public async Task<ActionResult> ObtenerPortfolio(int usuarioId)
    {
        var datos = await _context.Transacciones
            .Where(t => t.IdUsuario == usuarioId)
            .GroupBy(t => t.IdCripto)
            .Select(g => new
            {
                IdCripto = g.Key,
                Cantidad = g.Sum(t => t.Tipo == "Compra" ? t.Cantidad : -t.Cantidad)
            })
            .Where(x => x.Cantidad > 0)
            .ToListAsync();

        var criptos = await _context.Criptos.ToDictionaryAsync(c => c.Id, c => c);

        var result = new List<PortfolioDto>();

        foreach (var d in datos)
        {
            var cripto = criptos[d.IdCripto];
            var precio = await _criptoYaService.ObtenerPrecioAsync(cripto.Codigo.ToLower(), "buenbit"); 
            decimal valorEnPesos = precio.HasValue ? precio.Value * d.Cantidad : 0;

            result.Add(new PortfolioDto
            {
                Cripto = cripto.Nombre,
                Cantidad = d.Cantidad,
                ValorEnPesos = valorEnPesos
            });
        }


        return Ok(result);
    }
}
