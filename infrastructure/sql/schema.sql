CREATE TABLE dbo.Medications
(
    MedicationId UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    Ndc NVARCHAR(32) NOT NULL,
    Name NVARCHAR(160) NOT NULL,
    Strength NVARCHAR(80) NOT NULL,
    Form NVARCHAR(80) NOT NULL,
    IsControlledSubstance BIT NOT NULL,
    RequiresRefrigeration BIT NOT NULL
);

CREATE TABLE dbo.InventoryLocations
(
    LocationId UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    Name NVARCHAR(160) NOT NULL,
    Type NVARCHAR(80) NOT NULL
);

CREATE TABLE dbo.StockItems
(
    MedicationId UNIQUEIDENTIFIER NOT NULL,
    LocationId UNIQUEIDENTIFIER NOT NULL,
    QuantityOnHand INT NOT NULL,
    ReorderLevel INT NOT NULL,
    ExpirationDate DATE NOT NULL,
    LotNumber NVARCHAR(80) NOT NULL,
    CONSTRAINT PK_StockItems PRIMARY KEY (MedicationId, LocationId),
    CONSTRAINT FK_StockItems_Medications FOREIGN KEY (MedicationId) REFERENCES dbo.Medications(MedicationId),
    CONSTRAINT FK_StockItems_Locations FOREIGN KEY (LocationId) REFERENCES dbo.InventoryLocations(LocationId),
    CONSTRAINT CK_StockItems_QuantityOnHand CHECK (QuantityOnHand >= 0),
    CONSTRAINT CK_StockItems_ReorderLevel CHECK (ReorderLevel >= 0)
);

CREATE TABLE dbo.DispenseRecords
(
    DispenseId UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    MedicationId UNIQUEIDENTIFIER NOT NULL,
    LocationId UNIQUEIDENTIFIER NOT NULL,
    QuantityDispensed INT NOT NULL,
    PatientId NVARCHAR(80) NOT NULL,
    RequestedBy NVARCHAR(120) NOT NULL,
    CareArea NVARCHAR(120) NOT NULL,
    OccurredAt DATETIMEOFFSET NOT NULL,
    CONSTRAINT FK_DispenseRecords_Medications FOREIGN KEY (MedicationId) REFERENCES dbo.Medications(MedicationId),
    CONSTRAINT FK_DispenseRecords_Locations FOREIGN KEY (LocationId) REFERENCES dbo.InventoryLocations(LocationId),
    CONSTRAINT CK_DispenseRecords_Quantity CHECK (QuantityDispensed > 0)
);

CREATE TABLE dbo.AuditRecords
(
    AuditRecordId UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    OccurredAt DATETIMEOFFSET NOT NULL,
    Actor NVARCHAR(120) NOT NULL,
    Action NVARCHAR(120) NOT NULL,
    EntityType NVARCHAR(120) NOT NULL,
    EntityId NVARCHAR(120) NOT NULL,
    Summary NVARCHAR(500) NOT NULL
);

CREATE TABLE dbo.NotificationMessages
(
    NotificationId UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    CreatedAt DATETIMEOFFSET NOT NULL,
    Severity NVARCHAR(40) NOT NULL,
    Title NVARCHAR(160) NOT NULL,
    Body NVARCHAR(500) NOT NULL,
    Acknowledged BIT NOT NULL
);
