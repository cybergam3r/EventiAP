using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
[ApiController]
public class ArtistiController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public ArtistiController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Artista>>> GetArtisti()
    {
        return await _context.Artisti.ToListAsync();
    }

    [Authorize(Roles = "Amministratore")]
    [HttpPost]
    public async Task<ActionResult<Artista>> PostArtista(Artista artista)
    {
        _context.Artisti.Add(artista);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetArtisti), new { id = artista.ArtistaId }, artista);
    }

    [Authorize(Roles = "Amministratore")]
    [HttpPut("{id}")]
    public async Task<IActionResult> PutArtista(int id, Artista artista)
    {
        if (id != artista.ArtistaId)
        {
            return BadRequest();
        }

        _context.Entry(artista).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!ArtistaExists(id))
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
    public async Task<IActionResult> DeleteArtista(int id)
    {
        var artista = await _context.Artisti.FindAsync(id);
        if (artista == null)
        {
            return NotFound();
        }

        _context.Artisti.Remove(artista);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool ArtistaExists(int id)
    {
        return _context.Artisti.Any(e => e.ArtistaId == id);
    }
}


