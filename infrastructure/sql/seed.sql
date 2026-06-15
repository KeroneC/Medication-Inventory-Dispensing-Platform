INSERT INTO dbo.Medications
    (MedicationId, Ndc, Name, Strength, Form, IsControlledSubstance, RequiresRefrigeration)
VALUES
    ('11111111-1111-1111-1111-111111111111', '00045-0123-01', 'Acetaminophen', '500 mg', 'Tablet', 0, 0),
    ('22222222-2222-2222-2222-222222222222', '00409-1765-01', 'Morphine Sulfate', '2 mg/mL', 'Injection', 1, 0),
    ('33333333-3333-3333-3333-333333333333', '0169-1833-11', 'Insulin Glargine', '100 units/mL', 'Pen', 0, 1);

INSERT INTO dbo.InventoryLocations
    (LocationId, Name, Type)
VALUES
    ('aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa', 'Central Pharmacy', 'Pharmacy'),
    ('bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb', 'Emergency Department Cabinet', 'Care Area');

INSERT INTO dbo.StockItems
    (MedicationId, LocationId, QuantityOnHand, ReorderLevel, ExpirationDate, LotNumber)
VALUES
    ('11111111-1111-1111-1111-111111111111', 'aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa', 240, 80, '2027-10-15', 'LOT-ACET-240'),
    ('22222222-2222-2222-2222-222222222222', 'aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa', 35, 20, '2027-02-15', 'LOT-MOR-035'),
    ('33333333-3333-3333-3333-333333333333', 'aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa', 18, 10, '2026-11-15', 'LOT-INS-018'),
    ('11111111-1111-1111-1111-111111111111', 'bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb', 72, 25, '2027-08-15', 'LOT-ACET-072'),
    ('22222222-2222-2222-2222-222222222222', 'bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb', 12, 10, '2027-01-15', 'LOT-MOR-012'),
    ('33333333-3333-3333-3333-333333333333', 'bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb', 7, 8, '2026-10-15', 'LOT-INS-007');
