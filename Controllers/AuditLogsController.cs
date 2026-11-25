using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MDMS_PRoject_Backend.Data;
using MDMS_PRoject_Backend.Models;
using MDMS_PRoject_Backend.Models.Common;

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

    // GET /api/AuditLogs?userId=&action=&status=&page=1&pageSize=15
    [HttpGet]
    public async Task<IActionResult> GetAuditLogs(
        [FromQuery] string? userId,
        [FromQuery] string? action,
        [FromQuery] string? status,
        [FromQuery] int? page = null,
        [FromQuery] int pageSize = 15)
    {
        var query = _context.AuditLogs.AsQueryable();

        if (!string.IsNullOrEmpty(userId)) query = query.Where(a => a.UserId == userId);
        if (!string.IsNullOrEmpty(action)) query = query.Where(a => a.Action.ToLower().Contains(action.ToLower()));
        if (!string.IsNullOrEmpty(status)) query = query.Where(a => a.Status.ToLower() == status.ToLower());

        query = query.OrderByDescending(a => a.Timestamp);

        // ── Pagination (dual-mode, default pageSize=15 for audit trail) ──
        if (page.HasValue)
        {
            int pg   = Math.Max(1, page.Value);
            int size = Math.Clamp(pageSize, 1, 100);
            int total = await query.CountAsync();
            var items = await query.Skip((pg - 1) * size).Take(size).ToListAsync();
            return Ok(new PagedResult<AuditLog> { Page = pg, PageSize = size, TotalCount = total, Items = items });
        }

        return Ok(await query.ToListAsync());
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
