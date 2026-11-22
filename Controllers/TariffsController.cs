using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MDMS_PRoject_Backend.Data;
using MDMS_PRoject_Backend.Models;

namespace MDMS_PRoject_Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TariffsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public TariffsController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Tariff>>> GetTariffs()
    {
        return await _context.Tariffs.Include(t => t.Slabs).ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Tariff>> GetTariff(string id)
    {
        var tariff = await _context.Tariffs.Include(t => t.Slabs).FirstOrDefaultAsync(t => t.Id == id);
        if (tariff == null) return NotFound();
        return tariff;
    }

    [HttpPost]
    public async Task<ActionResult<Tariff>> CreateTariff(Tariff tariff)
    {
        _context.Tariffs.Add(tariff);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetTariff), new { id = tariff.Id }, tariff);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTariff(string id, Tariff tariff)
    {
        if (id != tariff.Id) return BadRequest("ID mismatch");

        _context.Entry(tariff).State = EntityState.Modified;
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.Tariffs.Any(e => e.Id == id)) return NotFound();
            throw;
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTariff(string id)
    {
        var tariff = await _context.Tariffs.FindAsync(id);
        if (tariff == null) return NotFound();

        _context.Tariffs.Remove(tariff);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
