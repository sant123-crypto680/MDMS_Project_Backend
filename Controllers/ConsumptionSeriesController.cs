using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MDMS_PRoject_Backend.Data;
using MDMS_PRoject_Backend.Models;

namespace MDMS_PRoject_Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ConsumptionSeriesController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public ConsumptionSeriesController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ConsumptionSeries>>> GetConsumptionSeries(
        [FromQuery] string? scope,
        [FromQuery] string? scopeId,
        [FromQuery] string? range)
    {
        var query = _context.ConsumptionSeries.AsQueryable();

        if (!string.IsNullOrEmpty(scope))
        {
            query = query.Where(c => c.Scope.ToLower() == scope.ToLower());
        }
        if (!string.IsNullOrEmpty(scopeId))
        {
            query = query.Where(c => c.ScopeId == scopeId);
        }
        if (!string.IsNullOrEmpty(range))
        {
            query = query.Where(c => c.Range.ToLower() == range.ToLower());
        }

        return await query.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ConsumptionSeries>> GetConsumptionSeriesById(string id)
    {
        var series = await _context.ConsumptionSeries.FindAsync(id);
        if (series == null) return NotFound();
        return series;
    }

    [HttpPost]
    public async Task<ActionResult<ConsumptionSeries>> CreateConsumptionSeries(ConsumptionSeries series)
    {
        _context.ConsumptionSeries.Add(series);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetConsumptionSeriesById), new { id = series.Id }, series);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateConsumptionSeries(string id, ConsumptionSeries series)
    {
        if (id != series.Id) return BadRequest("ID mismatch");

        _context.Entry(series).State = EntityState.Modified;
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.ConsumptionSeries.Any(e => e.Id == id)) return NotFound();
            throw;
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteConsumptionSeries(string id)
    {
        var series = await _context.ConsumptionSeries.FindAsync(id);
        if (series == null) return NotFound();

        _context.ConsumptionSeries.Remove(series);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
