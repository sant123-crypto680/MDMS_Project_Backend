namespace MDMS_PRoject_Backend.Models.DTOs;

public class EnterpriseDashboardDto
{
    // Overall Company Picture (Top StatCards 1 to 6)
    public int TotalZones { get; set; }
    public int TotalMeters { get; set; }
    public int TotalCustomers { get; set; }
    public double TotalElectricityConsumptionKwh { get; set; }
    public decimal TotalRevenueInr { get; set; }
    public decimal TotalPendingPaymentsInr { get; set; }
    public int CriticalAlertsCount { get; set; }

    // Zone-wise Performance Breakdown (Table / Bar Chart)
    public List<ZonePerformanceDto> ZoneWisePerformance { get; set; } = new();

    // Critical Alerts
    public List<EnterpriseCriticalAlertDto> RecentCriticalAlerts { get; set; } = new();
}

public class ZonePerformanceDto
{
    public string ZoneId { get; set; } = string.Empty;
    public string ZoneName { get; set; } = string.Empty;
    public int MetersCount { get; set; }
    public int CustomersCount { get; set; }
    public double ConsumptionKwh { get; set; }
    public decimal RevenueInr { get; set; }
    public decimal PendingAmountInr { get; set; }
}

public class EnterpriseCriticalAlertDto
{
    public string Id { get; set; } = string.Empty;
    public string ZoneId { get; set; } = string.Empty;
    public string ZoneName { get; set; } = string.Empty;
    public string? MeterId { get; set; }
    public string Type { get; set; } = string.Empty;
    public string Severity { get; set; } = "Critical";
    public string Status { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}
