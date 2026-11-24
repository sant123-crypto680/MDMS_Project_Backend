using MDMS_PRoject_Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace MDMS_PRoject_Backend.Data;

public static class DataGenerator
{
    public static void SeedAll(ApplicationDbContext context)
    {
        // Ensure change tracker is empty
        context.ChangeTracker.Clear();

        // Use a fixed seed for 100% deterministic, reproducible seed data
        var rnd = new Random(42);

        // ----------------------------------------------------
        // 1. ZONES (4 Zones)
        // ----------------------------------------------------
        var zones = new List<Zone>
        {
            new() { Id = "ZN-001", Name = "Mangalore", Location = "Mangalore Urban", Pincode = "575001", Description = "Mangalore Coastal Urban Zone", Status = "Active", CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new() { Id = "ZN-002", Name = "Udupi", Location = "Udupi District", Pincode = "576101", Description = "Udupi Coastal District Zone", Status = "Active", CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new() { Id = "ZN-003", Name = "Surathkal", Location = "Surathkal Suburb", Pincode = "575014", Description = "Surathkal Industrial Suburb Zone", Status = "Active", CreatedAt = new DateTime(2025, 2, 1, 0, 0, 0, DateTimeKind.Utc) },
            new() { Id = "ZN-004", Name = "Mulki", Location = "Mulki Semi-Urban", Pincode = "574154", Description = "Mulki Semi-Urban Division", Status = "Active", CreatedAt = new DateTime(2025, 3, 1, 0, 0, 0, DateTimeKind.Utc) }
        };
        context.Zones.AddRange(zones);
        context.SaveChanges();

        // ----------------------------------------------------
        // 2. USERS: Enterprise Admin, Zone Admins & 48 Customers (12 per zone)
        // ----------------------------------------------------
        var users = new List<User>
        {
            // Enterprise Admin (USR-001)
            new() {
                Id = "USR-001", Role = "enterprise", ZoneId = null, ZoneName = null,
                Name = "Rajesh Kumar", Email = "rajesh.enterprise@mdms.com", Phone = "+91 9876543210",
                Address = "HQ Tower, MG Road, Bengaluru", Status = "Active", MeterCount = 0,
                NotificationPrefs = "{\"email\":true,\"sms\":true,\"inApp\":true}",
                JoinedOn = new DateTime(2024, 6, 1, 0, 0, 0, DateTimeKind.Utc),
                AvatarUrl = "https://images.unsplash.com/photo-1534528741775-53994a69daeb"
            },
            // Zone Admins (USR-002 to USR-005)
            new() {
                Id = "USR-002", Role = "zone_admin", ZoneId = "ZN-001", ZoneName = "Mangalore",
                Name = "Priya Sharma", Email = "priya.mangalore@mdms.com", Phone = "+91 9811223344",
                Address = "Kodialbail, Mangalore", Status = "Active", MeterCount = 0,
                NotificationPrefs = "{\"email\":true,\"sms\":true,\"inApp\":true}",
                JoinedOn = new DateTime(2025, 1, 5, 0, 0, 0, DateTimeKind.Utc)
            },
            new() {
                Id = "USR-003", Role = "zone_admin", ZoneId = "ZN-002", ZoneName = "Udupi",
                Name = "Anand Shetty", Email = "anand.udupi@mdms.com", Phone = "+91 9822334455",
                Address = "Car Street, Udupi", Status = "Active", MeterCount = 0,
                NotificationPrefs = "{\"email\":true,\"sms\":true,\"inApp\":true}",
                JoinedOn = new DateTime(2025, 1, 5, 0, 0, 0, DateTimeKind.Utc)
            },
            new() {
                Id = "USR-004", Role = "zone_admin", ZoneId = "ZN-003", ZoneName = "Surathkal",
                Name = "Deepa Rao", Email = "deepa.surathkal@mdms.com", Phone = "+91 9833445566",
                Address = "NITK Road, Surathkal", Status = "Active", MeterCount = 0,
                NotificationPrefs = "{\"email\":true,\"sms\":true,\"inApp\":true}",
                JoinedOn = new DateTime(2025, 2, 5, 0, 0, 0, DateTimeKind.Utc)
            },
            new() {
                Id = "USR-005", Role = "zone_admin", ZoneId = "ZN-004", ZoneName = "Mulki",
                Name = "Kiran Bhat", Email = "kiran.mulki@mdms.com", Phone = "+91 9844556677",
                Address = "Main Road, Mulki", Status = "Active", MeterCount = 0,
                NotificationPrefs = "{\"email\":true,\"sms\":true,\"inApp\":true}",
                JoinedOn = new DateTime(2025, 3, 5, 0, 0, 0, DateTimeKind.Utc)
            },
            // Fixed Demo Customer (USR-006) - Mangalore, owns 2 meters
            new() {
                Id = "USR-006", Role = "end_user", ZoneId = "ZN-001", ZoneName = "Mangalore",
                Name = "Santosh Saw", Email = "santosh.saw@example.com", Phone = "+91 9900112233",
                Address = "Kadri Hills, House #12, Mangalore", Status = "Active", MeterCount = 2,
                NotificationPrefs = "{\"email\":true,\"sms\":true,\"inApp\":true}",
                JoinedOn = new DateTime(2025, 10, 1, 0, 0, 0, DateTimeKind.Utc),
                AvatarUrl = "https://images.unsplash.com/photo-1507003211169-0a1dd7228f2d"
            }
        };

        // Realistic name pools for 47 additional customers (12 per zone total = 48 customers)
        var firstNames = new[] { "Arun", "Sunita", "Ramesh", "Kavita", "Ganesh", "Divya", "Prashanth", "Swathi", "Manish", "Anjali", "Suresh", "Lakshmi", "Venkatesh", "Meera", "Rohit", "Snehal", "Harish", "Asha", "Naveen", "Geetha", "Sanjay", "Shweta", "Mohan", "Rekha", "Karthik", "Bhavya", "Ashok", "Pallavi", "Vivek", "Tanvi", "Satish", "Pooja", "Ajay", "Preeti", "Sunil", "Radha", "Rajiv", "Rashmi", "Chetan", "Vidya", "Nitin", "Deepika", "Raghu", "Suma", "Vijay", "Anita", "Vinay" };
        var lastNames = new[] { "Pai", "Shenoy", "Poojary", "Hegde", "Acharya", "Nayak", "Kudva", "Mallya", "Kamath", "Prabhu", "Bhandary", "Salian", "Kotian", "Alva", "Suvarna", "Kulal", "Devadiga", "Shetty", "Rao", "Bhat" };

        var zoneLookup = new[] {
            (Id: "ZN-001", Name: "Mangalore", Loc: "Mangalore", Count: 11), // USR-006 already in ZN-001 => total 12
            (Id: "ZN-002", Name: "Udupi", Loc: "Udupi", Count: 12),
            (Id: "ZN-003", Name: "Surathkal", Loc: "Surathkal", Count: 12),
            (Id: "ZN-004", Name: "Mulki", Loc: "Mulki", Count: 12)
        };

        int userIndex = 7;
        foreach (var z in zoneLookup)
        {
            for (int i = 0; i < z.Count; i++)
            {
                var fn = firstNames[(userIndex * 3) % firstNames.Length];
                var ln = lastNames[(userIndex * 7) % lastNames.Length];
                var uId = $"USR-{userIndex:D3}";

                users.Add(new User
                {
                    Id = uId,
                    Role = "end_user",
                    ZoneId = z.Id,
                    ZoneName = z.Name,
                    Name = $"{fn} {ln}",
                    Email = $"{fn.ToLower()}.{ln.ToLower()}{userIndex}@example.com",
                    Phone = $"+91 98{rnd.Next(10000000, 99999999)}",
                    Address = $"Door #{userIndex * 4}, Cross Road {i + 1}, {z.Loc}",
                    Status = "Active",
                    MeterCount = 1,
                    NotificationPrefs = "{\"email\":true,\"sms\":true,\"inApp\":true}",
                    JoinedOn = new DateTime(2025, 10, 1, 0, 0, 0, DateTimeKind.Utc).AddDays(userIndex % 15)
                });
                userIndex++;
            }
        }
        context.Users.AddRange(users);
        context.SaveChanges();

        // Assign Zone Admins to Zones
        var z1 = context.Zones.Find("ZN-001")!;
        z1.AdminId = "USR-002"; z1.AdminName = "Priya Sharma";
        var z2 = context.Zones.Find("ZN-002")!;
        z2.AdminId = "USR-003"; z2.AdminName = "Anand Shetty";
        var z3 = context.Zones.Find("ZN-003")!;
        z3.AdminId = "USR-004"; z3.AdminName = "Deepa Rao";
        var z4 = context.Zones.Find("ZN-004")!;
        z4.AdminId = "USR-005"; z4.AdminName = "Kiran Bhat";
        context.SaveChanges();

        // ----------------------------------------------------
        // 3. TARIFFS & SLABS
        // ----------------------------------------------------
        var tariffs = new List<Tariff>
        {
            new() { Id = "TRF-RES", Category = "Residential", FixedCharge = 120.00m, TaxPercent = 5.0m, EffectiveFrom = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new() { Id = "TRF-COM", Category = "Commercial", FixedCharge = 250.00m, TaxPercent = 18.0m, EffectiveFrom = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new() { Id = "TRF-IND", Category = "Industrial", FixedCharge = 600.00m, TaxPercent = 18.0m, EffectiveFrom = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc) }
        };
        context.Tariffs.AddRange(tariffs);
        context.SaveChanges();

        var slabs = new List<TariffSlab>
        {
            // Residential
            new() { Id = "SLB-RES-1", TariffId = "TRF-RES", FromUnit = 0, ToUnit = 100, RatePerUnit = 4.00m },
            new() { Id = "SLB-RES-2", TariffId = "TRF-RES", FromUnit = 101, ToUnit = 200, RatePerUnit = 5.50m },
            new() { Id = "SLB-RES-3", TariffId = "TRF-RES", FromUnit = 201, ToUnit = 400, RatePerUnit = 7.00m },
            new() { Id = "SLB-RES-4", TariffId = "TRF-RES", FromUnit = 401, ToUnit = null, RatePerUnit = 8.50m },

            // Commercial
            new() { Id = "SLB-COM-1", TariffId = "TRF-COM", FromUnit = 0, ToUnit = 200, RatePerUnit = 6.50m },
            new() { Id = "SLB-COM-2", TariffId = "TRF-COM", FromUnit = 201, ToUnit = null, RatePerUnit = 9.00m },

            // Industrial
            new() { Id = "SLB-IND-1", TariffId = "TRF-IND", FromUnit = 0, ToUnit = 500, RatePerUnit = 8.00m },
            new() { Id = "SLB-IND-2", TariffId = "TRF-IND", FromUnit = 501, ToUnit = null, RatePerUnit = 11.00m }
        };
        context.TariffSlabs.AddRange(slabs);
        context.SaveChanges();

        // ----------------------------------------------------
        // 4. METERS (Primary meter for every customer + 9 secondary meters = 57 meters)
        // ----------------------------------------------------
        var customerUsers = users.Where(u => u.Role == "end_user").ToList();
        var meters = new List<Meter>();
        int meterNum = 1001;

        // Primary meters
        foreach (var cust in customerUsers)
        {
            var isCom = (meterNum % 7 == 0); // ~14% commercial
            var tId = isCom ? "TRF-COM" : "TRF-RES";
            var tCat = isCom ? "Commercial" : "Residential";
            var initialReading = Math.Round(1200.0 + rnd.Next(500, 2500) + rnd.NextDouble(), 1);

            meters.Add(new Meter
            {
                Id = $"MTR-{meterNum}",
                OwnerId = cust.Id,
                OwnerName = cust.Name,
                ZoneId = cust.ZoneId!,
                ZoneName = cust.ZoneName,
                TariffId = tId,
                TariffCategory = tCat,
                Address = cust.Address,
                Status = "Active",
                IsPrimary = true,
                LastReading = initialReading + 2400.0,
                LastReadingAt = DateTime.UtcNow.AddMinutes(-rnd.Next(5, 60)),
                InstalledOn = new DateTime(2025, 10, 1, 0, 0, 0, DateTimeKind.Utc)
            });
            meterNum++;
        }

        // Secondary meters for 9 customers (including USR-006)
        var dualOwners = customerUsers.Take(9).ToList();
        foreach (var cust in dualOwners)
        {
            meters.Add(new Meter
            {
                Id = $"MTR-{meterNum}",
                OwnerId = cust.Id,
                OwnerName = cust.Name,
                ZoneId = cust.ZoneId!,
                ZoneName = cust.ZoneName,
                TariffId = "TRF-RES",
                TariffCategory = "Residential",
                Address = $"{cust.Address} (Annex/Floor 2)",
                Status = "Active",
                IsPrimary = false,
                LastReading = Math.Round(800.0 + rnd.Next(200, 1000) + rnd.NextDouble(), 1),
                LastReadingAt = DateTime.UtcNow.AddMinutes(-rnd.Next(10, 90)),
                InstalledOn = new DateTime(2025, 10, 1, 0, 0, 0, DateTimeKind.Utc)
            });
            cust.MeterCount = 2; // Update dual owner meter count
            meterNum++;
        }
        context.Meters.AddRange(meters);
        context.SaveChanges();

        // Update Zone total meters
        foreach (var z in zones)
        {
            z.TotalMeters = meters.Count(m => m.ZoneId == z.Id);
        }
        context.SaveChanges();

        // ----------------------------------------------------
        // 5. BILLS & PAYMENTS (12 Months per meter: Nov 2025 to Oct 2026)
        // ----------------------------------------------------
        var bills = new List<Bill>();
        var payments = new List<Payment>();
        int pmtIndex = 1;

        // 12 months array
        var months = new List<(int Year, int Month)>
        {
            (2025, 11), (2025, 12), (2026, 1), (2026, 2), (2026, 3), (2026, 4),
            (2025, 5),  (2026, 6),  (2026, 7), (2026, 8), (2026, 9), (2026, 10)
        };

        foreach (var m in meters)
        {
            double runningReading = m.LastReading - 2800.0;
            if (runningReading < 100) runningReading = 500.0;

            for (int mIndex = 0; mIndex < 12; mIndex++)
            {
                var (yr, mo) = months[mIndex];
                var billMonthStr = $"{yr}-{mo:D2}";
                var genDate = new DateTime(yr, mo, 1, 0, 0, 0, DateTimeKind.Utc).AddMonths(1); // 1st of next month
                var dueDate = genDate.AddDays(15);

                // Deterministic units consumed based on meter & month
                double units = (m.TariffId == "TRF-COM")
                    ? (280 + ((mIndex * 19 + int.Parse(m.Id.Replace("MTR-", "")) * 3) % 180))
                    : (140 + ((mIndex * 13 + int.Parse(m.Id.Replace("MTR-", "")) * 5) % 150));

                // USR-006 MTR-1001 for 2026-10 is the exact worked example from the brief (250 units)
                if (m.Id == "MTR-1001" && billMonthStr == "2026-10")
                {
                    units = 250.0;
                }

                double prevRdg = runningReading;
                double currRdg = runningReading + units;
                runningReading = currRdg;

                // Calculate energy charge strictly according to slabs
                decimal energyCharge = CalculateEnergyCharge(units, m.TariffId);
                decimal fixedCharge = (m.TariffId == "TRF-COM") ? 250.00m : 120.00m;
                decimal taxRate = (m.TariffId == "TRF-COM") ? 0.18m : 0.05m;
                decimal taxAmount = Math.Round((energyCharge + fixedCharge) * taxRate, 2);
                decimal totalAmount = energyCharge + fixedCharge + taxAmount;

                // Status distribution
                string status;
                DateTime? paidDate = null;
                if (mIndex <= 9) // Months 1-10: All Paid
                {
                    status = "Paid";
                    paidDate = genDate.AddDays(rnd.Next(3, 12));
                }
                else if (mIndex == 10) // Month 11: 85% Paid, 15% Pending
                {
                    status = (rnd.Next(100) < 85) ? "Paid" : "Pending";
                    if (status == "Paid") paidDate = genDate.AddDays(rnd.Next(4, 11));
                }
                else // Month 12 (latest: 2026-10): mix
                {
                    if (m.Id == "MTR-1001") // Demo meter is Paid
                    {
                        status = "Paid";
                        paidDate = new DateTime(2026, 11, 10, 14, 30, 0, DateTimeKind.Utc);
                    }
                    else
                    {
                        int r = rnd.Next(100);
                        if (r < 50) { status = "Paid"; paidDate = genDate.AddDays(rnd.Next(3, 10)); }
                        else if (r < 85) { status = "Pending"; }
                        else { status = "Overdue"; }
                    }
                }

                var billId = $"BILL-{yr}{mo:D2}-{m.Id}";
                var bill = new Bill
                {
                    Id = billId,
                    UserId = m.OwnerId,
                    MeterId = m.Id,
                    ZoneId = m.ZoneId,
                    BillingMonth = billMonthStr,
                    GeneratedOn = genDate,
                    DueDate = dueDate,
                    PreviousReading = Math.Round(prevRdg, 1),
                    CurrentReading = Math.Round(currRdg, 1),
                    TotalUnits = Math.Round(units, 1),
                    EnergyCharge = energyCharge,
                    FixedCharge = fixedCharge,
                    TaxAmount = taxAmount,
                    Amount = totalAmount,
                    Status = status,
                    PaidOn = paidDate
                };
                bills.Add(bill);

                // If Paid, create matching Payment record
                if (status == "Paid" && paidDate.HasValue)
                {
                    payments.Add(new Payment
                    {
                        Id = $"PMT-{pmtIndex:D5}",
                        BillId = billId,
                        UserId = m.OwnerId,
                        AmountPaid = totalAmount,
                        Method = (pmtIndex % 3 == 0) ? "Card" : (pmtIndex % 2 == 0) ? "NetBanking" : "UPI",
                        TransactionRef = $"TXN-{yr}{mo:D2}-{pmtIndex:D4}",
                        Status = "Success",
                        PaidOn = paidDate.Value
                    });
                    pmtIndex++;
                }
            }
        }
        context.Bills.AddRange(bills);
        context.Payments.AddRange(payments);
        context.SaveChanges();

        // ----------------------------------------------------
        // 6. BILLDAYS (365 Daily Rows for Demo Meter MTR-1001 across all 12 Months)
        // ----------------------------------------------------
        var demoBills = bills.Where(b => b.MeterId == "MTR-1001").OrderBy(b => b.BillingMonth).ToList();
        var billDays = new List<BillDay>();

        foreach (var bill in demoBills)
        {
            var parts = bill.BillingMonth.Split('-');
            int y = int.Parse(parts[0]);
            int mo = int.Parse(parts[1]);
            int daysInMo = DateTime.DaysInMonth(y, mo);

            double targetUnits = bill.TotalUnits;
            decimal targetCost = bill.EnergyCharge;
            double assignedUnits = 0;
            decimal assignedCost = 0;
            double curRdg = bill.PreviousReading;

            for (int day = 1; day <= daysInMo; day++)
            {
                double dailyUnits;
                decimal dailyCost;

                if (day == daysInMo) // Exact remainder to satisfy sum rule
                {
                    dailyUnits = Math.Round(targetUnits - assignedUnits, 1);
                    dailyCost = targetCost - assignedCost;
                }
                else
                {
                    double avg = targetUnits / daysInMo;
                    dailyUnits = Math.Round(avg + ((day % 5) - 2) * 0.7, 1);
                    if (dailyUnits < 1.0) dailyUnits = 1.0;
                    dailyCost = Math.Round((decimal)dailyUnits * (targetCost / (decimal)targetUnits), 2);
                    assignedUnits += dailyUnits;
                    assignedCost += dailyCost;
                }
                curRdg += dailyUnits;

                billDays.Add(new BillDay
                {
                    Id = $"BDAY-{y}{mo:D2}-{day:D2}",
                    BillId = bill.Id,
                    Date = new DateTime(y, mo, day, 0, 0, 0, DateTimeKind.Utc),
                    Reading = Math.Round(curRdg, 1),
                    Consumption = Math.Round(dailyUnits, 1),
                    Cost = dailyCost
                });
            }
        }
        context.BillDays.AddRange(billDays);
        context.SaveChanges();

        // ----------------------------------------------------
        // 7. METER READINGS (48 Hourly Telemetry Readings for MTR-1001)
        // ----------------------------------------------------
        var readings = new List<MeterReading>();
        var baseTime = DateTime.UtcNow.Date.AddDays(-2);
        double cumReading = 2400.0;
        for (int h = 0; h < 48; h++)
        {
            int hourOfDay = h % 24;
            double cons = (hourOfDay is >= 18 and <= 22) ? 1.4 : (hourOfDay is >= 6 and <= 9) ? 1.0 : 0.4;
            cumReading += cons;
            readings.Add(new MeterReading
            {
                Id = $"RDG-{h:D3}",
                MeterId = "MTR-1001",
                Timestamp = baseTime.AddHours(h),
                Granularity = "hourly",
                Reading = Math.Round(cumReading, 2),
                Consumption = Math.Round(cons, 2),
                Note = (hourOfDay is >= 18 and <= 22) ? "Peak evening usage" : null
            });
        }
        context.MeterReadings.AddRange(readings);
        context.SaveChanges();

        // ----------------------------------------------------
        // 8. CONSUMPTION SERIES (Enriched with 30-Day Daily Data & Summary Metrics)
        // ----------------------------------------------------
        // 30 days of data for the month view
        var monthLabels = Enumerable.Range(1, 30).Select(d => $"{d:D2} Oct").ToList();
        var currentDaily = new List<double> { 8.2, 7.9, 8.5, 9.1, 10.4, 12.8, 11.5, 8.1, 7.8, 8.4, 9.2, 11.0, 13.5, 12.0, 7.9, 8.2, 8.6, 9.0, 10.2, 11.8, 10.5, 8.0, 7.7, 8.3, 9.5, 11.2, 12.4, 8.5, 8.0, 7.9 };
        var previousDaily = new List<double> { 7.5, 7.6, 8.0, 8.5, 9.8, 11.4, 10.2, 7.8, 7.4, 7.9, 8.7, 10.2, 12.0, 11.1, 7.5, 7.8, 8.1, 8.6, 9.5, 10.8, 9.9, 7.4, 7.2, 7.8, 8.8, 10.5, 11.2, 7.9, 7.6, 7.5 };

        var consumptionSeries = new List<ConsumptionSeries>
        {
            // User Month Series (30 days with Max, Min, Total, Average)
            new() {
                Id = "CS-USR-006-MONTH",
                Scope = "user",
                ScopeId = "USR-006",
                Range = "month",
                Labels = monthLabels,
                Current = currentDaily,
                Previous = previousDaily,
                TotalConsumption = Math.Round(currentDaily.Sum(), 2),
                AverageConsumption = Math.Round(currentDaily.Average(), 2),
                MaxUsage = Math.Round(currentDaily.Max(), 2),
                MinUsage = Math.Round(currentDaily.Min(), 2)
            },
            // User Week Series
            new() {
                Id = "CS-USR-006-WEEK",
                Scope = "user",
                ScopeId = "USR-006",
                Range = "week",
                Labels = new() { "Mon", "Tue", "Wed", "Thu", "Fri", "Sat", "Sun" },
                Current = new() { 8.2, 8.5, 9.1, 10.4, 12.8, 11.5, 8.1 },
                Previous = new() { 7.8, 8.0, 8.5, 9.8, 11.4, 10.2, 7.5 },
                TotalConsumption = 68.6,
                AverageConsumption = 9.8,
                MaxUsage = 12.8,
                MinUsage = 8.1
            },
            // Zone Mangalore Month Series
            new() {
                Id = "CS-ZN-001-MONTH",
                Scope = "zone",
                ScopeId = "ZN-001",
                Range = "month",
                Labels = monthLabels,
                Current = currentDaily.Select(v => Math.Round(v * 14.2, 1)).ToList(),
                Previous = previousDaily.Select(v => Math.Round(v * 13.8, 1)).ToList(),
                TotalConsumption = Math.Round(currentDaily.Sum() * 14.2, 1),
                AverageConsumption = Math.Round(currentDaily.Average() * 14.2, 1),
                MaxUsage = Math.Round(currentDaily.Max() * 14.2, 1),
                MinUsage = Math.Round(currentDaily.Min() * 14.2, 1)
            },
            // Enterprise Year Series
            new() {
                Id = "CS-ALL-YEAR",
                Scope = "enterprise",
                ScopeId = "ALL",
                Range = "year",
                Labels = new() { "Nov 25", "Dec 25", "Jan 26", "Feb 26", "Mar 26", "Apr 26", "May 26", "Jun 26", "Jul 26", "Aug 26", "Sep 26", "Oct 26" },
                Current = new() { 14200, 15100, 14800, 16200, 17500, 18900, 19400, 18200, 17100, 16500, 15800, 16300 },
                Previous = new() { 13500, 14200, 14000, 15100, 16400, 17800, 18200, 17100, 16200, 15400, 14900, 15300 },
                TotalConsumption = 200000,
                AverageConsumption = 16666.7,
                MaxUsage = 19400,
                MinUsage = 14200
            }
        };
        context.ConsumptionSeries.AddRange(consumptionSeries);
        context.SaveChanges();

        // ----------------------------------------------------
        // 9. ALERTS (25 Alerts across all 4 zones)
        // ----------------------------------------------------
        var alertTypes = new[] { "High consumption", "Abnormal reading", "Inactive meter", "Voltage fluctuation", "Tamper detect" };
        var severities = new[] { "Critical", "Warning", "Info" };
        var alertStatuses = new[] { "Pending", "Acknowledged", "Resolved" };
        var alerts = new List<Alert>();

        for (int a = 1; a <= 25; a++)
        {
            var zone = zones[(a - 1) % zones.Count];
            var type = alertTypes[(a * 3) % alertTypes.Length];
            var sev = severities[(a * 2) % severities.Length];
            var stat = alertStatuses[a % alertStatuses.Length];
            var meter = meters.FirstOrDefault(m => m.ZoneId == zone.Id);

            alerts.Add(new Alert
            {
                Id = $"ALT-{a:D3}",
                ZoneId = zone.Id,
                ZoneName = zone.Name,
                MeterId = meter?.Id,
                Type = type,
                Severity = sev,
                Status = stat,
                Message = $"{type} triggered in {zone.Name} on meter {meter?.Id ?? "N/A"}. Value exceeded preset boundary.",
                CreatedAt = DateTime.UtcNow.AddHours(-a * 3)
            });
        }
        context.Alerts.AddRange(alerts);
        context.SaveChanges();

        // ----------------------------------------------------
        // 10. NOTIFICATIONS (20 Notifications)
        // ----------------------------------------------------
        var notifTypes = new[] { "bill_generated", "payment_due", "payment_received", "high_consumption", "maintenance" };
        var notifications = new List<Notification>();

        for (int n = 1; n <= 20; n++)
        {
            var user = customerUsers[(n - 1) % customerUsers.Count];
            var type = notifTypes[(n * 2) % notifTypes.Length];
            bool isRead = (n <= 12); // first 12 read, 8 unread

            string title = type switch
            {
                "bill_generated" => "New Electricity Bill Generated",
                "payment_due" => "Upcoming Bill Payment Reminder",
                "payment_received" => "Payment Confirmation",
                "high_consumption" => "Peak Usage Alert Notice",
                _ => "Scheduled Grid Maintenance"
            };

            notifications.Add(new Notification
            {
                Id = $"NOTIF-{n:D3}",
                UserId = user.Id,
                Type = type,
                Title = title,
                Message = $"Notification regarding your meter service in {user.ZoneName}. Please check your dashboard for details.",
                IsRead = isRead,
                CreatedAt = DateTime.UtcNow.AddDays(-n * 2)
            });
        }
        context.Notifications.AddRange(notifications);
        context.SaveChanges();

        // ----------------------------------------------------
        // 11. AUDIT LOGS (35 Logs)
        // ----------------------------------------------------
        var actions = new[] { "Created zone", "Deactivated meter", "Generated bill", "Updated zone settings", "Modified user profile", "Processed payment", "Acknowledged alert" };
        var auditLogs = new List<AuditLog>();
        var adminList = users.Where(u => u.Role != "end_user").ToList();

        for (int l = 1; l <= 35; l++)
        {
            var admin = adminList[(l - 1) % adminList.Count];
            var act = actions[(l * 3) % actions.Length];
            auditLogs.Add(new AuditLog
            {
                Id = $"AUD-{l:D3}",
                Timestamp = DateTime.UtcNow.AddDays(-l),
                UserId = admin.Id,
                UserName = admin.Name,
                Action = act,
                Resource = $"System Entity #{l * 10}",
                Status = "Success"
            });
        }
        context.AuditLogs.AddRange(auditLogs);
        context.SaveChanges();

        // ----------------------------------------------------
        // 12. ZONE SETTINGS (4 ZoneSettings)
        // ----------------------------------------------------
        var zoneSettings = new List<ZoneSettings>
        {
            new() { Id = "ZST-001", ZoneId = "ZN-001", HighConsumptionThreshold = 35.0, LowConsumptionThreshold = 1.0, AbnormalReadingFrequency = 5.0, InactiveMeterHours = 12.0, NotifyEmail = true, NotifySms = true, NotifyInApp = true },
            new() { Id = "ZST-002", ZoneId = "ZN-002", HighConsumptionThreshold = 30.0, LowConsumptionThreshold = 1.0, AbnormalReadingFrequency = 5.0, InactiveMeterHours = 12.0, NotifyEmail = true, NotifySms = false, NotifyInApp = true },
            new() { Id = "ZST-003", ZoneId = "ZN-003", HighConsumptionThreshold = 40.0, LowConsumptionThreshold = 2.0, AbnormalReadingFrequency = 6.0, InactiveMeterHours = 24.0, NotifyEmail = true, NotifySms = true, NotifyInApp = true },
            new() { Id = "ZST-004", ZoneId = "ZN-004", HighConsumptionThreshold = 25.0, LowConsumptionThreshold = 0.5, AbnormalReadingFrequency = 4.0, InactiveMeterHours = 12.0, NotifyEmail = true, NotifySms = false, NotifyInApp = true }
        };
        context.ZoneSettings.AddRange(zoneSettings);
        context.SaveChanges();

        // ----------------------------------------------------
        // 13. ENTERPRISE SETTINGS (Singleton)
        // ----------------------------------------------------
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

        // ----------------------------------------------------
        // 14. USER STATUS STATS (Donut Chart Data for 2024, 2025, 2026)
        // ----------------------------------------------------
        var userStats = new List<UserStatusStats>
        {
            new() { Id = "USS-2024-ADM", Year = 2024, Group = "admin", ActiveCount = 2, DeactivatedCount = 0 },
            new() { Id = "USS-2024-CON", Year = 2024, Group = "consumer", ActiveCount = 18, DeactivatedCount = 1 },
            new() { Id = "USS-2025-ADM", Year = 2025, Group = "admin", ActiveCount = 4, DeactivatedCount = 0 },
            new() { Id = "USS-2025-CON", Year = 2025, Group = "consumer", ActiveCount = 38, DeactivatedCount = 2 },
            new() { Id = "USS-2026-ADM", Year = 2026, Group = "admin", ActiveCount = 5, DeactivatedCount = 0 },
            new() { Id = "USS-2026-CON", Year = 2026, Group = "consumer", ActiveCount = 48, DeactivatedCount = 0 }
        };
        context.UserStatusStats.AddRange(userStats);
        context.SaveChanges();
    }

    private static decimal CalculateEnergyCharge(double units, string tariffId)
    {
        if (tariffId == "TRF-COM")
        {
            // Commercial: 0-200 @ 6.50, >200 @ 9.00
            if (units <= 200) return (decimal)units * 6.50m;
            return (200m * 6.50m) + ((decimal)(units - 200) * 9.00m);
        }

        // Residential TRF-RES:
        // 0-100 @ 4.00, 101-200 @ 5.50, 201-400 @ 7.00, >400 @ 8.50
        decimal cost = 0m;
        double remaining = units;

        double tier1 = Math.Min(remaining, 100);
        cost += (decimal)tier1 * 4.00m;
        remaining -= tier1;

        if (remaining > 0)
        {
            double tier2 = Math.Min(remaining, 100);
            cost += (decimal)tier2 * 5.50m;
            remaining -= tier2;
        }

        if (remaining > 0)
        {
            double tier3 = Math.Min(remaining, 200);
            cost += (decimal)tier3 * 7.00m;
            remaining -= tier3;
        }

        if (remaining > 0)
        {
            cost += (decimal)remaining * 8.50m;
        }

        return Math.Round(cost, 2);
    }
}
