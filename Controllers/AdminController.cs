using Microsoft.AspNetCore.Mvc;
using MDMS_PRoject_Backend.Data;

namespace MDMS_PRoject_Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AdminController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public AdminController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpPost("seed-database")]
    public IActionResult SeedDatabase([FromQuery] bool forceReset = true)
    {
        if (forceReset)
        {
            DbInitializer.ResetAndSeed(_context);
        }
        else
        {
            DbInitializer.Initialize(_context);
        }

        return Ok(new
        {
            message = "Database seeded successfully with the full Phase 1 dataset.",
            zones = _context.Zones.Count(),
            users = _context.Users.Count(),
            meters = _context.Meters.Count(),
            bills = _context.Bills.Count(),
            billDays = _context.BillDays.Count(),
            payments = _context.Payments.Count(),
            meterReadings = _context.MeterReadings.Count(),
            consumptionSeries = _context.ConsumptionSeries.Count(),
            alerts = _context.Alerts.Count(),
            notifications = _context.Notifications.Count(),
            auditLogs = _context.AuditLogs.Count(),
            zoneSettings = _context.ZoneSettings.Count(),
            enterpriseSettings = _context.EnterpriseSettings.Count(),
            userStatusStats = _context.UserStatusStats.Count()
        });
    }
}
