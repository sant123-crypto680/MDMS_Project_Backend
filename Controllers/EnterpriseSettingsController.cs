using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MDMS_PRoject_Backend.Data;
using MDMS_PRoject_Backend.Models;

namespace MDMS_PRoject_Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EnterpriseSettingsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public EnterpriseSettingsController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<EnterpriseSettings>> GetEnterpriseSettings()
    {
        var settings = await _context.EnterpriseSettings.FirstOrDefaultAsync();
        if (settings == null) return NotFound();
        return settings;
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateEnterpriseSettings(string id, EnterpriseSettings settings)
    {
        if (id != settings.Id) return BadRequest("ID mismatch");

        _context.Entry(settings).State = EntityState.Modified;
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.EnterpriseSettings.Any(e => e.Id == id)) return NotFound();
            throw;
        }

        return NoContent();
    }
}
