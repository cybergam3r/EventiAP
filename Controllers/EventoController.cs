using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/eventi")]
[ApiController]
public class EventoController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public EventoController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetEventi()
    {
        var eventi = await _context.Eventi.Include(e => e.Artista).ToListAsync();
        return Ok(eventi);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Evento>> GetEvento(int id)
    {
        var evento = await _context.Eventi.FindAsync(id);

        if (evento == null)
        {
            return NotFound();
        }

        return evento;
    }

    [Authorize(Roles = "Amministratore")]
    [HttpPost]
    public async Task<ActionResult<Evento>> PostEvento(Evento evento)
    {
        _context.Eventi.Add(evento);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetEvento), new { id = evento.EventoId }, evento);
    }

    [Authorize(Roles = "Amministratore")]
    [HttpPut("{id}")]
    public async Task<IActionResult> PutEvento(int id, Evento evento)
    {
        if (id != evento.EventoId)
        {
            return BadRequest();
        }

        _context.Entry(evento).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!EventoExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    [Authorize(Roles = "Amministratore")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEvento(int id)
    {
        var evento = await _context.Eventi.FindAsync(id);
        if (evento == null)
        {
            return NotFound();
        }

        _context.Eventi.Remove(evento);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [Authorize(Roles = "Amministratore")]
    [HttpGet("tutti")]
    public async Task<IActionResult> GetTuttiBiglietti()
    {
        var biglietti = await _context.Biglietti.Include(b => b.Evento).Include(b => b.User).ToListAsync();
        return Ok(biglietti);
    }

    [HttpPost("acquista")]
    public async Task<IActionResult> AcquistaBiglietto(AcquistoBigliettoDto acquistoBigliettoDto)
    {
        var evento = await _context.Eventi.FindAsync(acquistoBigliettoDto.EventoId);
        if (evento == null)
        {
            return NotFound("Evento non trovato.");
        }

        var biglietto = new Biglietto
        {
            EventoId = acquistoBigliettoDto.EventoId,
            UserId = acquistoBigliettoDto.UserId,
            DataAcquisto = acquistoBigliettoDto.DataAcquisto
        };

        _context.Biglietti.Add(biglietto);
        await _context.SaveChangesAsync();

        return Ok(biglietto);
    }

    private bool EventoExists(int id)
    {
        return _context.Eventi.Any(e => e.EventoId == id);
    }
}

