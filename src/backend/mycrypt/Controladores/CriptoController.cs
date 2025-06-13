using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using mycrypt.Data;

[ApiController]
[Route("api/[controller]")]
public class CriptoController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public CriptoController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult> GetAll()
    {
        var criptos = await _context.Criptos
            .Select(c => new { c.Id, c.Nombre })
            .ToListAsync();

        return Ok(criptos);
    }
}
