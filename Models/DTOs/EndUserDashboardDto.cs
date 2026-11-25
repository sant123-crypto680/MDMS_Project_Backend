using MDMS_PRoject_Backend.Models;

namespace MDMS_PRoject_Backend.Models.DTOs;

public class EndUserDashboardDto
{
    public string UserId { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string ZoneId { get; set; } = string.Empty;
    public string ZoneName { get; set; } = string.Empty;
    public int TotalMeters { get; set; }
    
    // Primary Meter Overview
    public MeterOverviewDto? PrimaryMeter { get; set; }

    // Current Month Bill Overview (StatCards 2, 3, 4)
    public CurrentBillSummaryDto? CurrentMonthBill { get; set; }
    public decimal OutstandingBalance { get; set; }
    public LastPaymentSummaryDto? LastPayment { get; set; }

    // Consumption Graph (Day/Month Usage)
    public ConsumptionGraphDto? ConsumptionGraph { get; set; }

    // Recent Tables
    public List<RecentBillDto> RecentBills { get; set; } = new();
    public List<RecentAlertDto> RecentAlerts { get; set; } = new();
}

public class MeterOverviewDto
{
    public string Id { get; set; } = string.Empty;
    public string TariffCategory { get; set; } = string.Empty;
    public double LastReading { get; set; }
    public DateTime? LastReadingAt { get; set; }
    public string Status { get; set; } = "Active";
}

public class CurrentBillSummaryDto
{
    public string BillId { get; set; } = string.Empty;
    public string BillingMonth { get; set; } = string.Empty;
    public double ConsumptionUnits { get; set; }
    public decimal Amount { get; set; }
    public DateTime DueDate { get; set; }
    public string Status { get; set; } = "Pending";
}

public class LastPaymentSummaryDto
{
    public string PaymentId { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string Method { get; set; } = string.Empty;
    public DateTime PaidOn { get; set; }
}

public class ConsumptionGraphDto
{
    public double Total { get; set; }
    public double Average { get; set; }
    public double Max { get; set; }
    public double Min { get; set; }
    public List<string> Labels { get; set; } = new();
    public List<double> Current { get; set; } = new();
    public List<double> Previous { get; set; } = new();
}

public class RecentBillDto
{
    public string Id { get; set; } = string.Empty;
    public string BillingMonth { get; set; } = string.Empty;
    public double TotalUnits { get; set; }
    public decimal Amount { get; set; }
    public DateTime DueDate { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime? PaidOn { get; set; }
}

public class RecentAlertDto
{
    public string Id { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string Severity { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}
