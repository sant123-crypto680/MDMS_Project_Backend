using MDMS_PRoject_Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace MDMS_PRoject_Backend.Data;

public static class DbInitializer
{
    public static void Initialize(ApplicationDbContext context)
    {
        // Apply pending migrations automatically
        context.Database.Migrate();

        // Check if database has already been seeded
        if (context.Zones.Any())
        {
            return; // DB has been seeded
        }

        // 1. Create Zones (initial adminId null to break circular dependency)
        var zones = new List<Zone>
        {
            new() { Id = "ZN-001", Name = "Mangalore", Location = "Mangalore City", Pincode = "575001", Description = "Mangalore Urban Zone", Status = "Active", TotalMeters = 3, CreatedAt = DateTime.UtcNow.AddMonths(-12) },
            new() { Id = "ZN-002", Name = "Udupi", Location = "Udupi Coastal", Pincode = "576101", Description = "Udupi District Zone", Status = "Active", TotalMeters = 1, CreatedAt = DateTime.UtcNow.AddMonths(-12) },
            new() { Id = "ZN-003", Name = "Surathkal", Location = "Surathkal Beach Road", Pincode = "575014", Description = "Surathkal Industrial Suburb", Status = "Active", TotalMeters = 1, CreatedAt = DateTime.UtcNow.AddMonths(-10) },
            new() { Id = "ZN-004", Name = "Mulki", Location = "Mulki Town", Pincode = "574154", Description = "Mulki Semi-Urban Zone", Status = "Active", TotalMeters = 1, CreatedAt = DateTime.UtcNow.AddMonths(-8) }
        };
        context.Zones.AddRange(zones);
        context.SaveChanges();

        // 2. Create Users (Enterprise Admin, Zone Admins, and End Users)
        var users = new List<User>
        {
            // Enterprise Admin
            new() {
                Id = "USR-001", Role = "enterprise", ZoneId = null, ZoneName = null,
                Name = "Rajesh Kumar", Email = "rajesh.enterprise@mdms.com", Phone = "+91 9876543210",
                Address = "HQ Tower, Bengaluru", Status = "Active", MeterCount = 0,
                NotificationPrefs = "{\"email\":true,\"sms\":true,\"inApp\":true}",
                JoinedOn = DateTime.UtcNow.AddYears(-2), AvatarUrl = "https://images.unsplash.com/photo-1534528741775-53994a69daeb"
            },
            // Zone Admins
            new() {
                Id = "USR-002", Role = "zone_admin", ZoneId = "ZN-001", ZoneName = "Mangalore",
                Name = "Priya Sharma", Email = "priya.mangalore@mdms.com", Phone = "+91 9811223344",
                Address = "Kodialbail, Mangalore", Status = "Active", MeterCount = 0,
                NotificationPrefs = "{\"email\":true,\"sms\":true,\"inApp\":true}",
                JoinedOn = DateTime.UtcNow.AddMonths(-12)
            },
            new() {
                Id = "USR-003", Role = "zone_admin", ZoneId = "ZN-002", ZoneName = "Udupi",
                Name = "Anand Shetty", Email = "anand.udupi@mdms.com", Phone = "+91 9822334455",
                Address = "Car Street, Udupi", Status = "Active", MeterCount = 0,
                NotificationPrefs = "{\"email\":true,\"sms\":true,\"inApp\":true}",
                JoinedOn = DateTime.UtcNow.AddMonths(-12)
            },
            new() {
                Id = "USR-004", Role = "zone_admin", ZoneId = "ZN-003", ZoneName = "Surathkal",
                Name = "Deepa Rao", Email = "deepa.surathkal@mdms.com", Phone = "+91 9833445566",
                Address = "NITK Road, Surathkal", Status = "Active", MeterCount = 0,
                NotificationPrefs = "{\"email\":true,\"sms\":true,\"inApp\":true}",
                JoinedOn = DateTime.UtcNow.AddMonths(-10)
            },
            new() {
                Id = "USR-005", Role = "zone_admin", ZoneId = "ZN-004", ZoneName = "Mulki",
                Name = "Kiran Bhat", Email = "kiran.mulki@mdms.com", Phone = "+91 9844556677",
                Address = "Main Road, Mulki", Status = "Active", MeterCount = 0,
                NotificationPrefs = "{\"email\":true,\"sms\":true,\"inApp\":true}",
                JoinedOn = DateTime.UtcNow.AddMonths(-8)
            },
            // End Users (Customers)
            new() {
                Id = "USR-006", Role = "end_user", ZoneId = "ZN-001", ZoneName = "Mangalore",
                Name = "Santosh Saw", Email = "santosh.saw@example.com", Phone = "+91 9900112233",
                Address = "Kadri Hills, Mangalore", Status = "Active", MeterCount = 2,
                NotificationPrefs = "{\"email\":true,\"sms\":true,\"inApp\":true}",
                JoinedOn = DateTime.UtcNow.AddMonths(-14), AvatarUrl = "https://images.unsplash.com/photo-1507003211169-0a1dd7228f2d"
            },
            new() {
                Id = "USR-007", Role = "end_user", ZoneId = "ZN-001", ZoneName = "Mangalore",
                Name = "Rahul Hegde", Email = "rahul.hegde@example.com", Phone = "+91 9911223344",
                Address = "Bejai, Mangalore", Status = "Active", MeterCount = 1,
                NotificationPrefs = "{\"email\":true,\"sms\":false,\"inApp\":true}",
                JoinedOn = DateTime.UtcNow.AddMonths(-11)
            },
            new() {
                Id = "USR-008", Role = "end_user", ZoneId = "ZN-002", ZoneName = "Udupi",
                Name = "Sneha Prabhu", Email = "sneha.prabhu@example.com", Phone = "+91 9922334455",
                Address = "Manipal Road, Udupi", Status = "Active", MeterCount = 1,
                NotificationPrefs = "{\"email\":true,\"sms\":true,\"inApp\":true}",
                JoinedOn = DateTime.UtcNow.AddMonths(-9)
            },
            new() {
                Id = "USR-009", Role = "end_user", ZoneId = "ZN-003", ZoneName = "Surathkal",
                Name = "Vikram Kamath", Email = "vikram.kamath@example.com", Phone = "+91 9933445566",
                Address = "Idya, Surathkal", Status = "Active", MeterCount = 1,
                NotificationPrefs = "{\"email\":true,\"sms\":true,\"inApp\":false}",
                JoinedOn = DateTime.UtcNow.AddMonths(-6)
            },
            new() {
                Id = "USR-010", Role = "end_user", ZoneId = "ZN-004", ZoneName = "Mulki",
                Name = "Pooja Nayak", Email = "pooja.nayak@example.com", Phone = "+91 9944556677",
                Address = "Bappanadu, Mulki", Status = "Active", MeterCount = 1,
                NotificationPrefs = "{\"email\":true,\"sms\":true,\"inApp\":true}",
                JoinedOn = DateTime.UtcNow.AddMonths(-4)
            }
        };
        context.Users.AddRange(users);
        context.SaveChanges();

        // 3. Assign Admins to Zones
        var zn1 = context.Zones.Find("ZN-001")!;
        zn1.AdminId = "USR-002";
        zn1.AdminName = "Priya Sharma";

        var zn2 = context.Zones.Find("ZN-002")!;
        zn2.AdminId = "USR-003";
        zn2.AdminName = "Anand Shetty";

        var zn3 = context.Zones.Find("ZN-003")!;
        zn3.AdminId = "USR-004";
        zn3.AdminName = "Deepa Rao";

        var zn4 = context.Zones.Find("ZN-004")!;
        zn4.AdminId = "USR-005";
        zn4.AdminName = "Kiran Bhat";
        context.SaveChanges();

        // 4. Create Tariffs & TariffSlabs
        var tariffs = new List<Tariff>
        {
            new() { Id = "TRF-RES", Category = "Residential", FixedCharge = 120.00m, TaxPercent = 5.0m, EffectiveFrom = DateTime.UtcNow.AddYears(-2) },
            new() { Id = "TRF-COM", Category = "Commercial", FixedCharge = 250.00m, TaxPercent = 18.0m, EffectiveFrom = DateTime.UtcNow.AddYears(-2) },
            new() { Id = "TRF-IND", Category = "Industrial", FixedCharge = 600.00m, TaxPercent = 18.0m, EffectiveFrom = DateTime.UtcNow.AddYears(-2) }
        };
        context.Tariffs.AddRange(tariffs);
        context.SaveChanges();

        var slabs = new List<TariffSlab>
        {
            // Residential Slabs
            new() { Id = "SLB-RES-1", TariffId = "TRF-RES", FromUnit = 0, ToUnit = 100, RatePerUnit = 4.00m },
            new() { Id = "SLB-RES-2", TariffId = "TRF-RES", FromUnit = 101, ToUnit = 200, RatePerUnit = 5.50m },
            new() { Id = "SLB-RES-3", TariffId = "TRF-RES", FromUnit = 201, ToUnit = 400, RatePerUnit = 7.00m },
            new() { Id = "SLB-RES-4", TariffId = "TRF-RES", FromUnit = 401, ToUnit = null, RatePerUnit = 8.50m },

            // Commercial Slabs
            new() { Id = "SLB-COM-1", TariffId = "TRF-COM", FromUnit = 0, ToUnit = 200, RatePerUnit = 6.50m },
            new() { Id = "SLB-COM-2", TariffId = "TRF-COM", FromUnit = 201, ToUnit = null, RatePerUnit = 9.00m },

            // Industrial Slabs
            new() { Id = "SLB-IND-1", TariffId = "TRF-IND", FromUnit = 0, ToUnit = 500, RatePerUnit = 8.00m },
            new() { Id = "SLB-IND-2", TariffId = "TRF-IND", FromUnit = 501, ToUnit = null, RatePerUnit = 11.00m }
        };
        context.TariffSlabs.AddRange(slabs);
        context.SaveChanges();

        // 5. Create Meters
        var meters = new List<Meter>
        {
            // Santosh Saw owns 2 meters
            new() {
                Id = "MTR-1001", OwnerId = "USR-006", OwnerName = "Santosh Saw",
                ZoneId = "ZN-001", ZoneName = "Mangalore", TariffId = "TRF-RES", TariffCategory = "Residential",
                Address = "Kadri Hills, House #12, Mangalore", Status = "Active", IsPrimary = true,
                LastReading = 2450.5, LastReadingAt = DateTime.UtcNow.AddMinutes(-15), InstalledOn = DateTime.UtcNow.AddMonths(-14)
            },
            new() {
                Id = "MTR-1002", OwnerId = "USR-006", OwnerName = "Santosh Saw",
                ZoneId = "ZN-001", ZoneName = "Mangalore", TariffId = "TRF-RES", TariffCategory = "Residential",
                Address = "Kadri Hills, Out-House, Mangalore", Status = "Active", IsPrimary = false,
                LastReading = 850.2, LastReadingAt = DateTime.UtcNow.AddMinutes(-20), InstalledOn = DateTime.UtcNow.AddMonths(-12)
            },
            // Rahul Hegde owns 1 meter (commercial)
            new() {
                Id = "MTR-1003", OwnerId = "USR-007", OwnerName = "Rahul Hegde",
                ZoneId = "ZN-001", ZoneName = "Mangalore", TariffId = "TRF-COM", TariffCategory = "Commercial",
                Address = "Bejai Commercial Complex, Shop #4, Mangalore", Status = "Active", IsPrimary = true,
                LastReading = 5320.0, LastReadingAt = DateTime.UtcNow.AddMinutes(-5), InstalledOn = DateTime.UtcNow.AddMonths(-11)
            },
            // Sneha Prabhu
            new() {
                Id = "MTR-1004", OwnerId = "USR-008", OwnerName = "Sneha Prabhu",
                ZoneId = "ZN-002", ZoneName = "Udupi", TariffId = "TRF-RES", TariffCategory = "Residential",
                Address = "Manipal Road, Apt 302, Udupi", Status = "Active", IsPrimary = true,
                LastReading = 1620.4, LastReadingAt = DateTime.UtcNow.AddHours(-1), InstalledOn = DateTime.UtcNow.AddMonths(-9)
            },
            // Vikram Kamath
            new() {
                Id = "MTR-1005", OwnerId = "USR-009", OwnerName = "Vikram Kamath",
                ZoneId = "ZN-003", ZoneName = "Surathkal", TariffId = "TRF-RES", TariffCategory = "Residential",
                Address = "Idya, Villa #7, Surathkal", Status = "Active", IsPrimary = true,
                LastReading = 980.8, LastReadingAt = DateTime.UtcNow.AddHours(-2), InstalledOn = DateTime.UtcNow.AddMonths(-6)
            },
            // Pooja Nayak
            new() {
                Id = "MTR-1006", OwnerId = "USR-010", OwnerName = "Pooja Nayak",
                ZoneId = "ZN-004", ZoneName = "Mulki", TariffId = "TRF-RES", TariffCategory = "Residential",
                Address = "Bappanadu, House #5, Mulki", Status = "Active", IsPrimary = true,
                LastReading = 610.1, LastReadingAt = DateTime.UtcNow.AddHours(-3), InstalledOn = DateTime.UtcNow.AddMonths(-4)
            }
        };
        context.Meters.AddRange(meters);
        context.SaveChanges();

        // 6. Create Bills
        var bills = new List<Bill>
        {
            // Bill for USR-006 (October 2026 - Paid) worked example from spec:
            // 250 units => 100*4.00 + 100*5.50 + 50*7.00 = 1300 + fixed 120 + tax 71 = 1491
            new() {
                Id = "BILL-2026-10-USR006", UserId = "USR-006", MeterId = "MTR-1001", ZoneId = "ZN-001",
                BillingMonth = "2026-10", GeneratedOn = new DateTime(2026, 11, 1, 0, 0, 0, DateTimeKind.Utc),
                DueDate = new DateTime(2026, 11, 16, 0, 0, 0, DateTimeKind.Utc),
                PreviousReading = 2200.5, CurrentReading = 2450.5, TotalUnits = 250.0,
                EnergyCharge = 1300.00m, FixedCharge = 120.00m, TaxAmount = 71.00m, Amount = 1491.00m,
                Status = "Paid", PaidOn = new DateTime(2026, 11, 10, 14, 30, 0, DateTimeKind.Utc)
            },
            // Bill for USR-006 (September 2026 - Paid)
            new() {
                Id = "BILL-2026-09-USR006", UserId = "USR-006", MeterId = "MTR-1001", ZoneId = "ZN-001",
                BillingMonth = "2026-09", GeneratedOn = new DateTime(2026, 10, 1, 0, 0, 0, DateTimeKind.Utc),
                DueDate = new DateTime(2026, 10, 16, 0, 0, 0, DateTimeKind.Utc),
                PreviousReading = 1980.5, CurrentReading = 2200.5, TotalUnits = 220.0,
                EnergyCharge = 1090.00m, FixedCharge = 120.00m, TaxAmount = 60.50m, Amount = 1270.50m,
                Status = "Paid", PaidOn = new DateTime(2026, 10, 12, 11, 15, 0, DateTimeKind.Utc)
            },
            // Bill for USR-007 (October 2026 - Pending)
            new() {
                Id = "BILL-2026-10-USR007", UserId = "USR-007", MeterId = "MTR-1003", ZoneId = "ZN-001",
                BillingMonth = "2026-10", GeneratedOn = new DateTime(2026, 11, 1, 0, 0, 0, DateTimeKind.Utc),
                DueDate = new DateTime(2026, 11, 16, 0, 0, 0, DateTimeKind.Utc),
                PreviousReading = 4970.0, CurrentReading = 5320.0, TotalUnits = 350.0,
                EnergyCharge = 2650.00m, FixedCharge = 250.00m, TaxAmount = 522.00m, Amount = 3422.00m,
                Status = "Pending", PaidOn = null
            }
        };
        context.Bills.AddRange(bills);
        context.SaveChanges();

        // 7. Create BillDays (daily breakdown for BILL-2026-10-USR006)
        var billDays = new List<BillDay>();
        double currentReading = 2200.5;
        for (int day = 1; day <= 30; day++)
        {
            double consumption = 8.0 + (day % 3); // ~8.33 kWh per day
            currentReading += consumption;
            billDays.Add(new BillDay
            {
                Id = $"BDAY-202610-{day:D2}",
                BillId = "BILL-2026-10-USR006",
                Date = new DateTime(2026, 10, day, 0, 0, 0, DateTimeKind.Utc),
                Reading = Math.Round(currentReading, 1),
                Consumption = Math.Round(consumption, 1),
                Cost = Math.Round((decimal)consumption * 5.2m, 2)
            });
        }
        context.BillDays.AddRange(billDays);
        context.SaveChanges();

        // 8. Create Payments
        var payments = new List<Payment>
        {
            new() {
                Id = "PMT-001", BillId = "BILL-2026-10-USR006", UserId = "USR-006",
                AmountPaid = 1491.00m, Method = "UPI", TransactionRef = "UPI-98234729104",
                Status = "Success", PaidOn = new DateTime(2026, 11, 10, 14, 30, 0, DateTimeKind.Utc)
            },
            new() {
                Id = "PMT-002", BillId = "BILL-2026-09-USR006", UserId = "USR-006",
                AmountPaid = 1270.50m, Method = "Card", TransactionRef = "TXN-8746392011",
                Status = "Success", PaidOn = new DateTime(2026, 10, 12, 11, 15, 0, DateTimeKind.Utc)
            }
        };
        context.Payments.AddRange(payments);
        context.SaveChanges();

        // 9. Create MeterReadings
        var readings = new List<MeterReading>();
        var baseTime = DateTime.UtcNow.Date.AddDays(-1);
        double cumReading = 2430.0;
        for (int hour = 0; hour < 24; hour++)
        {
            double cons = 0.5 + (hour is >= 18 and <= 22 ? 1.2 : 0.3);
            cumReading += cons;
            readings.Add(new MeterReading
            {
                Id = $"RDG-{hour:D2}",
                MeterId = "MTR-1001",
                Timestamp = baseTime.AddHours(hour),
                Granularity = "hourly",
                Reading = Math.Round(cumReading, 2),
                Consumption = Math.Round(cons, 2),
                Note = hour is >= 18 and <= 22 ? "Peak usage" : null
            });
        }
        context.MeterReadings.AddRange(readings);
        context.SaveChanges();

        // 10. Create ConsumptionSeries
        var consumptionSeries = new List<ConsumptionSeries>
        {
            new() {
                Id = "CS-USR-006-WEEK",
                Scope = "user",
                ScopeId = "USR-006",
                Range = "week",
                Labels = new() { "Mon", "Tue", "Wed", "Thu", "Fri", "Sat", "Sun" },
                Current = new() { 8.5, 9.2, 7.8, 8.9, 10.1, 12.4, 11.0 },
                Previous = new() { 7.9, 8.5, 8.0, 8.4, 9.5, 11.2, 10.5 }
            },
            new() {
                Id = "CS-ZN-001-WEEK",
                Scope = "zone",
                ScopeId = "ZN-001",
                Range = "week",
                Labels = new() { "Mon", "Tue", "Wed", "Thu", "Fri", "Sat", "Sun" },
                Current = new() { 125.0, 132.5, 120.0, 135.2, 140.0, 160.5, 155.0 },
                Previous = new() { 118.0, 125.0, 122.0, 130.0, 138.0, 152.0, 148.0 }
            },
            new() {
                Id = "CS-ALL-MONTH",
                Scope = "enterprise",
                ScopeId = "ALL",
                Range = "month",
                Labels = new() { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" },
                Current = new() { 3200, 3100, 3400, 3800, 4200, 4500, 4300, 4100, 3900, 3700, 3600, 3500 },
                Previous = new() { 3000, 2900, 3200, 3600, 3900, 4200, 4000, 3900, 3700, 3500, 3400, 3300 }
            }
        };
        context.ConsumptionSeries.AddRange(consumptionSeries);
        context.SaveChanges();

        // 11. Create Alerts
        var alerts = new List<Alert>
        {
            new() {
                Id = "ALT-001", ZoneId = "ZN-001", ZoneName = "Mangalore", MeterId = "MTR-1001",
                Type = "High consumption", Severity = "Warning", Status = "Pending",
                Message = "Daily consumption exceeded threshold (15.2 kWh vs 12.0 kWh threshold)",
                CreatedAt = DateTime.UtcNow.AddHours(-4)
            },
            new() {
                Id = "ALT-002", ZoneId = "ZN-002", ZoneName = "Udupi", MeterId = "MTR-1004",
                Type = "Abnormal reading", Severity = "Critical", Status = "Acknowledged",
                Message = "Abnormal sudden dip in hourly voltage / consumption reading detected",
                CreatedAt = DateTime.UtcNow.AddHours(-10)
            },
            new() {
                Id = "ALT-003", ZoneId = "ZN-003", ZoneName = "Surathkal", MeterId = "MTR-1005",
                Type = "Inactive meter", Severity = "Critical", Status = "Resolved",
                Message = "Meter heartbeat missing for > 24 hours. Tech team dispatched.",
                CreatedAt = DateTime.UtcNow.AddDays(-2)
            }
        };
        context.Alerts.AddRange(alerts);
        context.SaveChanges();

        // 12. Create Notifications
        var notifications = new List<Notification>
        {
            new() {
                Id = "NOTIF-001", UserId = "USR-006", Type = "bill_generated",
                Title = "New Electricity Bill Generated",
                Message = "Your bill for 2026-10 of INR 1491.00 has been generated. Due date: Nov 16, 2026.",
                IsRead = true, CreatedAt = DateTime.UtcNow.AddDays(-22)
            },
            new() {
                Id = "NOTIF-002", UserId = "USR-006", Type = "payment_received",
                Title = "Payment Successful",
                Message = "Payment of INR 1491.00 via UPI (Ref: UPI-98234729104) has been credited.",
                IsRead = true, CreatedAt = DateTime.UtcNow.AddDays(-12)
            },
            new() {
                Id = "NOTIF-003", UserId = "USR-006", Type = "high_consumption",
                Title = "High Energy Consumption Notice",
                Message = "Your consumption yesterday was 20% higher than your average weekday consumption.",
                IsRead = false, CreatedAt = DateTime.UtcNow.AddHours(-4)
            },
            new() {
                Id = "NOTIF-004", UserId = "USR-007", Type = "payment_due",
                Title = "Upcoming Payment Due",
                Message = "Your commercial bill for 2026-10 (INR 3422.00) is due on Nov 16, 2026.",
                IsRead = false, CreatedAt = DateTime.UtcNow.AddDays(-3)
            }
        };
        context.Notifications.AddRange(notifications);
        context.SaveChanges();

        // 13. Create AuditLogs
        var auditLogs = new List<AuditLog>
        {
            new() {
                Id = "AUD-001", Timestamp = DateTime.UtcNow.AddDays(-30), UserId = "USR-001", UserName = "Rajesh Kumar",
                Action = "Created zone", Resource = "Zone ZN-004 (Mulki)", Status = "Success"
            },
            new() {
                Id = "AUD-002", Timestamp = DateTime.UtcNow.AddDays(-22), UserId = "USR-002", UserName = "Priya Sharma",
                Action = "Generated billing cycle", Resource = "Billing Month 2026-10", Status = "Success"
            },
            new() {
                Id = "AUD-003", Timestamp = DateTime.UtcNow.AddDays(-12), UserId = "USR-006", UserName = "Santosh Saw",
                Action = "Processed payment", Resource = "Bill BILL-2026-10-USR006", Status = "Success"
            },
            new() {
                Id = "AUD-004", Timestamp = DateTime.UtcNow.AddDays(-5), UserId = "USR-003", UserName = "Anand Shetty",
                Action = "Updated zone settings", Resource = "ZoneSettings ZST-002", Status = "Success"
            }
        };
        context.AuditLogs.AddRange(auditLogs);
        context.SaveChanges();

        // 14. Create ZoneSettings
        var zoneSettings = new List<ZoneSettings>
        {
            new() { Id = "ZST-001", ZoneId = "ZN-001", HighConsumptionThreshold = 35.0, LowConsumptionThreshold = 1.0, AbnormalReadingFrequency = 5.0, InactiveMeterHours = 12.0, NotifyEmail = true, NotifySms = true, NotifyInApp = true },
            new() { Id = "ZST-002", ZoneId = "ZN-002", HighConsumptionThreshold = 30.0, LowConsumptionThreshold = 1.0, AbnormalReadingFrequency = 5.0, InactiveMeterHours = 12.0, NotifyEmail = true, NotifySms = false, NotifyInApp = true },
            new() { Id = "ZST-003", ZoneId = "ZN-003", HighConsumptionThreshold = 40.0, LowConsumptionThreshold = 2.0, AbnormalReadingFrequency = 6.0, InactiveMeterHours = 24.0, NotifyEmail = true, NotifySms = true, NotifyInApp = true },
            new() { Id = "ZST-004", ZoneId = "ZN-004", HighConsumptionThreshold = 25.0, LowConsumptionThreshold = 0.5, AbnormalReadingFrequency = 4.0, InactiveMeterHours = 12.0, NotifyEmail = true, NotifySms = false, NotifyInApp = true }
        };
        context.ZoneSettings.AddRange(zoneSettings);
        context.SaveChanges();

        // 15. Create EnterpriseSettings (Singleton)
        var enterpriseSettings = new EnterpriseSettings
        {
            Id = "1",
            DataRetentionDays = 365,
            AutoLogoutMinutes = 30,
            AuditLogRetentionDays = 180,
            TimeZone = "Asia/Kolkata",
            DefaultLanguage = "en",
            Currency = "INR",
            NotificationDefaults = "{\"email\":true,\"sms\":true,\"inApp\":true}"
        };
        context.EnterpriseSettings.Add(enterpriseSettings);
        context.SaveChanges();

        // 16. Create UserStatusStats
        var userStatusStats = new List<UserStatusStats>
        {
            new() { Id = "USS-2024-ADM", Year = 2024, Group = "admin", ActiveCount = 3, DeactivatedCount = 0 },
            new() { Id = "USS-2024-CON", Year = 2024, Group = "consumer", ActiveCount = 20, DeactivatedCount = 2 },
            new() { Id = "USS-2025-ADM", Year = 2025, Group = "admin", ActiveCount = 4, DeactivatedCount = 0 },
            new() { Id = "USS-2025-CON", Year = 2025, Group = "consumer", ActiveCount = 35, DeactivatedCount = 3 },
            new() { Id = "USS-2026-ADM", Year = 2026, Group = "admin", ActiveCount = 5, DeactivatedCount = 0 },
            new() { Id = "USS-2026-CON", Year = 2026, Group = "consumer", ActiveCount = 5, DeactivatedCount = 0 }
        };
        context.UserStatusStats.AddRange(userStatusStats);
        context.SaveChanges();
    }
}
