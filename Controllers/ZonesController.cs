using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MDMS_PRoject_Backend.Data;
using MDMS_PRoject_Backend.Models;

namespace MDMS_PRoject_Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ZonesController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public ZonesController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Zone>>> GetZones([FromQuery] string? status)
    {
        var query = _context.Zones.Include(z => z.Admin).Include(z => z.Settings).AsQueryable();
        if (!string.IsNullOrEmpty(status))
        {
            query = query.Where(z => z.Status.ToLower() == status.ToLower());
        }
        return await query.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Zone>> GetZone(string id)
    {
        var zone = await _context.Zones
            .Include(z => z.Admin)
            .Include(z => z.Settings)
            .FirstOrDefaultAsync(z => z.Id == id);

        if (zone == null) return NotFound();
        return zone;
    }

    [HttpPost]
    public async Task<ActionResult<Zone>> CreateZone(Zone zone)
    {
        _context.Zones.Add(zone);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetZone), new { id = zone.Id }, zone);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateZone(string id, Zone zone)
    {
        if (id != zone.Id) return BadRequest("ID mismatch");

        _context.Entry(zone).State = EntityState.Modified;
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.Zones.Any(e => e.Id == id)) return NotFound();
            throw;
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteZone(string id)
    {
        var zone = await _context.Zones.FindAsync(id);
        if (zone == null) return NotFound();

        _context.Zones.Remove(zone);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
