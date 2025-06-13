using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using mycrypt.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TuProyecto.Models.DTOs;

[ApiController]
[Route("api/[controller]")]
public class UsuariosController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IConfiguration _config;

    public UsuariosController(ApplicationDbContext context, IConfiguration config)
    {
        _context = context;
        _config = config;
    }

    [HttpPost("login")]
    public async Task<ActionResult> Login([FromBody] UsuarioLoginDto dto)
    {
        var usuario = await _context.Usuarios
            .FirstOrDefaultAsync(u => u.Nombre == dto.Nombre && u.Contrasenia == dto.Contrasenia);

        if (usuario == null) return Unauthorized();

        var claims = new[]
        {
        new Claim("UsuarioId", usuario.Id.ToString()),
        new Claim(ClaimTypes.Name, usuario.Nombre)
    };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddHours(2),
            signingCredentials: creds
        );

        return Ok(new
        {
            token = new JwtSecurityTokenHandler().WriteToken(token),
            id = usuario.Id
        });
    }


    [HttpGet("{id}/pesos")]
    public async Task<ActionResult<decimal>> ObtenerPesosDisponibles(int id)
    {
        var usuario = await _context.Usuarios.FindAsync(id);

        if (usuario == null)
            return NotFound("Usuario no encontrado.");

        return Ok(usuario.TotalPesos);
    }

    [HttpGet]
    public async Task<ActionResult> GetAll()
    {
        var usuarios = await _context.Usuarios
            .Select(u => new { u.Id, u.Nombre })
            .ToListAsync();

        return Ok(usuarios);
    }



}
