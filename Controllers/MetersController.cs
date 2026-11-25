using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MDMS_PRoject_Backend.Data;
using MDMS_PRoject_Backend.Models;
using MDMS_PRoject_Backend.Models.Common;

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

    // GET /api/Meters?ownerId=&zoneId=&status=&isPrimary=&sortBy=installed_desc&page=1&pageSize=10
    [HttpGet]
    public async Task<IActionResult> GetMeters(
        [FromQuery] string? ownerId,
        [FromQuery] string? zoneId,
        [FromQuery] string? status,
        [FromQuery] bool? isPrimary,
        [FromQuery] string? sortBy = "installed_desc",
        [FromQuery] int? page = null,
        [FromQuery] int pageSize = 10)
    {
        var query = _context.Meters
            .Include(m => m.Owner)
            .Include(m => m.Zone)
            .Include(m => m.Tariff)
            .AsQueryable();

        if (!string.IsNullOrEmpty(ownerId))  query = query.Where(m => m.OwnerId == ownerId);
        if (!string.IsNullOrEmpty(zoneId))   query = query.Where(m => m.ZoneId == zoneId);
        if (!string.IsNullOrEmpty(status))   query = query.Where(m => m.Status.ToLower() == status.ToLower());
        if (isPrimary.HasValue)              query = query.Where(m => m.IsPrimary == isPrimary.Value);

        // ── Sorting ──
        query = sortBy switch
        {
            "installed_asc"  => query.OrderBy(m => m.InstalledOn),
            "reading_desc"   => query.OrderByDescending(m => m.LastReading),
            "reading_asc"    => query.OrderBy(m => m.LastReading),
            _                => query.OrderByDescending(m => m.InstalledOn) // installed_desc (default)
        };

        // ── Pagination (dual-mode) ──
        if (page.HasValue)
        {
            int pg   = Math.Max(1, page.Value);
            int size = Math.Clamp(pageSize, 1, 100);
            int total = await query.CountAsync();
            var items = await query.Skip((pg - 1) * size).Take(size).ToListAsync();
            return Ok(new PagedResult<Meter> { Page = pg, PageSize = size, TotalCount = total, Items = items });
        }

        return Ok(await query.ToListAsync());
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
