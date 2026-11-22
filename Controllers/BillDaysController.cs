using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MDMS_PRoject_Backend.Data;
using MDMS_PRoject_Backend.Models;

namespace MDMS_PRoject_Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BillDaysController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public BillDaysController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<BillDay>>> GetBillDays([FromQuery] string? billId)
    {
        var query = _context.BillDays.AsQueryable();
        if (!string.IsNullOrEmpty(billId))
        {
            query = query.Where(d => d.BillId == billId);
        }
        return await query.OrderBy(d => d.Date).ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<BillDay>> GetBillDay(string id)
    {
        var day = await _context.BillDays.FindAsync(id);
        if (day == null) return NotFound();
        return day;
    }

    [HttpPost]
    public async Task<ActionResult<BillDay>> CreateBillDay(BillDay billDay)
    {
        _context.BillDays.Add(billDay);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetBillDay), new { id = billDay.Id }, billDay);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateBillDay(string id, BillDay billDay)
    {
        if (id != billDay.Id) return BadRequest("ID mismatch");

        _context.Entry(billDay).State = EntityState.Modified;
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.BillDays.Any(e => e.Id == id)) return NotFound();
            throw;
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBillDay(string id)
    {
        var billDay = await _context.BillDays.FindAsync(id);
        if (billDay == null) return NotFound();

        _context.BillDays.Remove(billDay);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
