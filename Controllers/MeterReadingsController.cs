using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MDMS_PRoject_Backend.Data;
using MDMS_PRoject_Backend.Models;
using MDMS_PRoject_Backend.Models.Common;

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

    // GET /api/MeterReadings?meterId=&granularity=&from=&to=&page=1&pageSize=24
    [HttpGet]
    public async Task<IActionResult> GetMeterReadings(
        [FromQuery] string? meterId,
        [FromQuery] string? granularity,
        [FromQuery] DateTime? from,
        [FromQuery] DateTime? to,
        [FromQuery] int? page = null,
        [FromQuery] int pageSize = 24)
    {
        var query = _context.MeterReadings.AsQueryable();

        if (!string.IsNullOrEmpty(meterId))     query = query.Where(r => r.MeterId == meterId);
        if (!string.IsNullOrEmpty(granularity)) query = query.Where(r => r.Granularity.ToLower() == granularity.ToLower());
        if (from.HasValue)                      query = query.Where(r => r.Timestamp >= from.Value);
        if (to.HasValue)                        query = query.Where(r => r.Timestamp <= to.Value);

        query = query.OrderByDescending(r => r.Timestamp);

        // ── Pagination (dual-mode, default pageSize=24 for hourly data) ──
        if (page.HasValue)
        {
            int pg   = Math.Max(1, page.Value);
            int size = Math.Clamp(pageSize, 1, 200);
            int total = await query.CountAsync();
            var items = await query.Skip((pg - 1) * size).Take(size).ToListAsync();
            return Ok(new PagedResult<MeterReading> { Page = pg, PageSize = size, TotalCount = total, Items = items });
        }

        return Ok(await query.ToListAsync());
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
