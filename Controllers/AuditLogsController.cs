using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MDMS_PRoject_Backend.Data;
using MDMS_PRoject_Backend.Models;

namespace MDMS_PRoject_Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuditLogsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public AuditLogsController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<AuditLog>>> GetAuditLogs(
        [FromQuery] string? userId,
        [FromQuery] string? action,
        [FromQuery] string? status)
    {
        var query = _context.AuditLogs.AsQueryable();

        if (!string.IsNullOrEmpty(userId))
        {
            query = query.Where(a => a.UserId == userId);
        }
        if (!string.IsNullOrEmpty(action))
        {
            query = query.Where(a => a.Action.ToLower().Contains(action.ToLower()));
        }
        if (!string.IsNullOrEmpty(status))
        {
            query = query.Where(a => a.Status.ToLower() == status.ToLower());
        }

        return await query.OrderByDescending(a => a.Timestamp).ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<AuditLog>> GetAuditLog(string id)
    {
        var log = await _context.AuditLogs.FindAsync(id);
        if (log == null) return NotFound();
        return log;
    }

    [HttpPost]
    public async Task<ActionResult<AuditLog>> CreateAuditLog(AuditLog log)
    {
        _context.AuditLogs.Add(log);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetAuditLog), new { id = log.Id }, log);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAuditLog(string id)
    {
        var log = await _context.AuditLogs.FindAsync(id);
        if (log == null) return NotFound();

        _context.AuditLogs.Remove(log);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
