using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MDMS_PRoject_Backend.Data;
using MDMS_PRoject_Backend.Models;

namespace MDMS_PRoject_Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserStatusStatsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public UserStatusStatsController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserStatusStats>>> GetUserStatusStats(
        [FromQuery] int? year,
        [FromQuery] string? group)
    {
        var query = _context.UserStatusStats.AsQueryable();

        if (year.HasValue)
        {
            query = query.Where(s => s.Year == year.Value);
        }
        if (!string.IsNullOrEmpty(group))
        {
            query = query.Where(s => s.Group.ToLower() == group.ToLower());
        }

        return await query.OrderBy(s => s.Year).ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<UserStatusStats>> GetUserStatusStat(string id)
    {
        var stat = await _context.UserStatusStats.FindAsync(id);
        if (stat == null) return NotFound();
        return stat;
    }

    [HttpPost]
    public async Task<ActionResult<UserStatusStats>> CreateUserStatusStat(UserStatusStats stat)
    {
        _context.UserStatusStats.Add(stat);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetUserStatusStat), new { id = stat.Id }, stat);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUserStatusStat(string id, UserStatusStats stat)
    {
        if (id != stat.Id) return BadRequest("ID mismatch");

        _context.Entry(stat).State = EntityState.Modified;
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.UserStatusStats.Any(e => e.Id == id)) return NotFound();
            throw;
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUserStatusStat(string id)
    {
        var stat = await _context.UserStatusStats.FindAsync(id);
        if (stat == null) return NotFound();

        _context.UserStatusStats.Remove(stat);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
