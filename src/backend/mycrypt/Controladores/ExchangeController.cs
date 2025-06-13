using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using mycrypt.Data;

[ApiController]
[Route("api/[controller]")]
public class ExchangeController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public ExchangeController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult> GetAll()
    {
        var exchanges = await _context.Exchanges
            .Select(e => new { e.Id, e.Nombre })
            .ToListAsync();

        return Ok(exchanges);
    }
}
