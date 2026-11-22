using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MDMS_PRoject_Backend.Data;
using MDMS_PRoject_Backend.Models;

namespace MDMS_PRoject_Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PaymentsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public PaymentsController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Payment>>> GetPayments(
        [FromQuery] string? userId,
        [FromQuery] string? billId,
        [FromQuery] string? status)
    {
        var query = _context.Payments.Include(p => p.Bill).Include(p => p.User).AsQueryable();

        if (!string.IsNullOrEmpty(userId))
        {
            query = query.Where(p => p.UserId == userId);
        }
        if (!string.IsNullOrEmpty(billId))
        {
            query = query.Where(p => p.BillId == billId);
        }
        if (!string.IsNullOrEmpty(status))
        {
            query = query.Where(p => p.Status.ToLower() == status.ToLower());
        }

        return await query.OrderByDescending(p => p.PaidOn).ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Payment>> GetPayment(string id)
    {
        var payment = await _context.Payments
            .Include(p => p.Bill)
            .Include(p => p.User)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (payment == null) return NotFound();
        return payment;
    }

    [HttpPost]
    public async Task<ActionResult<Payment>> CreatePayment(Payment payment)
    {
        _context.Payments.Add(payment);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetPayment), new { id = payment.Id }, payment);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePayment(string id, Payment payment)
    {
        if (id != payment.Id) return BadRequest("ID mismatch");

        _context.Entry(payment).State = EntityState.Modified;
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.Payments.Any(e => e.Id == id)) return NotFound();
            throw;
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePayment(string id)
    {
        var payment = await _context.Payments.FindAsync(id);
        if (payment == null) return NotFound();

        _context.Payments.Remove(payment);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
