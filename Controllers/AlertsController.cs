using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MDMS_PRoject_Backend.Data;
using MDMS_PRoject_Backend.Models;

namespace MDMS_PRoject_Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AlertsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public AlertsController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Alert>>> GetAlerts(
        [FromQuery] string? zoneId,
        [FromQuery] string? meterId,
        [FromQuery] string? severity,
        [FromQuery] string? status)
    {
        var query = _context.Alerts.Include(a => a.Zone).Include(a => a.Meter).AsQueryable();

        if (!string.IsNullOrEmpty(zoneId))
        {
            query = query.Where(a => a.ZoneId == zoneId);
        }
        if (!string.IsNullOrEmpty(meterId))
        {
            query = query.Where(a => a.MeterId == meterId);
        }
        if (!string.IsNullOrEmpty(severity))
        {
            query = query.Where(a => a.Severity.ToLower() == severity.ToLower());
        }
        if (!string.IsNullOrEmpty(status))
        {
            query = query.Where(a => a.Status.ToLower() == status.ToLower());
        }

        return await query.OrderByDescending(a => a.CreatedAt).ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Alert>> GetAlert(string id)
    {
        var alert = await _context.Alerts
            .Include(a => a.Zone)
            .Include(a => a.Meter)
            .FirstOrDefaultAsync(a => a.Id == id);

        if (alert == null) return NotFound();
        return alert;
    }

    [HttpPost]
    public async Task<ActionResult<Alert>> CreateAlert(Alert alert)
    {
        _context.Alerts.Add(alert);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetAlert), new { id = alert.Id }, alert);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAlert(string id, Alert alert)
    {
        if (id != alert.Id) return BadRequest("ID mismatch");

        _context.Entry(alert).State = EntityState.Modified;
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.Alerts.Any(e => e.Id == id)) return NotFound();
            throw;
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAlert(string id)
    {
        var alert = await _context.Alerts.FindAsync(id);
        if (alert == null) return NotFound();

        _context.Alerts.Remove(alert);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
