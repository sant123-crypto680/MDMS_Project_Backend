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
[HttpGet]
public async Task<ActionResult<IEnumerable<Meter>>> GetMeters(
    [FromQuery] string userId,
    [FromQuery] string zoneId)
{
    var meters = await _context.Meters
        .Where(m => m.OwnerId == userId && m.ZoneId == zoneId)
        .ToListAsync();

    return Ok(meters);
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
