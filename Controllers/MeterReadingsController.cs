using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MDMS_PRoject_Backend.Data;
using MDMS_PRoject_Backend.Models;

namespace MDMS_PRoject_Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MeterReadingsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public MeterReadingsController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<MeterReading>>> GetMeterReadings(
        [FromQuery] string? meterId,
        [FromQuery] string? granularity,
        [FromQuery] DateTime? from,
        [FromQuery] DateTime? to)
    {
        var query = _context.MeterReadings.AsQueryable();

        if (!string.IsNullOrEmpty(meterId))
        {
            query = query.Where(r => r.MeterId == meterId);
        }
        if (!string.IsNullOrEmpty(granularity))
        {
            query = query.Where(r => r.Granularity.ToLower() == granularity.ToLower());
        }
        if (from.HasValue)
        {
            query = query.Where(r => r.Timestamp >= from.Value);
        }
        if (to.HasValue)
        {
            query = query.Where(r => r.Timestamp <= to.Value);
        }

        return await query.OrderByDescending(r => r.Timestamp).ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<MeterReading>> GetMeterReading(string id)
    {
        var reading = await _context.MeterReadings.FindAsync(id);
        if (reading == null) return NotFound();
        return reading;
    }

    [HttpPost]
    public async Task<ActionResult<MeterReading>> CreateMeterReading(MeterReading reading)
    {
        _context.MeterReadings.Add(reading);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetMeterReading), new { id = reading.Id }, reading);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateMeterReading(string id, MeterReading reading)
    {
        if (id != reading.Id) return BadRequest("ID mismatch");

        _context.Entry(reading).State = EntityState.Modified;
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.MeterReadings.Any(e => e.Id == id)) return NotFound();
            throw;
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMeterReading(string id)
    {
        var reading = await _context.MeterReadings.FindAsync(id);
        if (reading == null) return NotFound();

        _context.MeterReadings.Remove(reading);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
