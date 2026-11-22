using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MDMS_PRoject_Backend.Data;
using MDMS_PRoject_Backend.Models;

namespace MDMS_PRoject_Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ZoneSettingsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public ZoneSettingsController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ZoneSettings>>> GetZoneSettings([FromQuery] string? zoneId)
    {
        var query = _context.ZoneSettings.Include(s => s.Zone).AsQueryable();
        if (!string.IsNullOrEmpty(zoneId))
        {
            query = query.Where(s => s.ZoneId == zoneId);
        }
        return await query.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ZoneSettings>> GetZoneSettingsById(string id)
    {
        var settings = await _context.ZoneSettings.Include(s => s.Zone).FirstOrDefaultAsync(s => s.Id == id);
        if (settings == null) return NotFound();
        return settings;
    }

    [HttpPost]
    public async Task<ActionResult<ZoneSettings>> CreateZoneSettings(ZoneSettings settings)
    {
        _context.ZoneSettings.Add(settings);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetZoneSettingsById), new { id = settings.Id }, settings);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateZoneSettings(string id, ZoneSettings settings)
    {
        if (id != settings.Id) return BadRequest("ID mismatch");

        _context.Entry(settings).State = EntityState.Modified;
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.ZoneSettings.Any(e => e.Id == id)) return NotFound();
            throw;
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteZoneSettings(string id)
    {
        var settings = await _context.ZoneSettings.FindAsync(id);
        if (settings == null) return NotFound();

        _context.ZoneSettings.Remove(settings);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
