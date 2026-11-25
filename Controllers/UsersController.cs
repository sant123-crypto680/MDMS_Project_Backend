using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MDMS_PRoject_Backend.Data;
using MDMS_PRoject_Backend.Models;

namespace MDMS_PRoject_Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public UsersController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET /api/Users/demo-users — returns the 3 known demo accounts (fetched live from DB)
    [HttpGet("demo-users")]
    public async Task<ActionResult<IEnumerable<User>>> GetDemoUsers()
    {
        var ids = new[] { "USR-001", "USR-002", "USR-006" };
        var users = await _context.Users
            .Include(u => u.Zone)
            .Include(u => u.Meters)
            .Where(u => ids.Contains(u.Id))
            .OrderBy(u => u.Id)
            .ToListAsync();
        return Ok(users);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<User>>> GetUsers(
        [FromQuery] string? role,
        [FromQuery] string? zoneId,
        [FromQuery] string? status)
    {
        var query = _context.Users.Include(u => u.Zone).AsQueryable();

        if (!string.IsNullOrEmpty(role))
        {
            query = query.Where(u => u.Role.ToLower() == role.ToLower());
        }
        if (!string.IsNullOrEmpty(zoneId))
        {
            query = query.Where(u => u.ZoneId == zoneId);
        }
        if (!string.IsNullOrEmpty(status))
        {
            query = query.Where(u => u.Status.ToLower() == status.ToLower());
        }

        return await query.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<User>> GetUser(string id)
    {
        var user = await _context.Users
            .Include(u => u.Zone)
            .Include(u => u.Meters)
            .FirstOrDefaultAsync(u => u.Id == id);

        if (user == null) return NotFound();
        return user;
    }

    [HttpPost]
    public async Task<ActionResult<User>> CreateUser(User user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser(string id, User user)
    {
        if (id != user.Id) return BadRequest("ID mismatch");

        _context.Entry(user).State = EntityState.Modified;
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.Users.Any(e => e.Id == id)) return NotFound();
            throw;
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(string id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null) return NotFound();

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
