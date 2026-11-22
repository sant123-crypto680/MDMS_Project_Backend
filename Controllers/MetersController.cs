using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MDMS_PRoject_Backend.Data;
using MDMS_PRoject_Backend.Models;

namespace MDMS_PRoject_Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MetersController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public MetersController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Meter>>> GetMeters(
        [FromQuery] string? ownerId,
        [FromQuery] string? zoneId,
        [FromQuery] string? status,
        [FromQuery] bool? isPrimary)
    {
        var query = _context.Meters
            .Include(m => m.Owner)
            .Include(m => m.Zone)
            .Include(m => m.Tariff)
            .AsQueryable();

        if (!string.IsNullOrEmpty(ownerId))
        {
            query = query.Where(m => m.OwnerId == ownerId);
        }
        if (!string.IsNullOrEmpty(zoneId))
        {
            query = query.Where(m => m.ZoneId == zoneId);
        }
        if (!string.IsNullOrEmpty(status))
        {
            query = query.Where(m => m.Status.ToLower() == status.ToLower());
        }
        if (isPrimary.HasValue)
        {
            query = query.Where(m => m.IsPrimary == isPrimary.Value);
        }

        return await query.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Meter>> GetMeter(string id)
    {
        var meter = await _context.Meters
            .Include(m => m.Owner)
            .Include(m => m.Zone)
            .Include(m => m.Tariff)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (meter == null) return NotFound();
        return meter;
    }

    [HttpPost]
    public async Task<ActionResult<Meter>> CreateMeter(Meter meter)
    {
        _context.Meters.Add(meter);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetMeter), new { id = meter.Id }, meter);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateMeter(string id, Meter meter)
    {
        if (id != meter.Id) return BadRequest("ID mismatch");

        _context.Entry(meter).State = EntityState.Modified;
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.Meters.Any(e => e.Id == id)) return NotFound();
            throw;
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMeter(string id)
    {
        var meter = await _context.Meters.FindAsync(id);
        if (meter == null) return NotFound();

        _context.Meters.Remove(meter);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
