using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MDMS_PRoject_Backend.Data;
using MDMS_PRoject_Backend.Models;

namespace MDMS_PRoject_Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BillsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public BillsController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Bill>>> GetBills(
        [FromQuery] string? userId,
        [FromQuery] string? meterId,
        [FromQuery] string? zoneId,
        [FromQuery] string? billingMonth,
        [FromQuery] string? status)
    {
        var query = _context.Bills
            .Include(b => b.User)
            .Include(b => b.Meter)
            .Include(b => b.Zone)
            .AsQueryable();

        if (!string.IsNullOrEmpty(userId))
        {
            query = query.Where(b => b.UserId == userId);
        }
        if (!string.IsNullOrEmpty(meterId))
        {
            query = query.Where(b => b.MeterId == meterId);
        }
        if (!string.IsNullOrEmpty(zoneId))
        {
            query = query.Where(b => b.ZoneId == zoneId);
        }
        if (!string.IsNullOrEmpty(billingMonth))
        {
            query = query.Where(b => b.BillingMonth == billingMonth);
        }
        if (!string.IsNullOrEmpty(status))
        {
            query = query.Where(b => b.Status.ToLower() == status.ToLower());
        }

        return await query.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Bill>> GetBill(string id)
    {
        var bill = await _context.Bills
            .Include(b => b.User)
            .Include(b => b.Meter)
            .Include(b => b.Zone)
            .Include(b => b.BillDays)
            .Include(b => b.Payments)
            .FirstOrDefaultAsync(b => b.Id == id);

        if (bill == null) return NotFound();
        return bill;
    }

    [HttpPost]
    public async Task<ActionResult<Bill>> CreateBill(Bill bill)
    {
        _context.Bills.Add(bill);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetBill), new { id = bill.Id }, bill);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateBill(string id, Bill bill)
    {
        if (id != bill.Id) return BadRequest("ID mismatch");

        _context.Entry(bill).State = EntityState.Modified;
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.Bills.Any(e => e.Id == id)) return NotFound();
            throw;
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBill(string id)
    {
        var bill = await _context.Bills.FindAsync(id);
        if (bill == null) return NotFound();

        _context.Bills.Remove(bill);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
