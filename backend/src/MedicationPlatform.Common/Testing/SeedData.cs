using MedicationPlatform.Common.Domain;

namespace MedicationPlatform.Common.Testing;

public static class SeedData
{
    public static readonly Guid AcetaminophenId = Guid.Parse("11111111-1111-1111-1111-111111111111");
    public static readonly Guid MorphineId = Guid.Parse("22222222-2222-2222-2222-222222222222");
    public static readonly Guid InsulinId = Guid.Parse("33333333-3333-3333-3333-333333333333");

    public static readonly Guid CentralPharmacyId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa");
    public static readonly Guid EmergencyDepartmentId = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb");

    public static IReadOnlyList<Medication> Medications =>
    [
        new Medication(AcetaminophenId, "00045-0123-01", "Acetaminophen", "500 mg", "Tablet", false, false),
        new Medication(MorphineId, "00409-1765-01", "Morphine Sulfate", "2 mg/mL", "Injection", true, false),
        new Medication(InsulinId, "0169-1833-11", "Insulin Glargine", "100 units/mL", "Pen", false, true)
    ];

    public static IReadOnlyList<InventoryLocation> Locations =>
    [
        new InventoryLocation(CentralPharmacyId, "Central Pharmacy", "Pharmacy"),
        new InventoryLocation(EmergencyDepartmentId, "Emergency Department Cabinet", "Care Area")
    ];

    public static IReadOnlyList<StockItem> Stock =>
    [
        new StockItem(AcetaminophenId, CentralPharmacyId, 240, 80, DateOnly.FromDateTime(DateTime.UtcNow.AddMonths(16)), "LOT-ACET-240"),
        new StockItem(MorphineId, CentralPharmacyId, 35, 20, DateOnly.FromDateTime(DateTime.UtcNow.AddMonths(8)), "LOT-MOR-035"),
        new StockItem(InsulinId, CentralPharmacyId, 18, 10, DateOnly.FromDateTime(DateTime.UtcNow.AddMonths(5)), "LOT-INS-018"),
        new StockItem(AcetaminophenId, EmergencyDepartmentId, 72, 25, DateOnly.FromDateTime(DateTime.UtcNow.AddMonths(14)), "LOT-ACET-072"),
        new StockItem(MorphineId, EmergencyDepartmentId, 12, 10, DateOnly.FromDateTime(DateTime.UtcNow.AddMonths(6)), "LOT-MOR-012"),
        new StockItem(InsulinId, EmergencyDepartmentId, 7, 8, DateOnly.FromDateTime(DateTime.UtcNow.AddMonths(4)), "LOT-INS-007")
    ];
}
