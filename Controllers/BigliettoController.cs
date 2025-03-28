using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

[Route("api/biglietti")]
[ApiController]
public class BigliettoController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public BigliettoController(ApplicationDbContext context)
    {
        _context = context;
    }

    [Authorize(Roles = "Utente")]
    [HttpPost]
    public async Task<IActionResult> AcquistaBiglietto([FromBody] AcquistoBigliettoDto dto)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null) return Unauthorized();

        var biglietto = new Biglietto
        {
            EventoId = dto.EventoId,
            UserId = userId
        };

        _context.Biglietti.Add(biglietto);
        await _context.SaveChangesAsync();

        return Ok(new { Message = "Biglietto acquistato con successo!" });
    }

    [Authorize(Roles = "Utente")]
    [HttpGet("miei")]
    public async Task<IActionResult> GetMieiBiglietti()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var biglietti = await _context.Biglietti
            .Where(b => b.UserId == userId)
            .Include(b => b.Evento)
            .ToListAsync();

        return Ok(biglietti);
    }
}
