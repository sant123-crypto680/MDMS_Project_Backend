using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MDMS_PRoject_Backend.Data;
using MDMS_PRoject_Backend.Models;
using MDMS_PRoject_Backend.Models.Common;

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

    // GET /api/Bills?userId=&meterId=&zoneId=&billingMonth=&status=&sortBy=date_desc&page=1&pageSize=10
    // If ?page is omitted → returns full list (backward compatible).
    // If ?page is provided → returns PagedResult<Bill>.
    [HttpGet]
    public async Task<IActionResult> GetBills(
        [FromQuery] string? userId,
        [FromQuery] string? meterId,
        [FromQuery] string? zoneId,
        [FromQuery] string? billingMonth,
        [FromQuery] string? status,
        [FromQuery] string? sortBy = "date_desc",
        [FromQuery] int? page = null,
        [FromQuery] int pageSize = 10)
    {
        var query = _context.Bills
            .Include(b => b.User)
            .Include(b => b.Meter)
            .Include(b => b.Zone)
            .AsQueryable();

        if (!string.IsNullOrEmpty(userId))        query = query.Where(b => b.UserId == userId);
        if (!string.IsNullOrEmpty(meterId))       query = query.Where(b => b.MeterId == meterId);
        if (!string.IsNullOrEmpty(zoneId))        query = query.Where(b => b.ZoneId == zoneId);
        if (!string.IsNullOrEmpty(billingMonth))  query = query.Where(b => b.BillingMonth == billingMonth);
        if (!string.IsNullOrEmpty(status))        query = query.Where(b => b.Status.ToLower() == status.ToLower());

        // ── Sorting ──
        query = sortBy switch
        {
            "date_asc"    => query.OrderBy(b => b.BillingMonth),
            "amount_desc" => query.OrderByDescending(b => b.Amount),
            "amount_asc"  => query.OrderBy(b => b.Amount),
            _             => query.OrderByDescending(b => b.BillingMonth)   // date_desc (default)
        };

        // ── Pagination (dual-mode) ──
        if (page.HasValue)
        {
            int pg   = Math.Max(1, page.Value);
            int size = Math.Clamp(pageSize, 1, 100);
            int total = await query.CountAsync();
            var items = await query.Skip((pg - 1) * size).Take(size).ToListAsync();
            return Ok(new PagedResult<Bill> { Page = pg, PageSize = size, TotalCount = total, Items = items });
        }

        return Ok(await query.ToListAsync());
    }

    // GET /api/Bills/user-summary/{userId}  — quick overview for end-user dashboard cards
    [HttpGet("user-summary/{userId}")]
    public async Task<IActionResult> GetUserBillSummary(string userId)
    {
        var latestBill = await _context.Bills
            .Where(b => b.UserId == userId)
            .OrderByDescending(b => b.BillingMonth)
            .FirstOrDefaultAsync();

        if (latestBill == null)
            return NotFound(new { message = $"No bills found for user '{userId}'." });

        var primaryMeter = await _context.Meters
            .Where(m => m.OwnerId == userId && m.IsPrimary)
            .FirstOrDefaultAsync()
            ?? await _context.Meters.Where(m => m.OwnerId == userId).FirstOrDefaultAsync();

        var outstandingBalance = await _context.Bills
            .Where(b => b.UserId == userId && b.Status != "Paid")
            .SumAsync(b => b.Amount);

        var lastPayment = await _context.Payments
            .Where(p => p.UserId == userId && p.Status == "Success")
            .OrderByDescending(p => p.PaidOn)
            .FirstOrDefaultAsync();

        return Ok(new
        {
            UserId                 = userId,
            PrimaryMeterId         = primaryMeter?.Id,
            CurrentConsumptionKwh  = latestBill.TotalUnits,
            ThisMonthBillAmount    = latestBill.Amount,
            DueDate                = latestBill.DueDate,
            BillStatus             = latestBill.Status,
            OutstandingBalance     = outstandingBalance,
            LastPaymentAmount      = lastPayment?.AmountPaid,
            LastPaymentDate        = lastPayment?.PaidOn
        });
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
