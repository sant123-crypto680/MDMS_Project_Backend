namespace MDMS_PRoject_Backend.Models.DTOs;

public class ZoneAdminDashboardDto
{
    public string ZoneId { get; set; } = string.Empty;
    public string ZoneName { get; set; } = string.Empty;
    public string AdminName { get; set; } = string.Empty;

    // Meter Health (StatCard 1 & 2)
    public ZoneMeterHealthDto MeterHealth { get; set; } = new();

    // Customer & Usage Stats (StatCard 3 & 4)
    public int TotalCustomers { get; set; }
    public double TotalPowerConsumptionKwh { get; set; }
    public double AverageConsumptionPerCustomerKwh { get; set; }

    // Revenue & Billing (StatCard 5 & 6)
    public ZoneFinancialsDto Financials { get; set; } = new();

    // Alerts (StatCard 7)
    public ZoneAlertsSummaryDto AlertsSummary { get; set; } = new();

    // Consumption Trend Graph (8)
    public ConsumptionGraphDto? ConsumptionTrend { get; set; }
}

public class ZoneMeterHealthDto
{
    public int TotalMeters { get; set; }
    public int ActiveMeters { get; set; }
    public int InactiveMeters { get; set; }
    public double HealthPercentage => TotalMeters > 0 ? Math.Round(((double)ActiveMeters / TotalMeters) * 100, 1) : 100.0;
}

public class ZoneFinancialsDto
{
    public decimal TotalBilledRevenue { get; set; }
    public decimal TotalCollectedRevenue { get; set; }
    public int PendingBillsCount { get; set; }
    public decimal PendingBillsAmount { get; set; }
}

public class ZoneAlertsSummaryDto
{
    public int TotalAlerts { get; set; }
    public int CriticalAlertsCount { get; set; }
    public int WarningAlertsCount { get; set; }
    public int InfoAlertsCount { get; set; }
    public List<RecentAlertDto> RecentAlerts { get; set; } = new();
}
