using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MDMS_PRoject_Backend.Data;
using MDMS_PRoject_Backend.Models;

namespace MDMS_PRoject_Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TariffSlabsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public TariffSlabsController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TariffSlab>>> GetTariffSlabs([FromQuery] string? tariffId)
    {
        var query = _context.TariffSlabs.AsQueryable();
        if (!string.IsNullOrEmpty(tariffId))
        {
            query = query.Where(s => s.TariffId == tariffId);
        }
        return await query.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TariffSlab>> GetTariffSlab(string id)
    {
        var slab = await _context.TariffSlabs.FindAsync(id);
        if (slab == null) return NotFound();
        return slab;
    }

    [HttpPost]
    public async Task<ActionResult<TariffSlab>> CreateTariffSlab(TariffSlab slab)
    {
        _context.TariffSlabs.Add(slab);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetTariffSlab), new { id = slab.Id }, slab);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTariffSlab(string id, TariffSlab slab)
    {
        if (id != slab.Id) return BadRequest("ID mismatch");

        _context.Entry(slab).State = EntityState.Modified;
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.TariffSlabs.Any(e => e.Id == id)) return NotFound();
            throw;
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTariffSlab(string id)
    {
        var slab = await _context.TariffSlabs.FindAsync(id);
        if (slab == null) return NotFound();

        _context.TariffSlabs.Remove(slab);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
