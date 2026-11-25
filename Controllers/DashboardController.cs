using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MDMS_PRoject_Backend.Data;
using MDMS_PRoject_Backend.Models.DTOs;

namespace MDMS_PRoject_Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DashboardController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public DashboardController(ApplicationDbContext context)
    {
        _context = context;
    }

    // ─────────────────────────────────────────────────────────────────────────
    // GET /api/Dashboard/end-user/{userId}
    // Returns full End-User dashboard data in a single roundtrip
    // ─────────────────────────────────────────────────────────────────────────
    [HttpGet("end-user/{userId}")]
    public async Task<ActionResult<EndUserDashboardDto>> GetEndUserDashboard(string userId)
    {
        var user = await _context.Users
            .Include(u => u.Zone)
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null)
            return NotFound(new { message = $"User '{userId}' not found." });

        // ── Meters ──────────────────────────────────────────────────────────
        var meters = await _context.Meters
            .Where(m => m.OwnerId == userId)
            .OrderByDescending(m => m.IsPrimary)
            .ToListAsync();

        var primaryMeter = meters.FirstOrDefault();

        MeterOverviewDto? primaryMeterDto = primaryMeter == null ? null : new MeterOverviewDto
        {
            Id             = primaryMeter.Id,
            TariffCategory = primaryMeter.TariffCategory ?? "Standard",
            LastReading    = primaryMeter.LastReading,
            LastReadingAt  = primaryMeter.LastReadingAt,
            Status         = primaryMeter.Status
        };

        // ── Bills ────────────────────────────────────────────────────────────
        var bills = await _context.Bills
            .Where(b => b.UserId == userId)
            .OrderByDescending(b => b.BillingMonth)
            .Take(6)
            .ToListAsync();

        var latestBill  = bills.FirstOrDefault();
        var outstandingBalance = bills
            .Where(b => b.Status != "Paid")
            .Sum(b => b.Amount);

        CurrentBillSummaryDto? currentBillDto = latestBill == null ? null : new CurrentBillSummaryDto
        {
            BillId            = latestBill.Id,
            BillingMonth      = latestBill.BillingMonth,
            ConsumptionUnits  = latestBill.TotalUnits,
            Amount            = latestBill.Amount,
            DueDate           = latestBill.DueDate,
            Status            = latestBill.Status
        };

        // ── Payments ─────────────────────────────────────────────────────────
        var lastPayment = await _context.Payments
            .Where(p => p.UserId == userId && p.Status == "Success")
            .OrderByDescending(p => p.PaidOn)
            .FirstOrDefaultAsync();

        LastPaymentSummaryDto? lastPaymentDto = lastPayment == null ? null : new LastPaymentSummaryDto
        {
            PaymentId = lastPayment.Id,
            Amount    = lastPayment.AmountPaid,
            Method    = lastPayment.Method,
            PaidOn    = lastPayment.PaidOn
        };

        // ── Consumption Graph ─────────────────────────────────────────────────
        var consumptionSeries = await _context.ConsumptionSeries
            .FirstOrDefaultAsync(cs => cs.Scope == "user" && cs.ScopeId == userId && cs.Range == "month");

        ConsumptionGraphDto? consumptionGraphDto = consumptionSeries == null ? null : new ConsumptionGraphDto
        {
            Total   = consumptionSeries.TotalConsumption,
            Average = consumptionSeries.AverageConsumption,
            Max     = consumptionSeries.MaxUsage,
            Min     = consumptionSeries.MinUsage,
            Labels  = consumptionSeries.Labels,
            Current = consumptionSeries.Current,
            Previous = consumptionSeries.Previous
        };

        // ── Recent Bills (top 5) ──────────────────────────────────────────────
        var recentBills = bills.Take(5).Select(b => new RecentBillDto
        {
            Id           = b.Id,
            BillingMonth = b.BillingMonth,
            TotalUnits   = b.TotalUnits,
            Amount       = b.Amount,
            DueDate      = b.DueDate,
            Status       = b.Status,
            PaidOn       = b.PaidOn
        }).ToList();

        // ── Recent Alerts (top 5 from user's zone) ───────────────────────────
        var recentAlerts = new List<RecentAlertDto>();
        if (user.ZoneId != null)
        {
            recentAlerts = await _context.Alerts
                .Where(a => a.ZoneId == user.ZoneId)
                .OrderByDescending(a => a.CreatedAt)
                .Take(5)
                .Select(a => new RecentAlertDto
                {
                    Id        = a.Id,
                    Type      = a.Type,
                    Severity  = a.Severity,
                    Status    = a.Status,
                    Message   = a.Message,
                    CreatedAt = a.CreatedAt
                })
                .ToListAsync();
        }

        return Ok(new EndUserDashboardDto
        {
            UserId          = user.Id,
            UserName        = user.Name,
            ZoneId          = user.ZoneId ?? "N/A",
            ZoneName        = user.Zone?.Name ?? "N/A",
            TotalMeters     = meters.Count,
            PrimaryMeter    = primaryMeterDto,
            CurrentMonthBill = currentBillDto,
            OutstandingBalance = outstandingBalance,
            LastPayment     = lastPaymentDto,
            ConsumptionGraph = consumptionGraphDto,
            RecentBills     = recentBills,
            RecentAlerts    = recentAlerts
        });
    }

    // ─────────────────────────────────────────────────────────────────────────
    // GET /api/Dashboard/zone/{zoneId}
    // Returns full Zone Admin dashboard data in a single roundtrip
    // ─────────────────────────────────────────────────────────────────────────
    [HttpGet("zone/{zoneId}")]
    public async Task<ActionResult<ZoneAdminDashboardDto>> GetZoneAdminDashboard(string zoneId)
    {
        var zone = await _context.Zones
            .Include(z => z.Admin)
            .FirstOrDefaultAsync(z => z.Id == zoneId);

        if (zone == null)
            return NotFound(new { message = $"Zone '{zoneId}' not found." });

        // ── Meter Health ─────────────────────────────────────────────────────
        var meters = await _context.Meters
            .Where(m => m.ZoneId == zoneId)
            .Select(m => new { m.Status })
            .ToListAsync();

        var totalMeters    = meters.Count;
        var activeMeters   = meters.Count(m => m.Status == "Active");
        var inactiveMeters = totalMeters - activeMeters;

        // ── Customers ────────────────────────────────────────────────────────
        var totalCustomers = await _context.Users
            .CountAsync(u => u.ZoneId == zoneId && u.Role == "end_user");

        // ── Consumption ──────────────────────────────────────────────────────
        var consumptionData = await _context.Bills
            .Where(b => b.ZoneId == zoneId)
            .GroupBy(b => 1)
            .Select(g => new
            {
                TotalUnits = g.Sum(b => b.TotalUnits)
            })
            .FirstOrDefaultAsync();

        double totalPowerKwh = consumptionData?.TotalUnits ?? 0;
        double avgPerCustomer = totalCustomers > 0
            ? Math.Round(totalPowerKwh / totalCustomers, 2)
            : 0;

        // ── Financials ───────────────────────────────────────────────────────
        var financials = await _context.Bills
            .Where(b => b.ZoneId == zoneId)
            .GroupBy(b => 1)
            .Select(g => new
            {
                TotalBilled  = g.Sum(b => b.Amount),
                PendingCount = g.Count(b => b.Status != "Paid"),
                PendingAmt   = g.Where(b => b.Status != "Paid").Sum(b => b.Amount)
            })
            .FirstOrDefaultAsync();

        var collectedRevenue = await _context.Payments
            .Where(p => _context.Bills
                .Where(b => b.ZoneId == zoneId)
                .Select(b => b.Id)
                .Contains(p.BillId) && p.Status == "Success")
            .SumAsync(p => p.AmountPaid);

        // ── Alerts ───────────────────────────────────────────────────────────
        var alertCounts = await _context.Alerts
            .Where(a => a.ZoneId == zoneId)
            .GroupBy(a => a.Severity)
            .Select(g => new { Severity = g.Key, Count = g.Count() })
            .ToListAsync();

        int totalAlerts    = alertCounts.Sum(a => a.Count);
        int criticalAlerts = alertCounts.FirstOrDefault(a => a.Severity == "Critical")?.Count ?? 0;
        int warningAlerts  = alertCounts.FirstOrDefault(a => a.Severity == "Warning")?.Count ?? 0;
        int infoAlerts     = alertCounts.FirstOrDefault(a => a.Severity == "Info")?.Count ?? 0;

        var recentAlerts = await _context.Alerts
            .Where(a => a.ZoneId == zoneId)
            .OrderByDescending(a => a.CreatedAt)
            .Take(5)
            .Select(a => new RecentAlertDto
            {
                Id        = a.Id,
                Type      = a.Type,
                Severity  = a.Severity,
                Status    = a.Status,
                Message   = a.Message,
                CreatedAt = a.CreatedAt
            })
            .ToListAsync();

        // ── Consumption Graph ─────────────────────────────────────────────────
        var consumptionSeries = await _context.ConsumptionSeries
            .FirstOrDefaultAsync(cs => cs.Scope == "zone" && cs.ScopeId == zoneId && cs.Range == "month");

        ConsumptionGraphDto? trendDto = consumptionSeries == null ? null : new ConsumptionGraphDto
        {
            Total    = consumptionSeries.TotalConsumption,
            Average  = consumptionSeries.AverageConsumption,
            Max      = consumptionSeries.MaxUsage,
            Min      = consumptionSeries.MinUsage,
            Labels   = consumptionSeries.Labels,
            Current  = consumptionSeries.Current,
            Previous = consumptionSeries.Previous
        };

        return Ok(new ZoneAdminDashboardDto
        {
            ZoneId    = zone.Id,
            ZoneName  = zone.Name,
            AdminName = zone.Admin?.Name ?? "N/A",
            MeterHealth = new ZoneMeterHealthDto
            {
                TotalMeters    = totalMeters,
                ActiveMeters   = activeMeters,
                InactiveMeters = inactiveMeters
            },
            TotalCustomers                  = totalCustomers,
            TotalPowerConsumptionKwh        = Math.Round(totalPowerKwh, 2),
            AverageConsumptionPerCustomerKwh = avgPerCustomer,
            Financials = new ZoneFinancialsDto
            {
                TotalBilledRevenue    = financials?.TotalBilled ?? 0,
                TotalCollectedRevenue = collectedRevenue,
                PendingBillsCount     = financials?.PendingCount ?? 0,
                PendingBillsAmount    = financials?.PendingAmt ?? 0
            },
            AlertsSummary = new ZoneAlertsSummaryDto
            {
                TotalAlerts        = totalAlerts,
                CriticalAlertsCount = criticalAlerts,
                WarningAlertsCount  = warningAlerts,
                InfoAlertsCount     = infoAlerts,
                RecentAlerts        = recentAlerts
            },
            ConsumptionTrend = trendDto
        });
    }

    // ─────────────────────────────────────────────────────────────────────────
    // GET /api/Dashboard/enterprise
    // Returns full Enterprise dashboard data in a single roundtrip
    // ─────────────────────────────────────────────────────────────────────────
    [HttpGet("enterprise")]
    public async Task<ActionResult<EnterpriseDashboardDto>> GetEnterpriseDashboard()
    {
        // ── Top-level counts ─────────────────────────────────────────────────
        var totalZones     = await _context.Zones.CountAsync();
        var totalMeters    = await _context.Meters.CountAsync();
        var totalCustomers = await _context.Users.CountAsync(u => u.Role == "end_user");

        // ── Billing & Revenue ────────────────────────────────────────────────
        var billSummary = await _context.Bills
            .GroupBy(b => 1)
            .Select(g => new
            {
                TotalConsumption = g.Sum(b => b.TotalUnits),
                TotalRevenue     = g.Sum(b => b.Amount),
                PendingPayments  = g.Where(b => b.Status != "Paid").Sum(b => b.Amount)
            })
            .FirstOrDefaultAsync();

        // ── Critical Alerts ──────────────────────────────────────────────────
        var criticalAlertCount = await _context.Alerts
            .CountAsync(a => a.Severity == "Critical");

        // ── Zone-wise Performance ─────────────────────────────────────────────
        var zones = await _context.Zones.Select(z => new { z.Id, z.Name }).ToListAsync();

        var zoneMeters = await _context.Meters
            .GroupBy(m => m.ZoneId)
            .Select(g => new { ZoneId = g.Key, Count = g.Count() })
            .ToListAsync();

        var zoneCustomers = await _context.Users
            .Where(u => u.Role == "end_user" && u.ZoneId != null)
            .GroupBy(u => u.ZoneId!)
            .Select(g => new { ZoneId = g.Key, Count = g.Count() })
            .ToListAsync();

        var zoneConsumption = await _context.Bills
            .GroupBy(b => b.ZoneId)
            .Select(g => new
            {
                ZoneId       = g.Key,
                Consumption  = g.Sum(b => b.TotalUnits),
                Revenue      = g.Sum(b => b.Amount),
                PendingAmt   = g.Where(b => b.Status != "Paid").Sum(b => b.Amount)
            })
            .ToListAsync();

        var zoneWisePerformance = zones.Select(z => new ZonePerformanceDto
        {
            ZoneId        = z.Id,
            ZoneName      = z.Name,
            MetersCount   = zoneMeters.FirstOrDefault(m => m.ZoneId == z.Id)?.Count ?? 0,
            CustomersCount = zoneCustomers.FirstOrDefault(c => c.ZoneId == z.Id)?.Count ?? 0,
            ConsumptionKwh = Math.Round(zoneConsumption.FirstOrDefault(c => c.ZoneId == z.Id)?.Consumption ?? 0, 2),
            RevenueInr    = zoneConsumption.FirstOrDefault(c => c.ZoneId == z.Id)?.Revenue ?? 0,
            PendingAmountInr = zoneConsumption.FirstOrDefault(c => c.ZoneId == z.Id)?.PendingAmt ?? 0
        }).ToList();

        // ── Recent Critical Alerts (top 5) ────────────────────────────────────
        var recentCriticalAlerts = await _context.Alerts
            .Include(a => a.Zone)
            .Where(a => a.Severity == "Critical")
            .OrderByDescending(a => a.CreatedAt)
            .Take(5)
            .Select(a => new EnterpriseCriticalAlertDto
            {
                Id       = a.Id,
                ZoneId   = a.ZoneId,
                ZoneName = a.Zone != null ? a.Zone.Name : a.ZoneName ?? "Unknown",
                MeterId  = a.MeterId,
                Type     = a.Type,
                Severity = a.Severity,
                Status   = a.Status,
                Message  = a.Message,
                CreatedAt = a.CreatedAt
            })
            .ToListAsync();

        return Ok(new EnterpriseDashboardDto
        {
            TotalZones                   = totalZones,
            TotalMeters                  = totalMeters,
            TotalCustomers               = totalCustomers,
            TotalElectricityConsumptionKwh = Math.Round(billSummary?.TotalConsumption ?? 0, 2),
            TotalRevenueInr              = billSummary?.TotalRevenue ?? 0,
            TotalPendingPaymentsInr      = billSummary?.PendingPayments ?? 0,
            CriticalAlertsCount          = criticalAlertCount,
            ZoneWisePerformance          = zoneWisePerformance,
            RecentCriticalAlerts         = recentCriticalAlerts
        });
    }
}
