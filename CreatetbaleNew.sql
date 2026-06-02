




/* =========================================================
   1. PICKUP HEADER
========================================================= */

CREATE TABLE LaundryPickup
(
    PickupId               INT IDENTITY(1,1) PRIMARY KEY,

    PickupNo               NVARCHAR(50) NOT NULL,

    AgreementId            INT NOT NULL,
    HospitalId             INT NOT NULL,
    ProviderId             INT NOT NULL,

    WardId                 INT NULL,

    PickupDateTime         DATETIME NOT NULL DEFAULT GETDATE(),

    ShiftName              NVARCHAR(50),

    PickupBy               NVARCHAR(100),
    ReceivedBy             NVARCHAR(100),

    IsInfected             BIT DEFAULT 0,

    TotalCollectedQty      INT DEFAULT 0,

    SLAHours               INT DEFAULT 24,

    DueDateTime            DATETIME NULL,

    Status                 NVARCHAR(50)
        DEFAULT 'Pending Acceptance',

    Remarks                NVARCHAR(500),

    CreatedBy              INT,
    CreatedOn              DATETIME DEFAULT GETDATE(),

    ClosedOn               DATETIME NULL
);

ALTER TABLE LaundryPickup
ADD CONSTRAINT FK_Pickup_Hospital
FOREIGN KEY (HospitalId)
REFERENCES tbl_Hospitals(HospitalId);

ALTER TABLE LaundryPickup
ADD CONSTRAINT FK_Pickup_Provider
FOREIGN KEY (ProviderId)
REFERENCES tbl_Providers(ProviderId);

ALTER TABLE LaundryPickup
ADD CONSTRAINT FK_Pickup_Agreement
FOREIGN KEY (AgreementId)
REFERENCES ProviderHospitalAgreements(Id);

ALTER TABLE LaundryPickup
ADD CONSTRAINT FK_Pickup_Ward
FOREIGN KEY (WardId)
REFERENCES tbl_Wards(WardId);



/* =========================================================
   2. PICKUP ITEM DETAILS
========================================================= */

CREATE TABLE LaundryPickupItems
(
    PickupItemId           INT IDENTITY(1,1) PRIMARY KEY,

    PickupId               INT NOT NULL,

    LinenTypeId            INT NOT NULL,

    CollectedQty           INT NOT NULL DEFAULT 0,

    DeliveredQty           INT DEFAULT 0,

    PendingQty             AS (CollectedQty - DeliveredQty),

    DamagedQty             INT DEFAULT 0,

    LostQty                INT DEFAULT 0
);

ALTER TABLE LaundryPickupItems
ADD CONSTRAINT FK_PickupItems_Pickup
FOREIGN KEY (PickupId)
REFERENCES LaundryPickup(PickupId)
ON DELETE CASCADE;

ALTER TABLE LaundryPickupItems
ADD CONSTRAINT FK_PickupItems_Linen
FOREIGN KEY (LinenTypeId)
REFERENCES tbl_LinenTypes(LinenTypeId);



/* =========================================================
   3. PICKUP ACCEPTANCE LOG
========================================================= */

CREATE TABLE PickupAcceptanceLog
(
    AcceptanceId           INT IDENTITY(1,1) PRIMARY KEY,

    PickupId               INT NOT NULL,

    AcceptedByUserId       INT NOT NULL,

    AcceptedDateTime       DATETIME DEFAULT GETDATE(),

    Status                 NVARCHAR(50),

    Remarks                NVARCHAR(500)
);

ALTER TABLE PickupAcceptanceLog
ADD CONSTRAINT FK_Acceptance_Pickup
FOREIGN KEY (PickupId)
REFERENCES LaundryPickup(PickupId);

ALTER TABLE PickupAcceptanceLog
ADD CONSTRAINT FK_Acceptance_User
FOREIGN KEY (AcceptedByUserId)
REFERENCES Tbl_Users(UserId);



/* =========================================================
   4. DELIVERY HEADER
========================================================= */

CREATE TABLE DeliveryChallan
(
    DeliveryId             INT IDENTITY(1,1) PRIMARY KEY,

    DeliveryNo             NVARCHAR(50),

    PickupId               INT NOT NULL,

    DeliveryDateTime       DATETIME DEFAULT GETDATE(),

    DeliveredByUserId      INT,

    ReceivedBy             NVARCHAR(100),

    IsPartialDelivery      BIT DEFAULT 1,

    Status                 NVARCHAR(50)
        DEFAULT 'Pending Hospital Verification',

    Remarks                NVARCHAR(500),

    CreatedOn              DATETIME DEFAULT GETDATE()
);

ALTER TABLE DeliveryChallan
ADD CONSTRAINT FK_Delivery_Pickup
FOREIGN KEY (PickupId)
REFERENCES LaundryPickup(PickupId);

ALTER TABLE DeliveryChallan
ADD CONSTRAINT FK_Delivery_User
FOREIGN KEY (DeliveredByUserId)
REFERENCES Tbl_Users(UserId);



/* =========================================================
   5. DELIVERY ITEM DETAILS
========================================================= */

CREATE TABLE DeliveryChallanItems
(
    DeliveryItemId         INT IDENTITY(1,1) PRIMARY KEY,

    DeliveryId             INT NOT NULL,

    LinenTypeId            INT NOT NULL,

    DeliveredQty           INT DEFAULT 0,

    AcceptedQty            INT DEFAULT 0,

    RejectedQty            INT DEFAULT 0,

    DamagedQty             INT DEFAULT 0,

    Remarks                NVARCHAR(500)
);

ALTER TABLE DeliveryChallanItems
ADD CONSTRAINT FK_DeliveryItems_Delivery
FOREIGN KEY (DeliveryId)
REFERENCES DeliveryChallan(DeliveryId)
ON DELETE CASCADE;

ALTER TABLE DeliveryChallanItems
ADD CONSTRAINT FK_DeliveryItems_Linen
FOREIGN KEY (LinenTypeId)
REFERENCES tbl_LinenTypes(LinenTypeId);



/* =========================================================
   6. DELIVERY ACCEPTANCE LOG
========================================================= */

CREATE TABLE DeliveryAcceptanceLog
(
    VerificationId         INT IDENTITY(1,1) PRIMARY KEY,

    DeliveryId             INT NOT NULL,

    VerifiedByUserId       INT NOT NULL,

    VerifiedDateTime       DATETIME DEFAULT GETDATE(),

    Status                 NVARCHAR(50),

    Remarks                NVARCHAR(500)
);

ALTER TABLE DeliveryAcceptanceLog
ADD CONSTRAINT FK_Verification_Delivery
FOREIGN KEY (DeliveryId)
REFERENCES DeliveryChallan(DeliveryId);

ALTER TABLE DeliveryAcceptanceLog
ADD CONSTRAINT FK_Verification_User
FOREIGN KEY (VerifiedByUserId)
REFERENCES Tbl_Users(UserId);


/* =========================================================
   7. LINEN ISSUE / DAMAGE / LOST LOG
========================================================= */

CREATE TABLE LinenIssueLog
(
    IssueId                 INT IDENTITY(1,1) PRIMARY KEY,

    PickupId                INT NOT NULL,

    DeliveryId              INT NULL,

    LinenTypeId             INT NOT NULL,

    IssueType               NVARCHAR(50),
    /*
        Possible Values:
        ----------------
        Damaged
        Lost
        Missing
        Rejected
        Stained
        Torn
    */

    IssueQty                INT NOT NULL DEFAULT 0,

    ReportedByUserId        INT NOT NULL,

    ReportedDateTime        DATETIME DEFAULT GETDATE(),

    ResponsibleParty        NVARCHAR(50),
    /*
        Possible Values:
        ----------------
        Hospital
        Provider
        Unknown
    */

    PenaltyAmount           DECIMAL(18,2) DEFAULT 0,

    IsResolved              BIT DEFAULT 0,

    ResolvedDateTime        DATETIME NULL,

    ResolutionRemarks       NVARCHAR(1000),

    Status                  NVARCHAR(50)
        DEFAULT 'Open',
    /*
        Possible Values:
        ----------------
        Open
        Under Review
        Resolved
        Closed
    */

    AttachmentPath          NVARCHAR(500),

    CreatedOn               DATETIME DEFAULT GETDATE()
);



/* =========================================================
   FOREIGN KEYS
========================================================= */

ALTER TABLE LinenIssueLog
ADD CONSTRAINT FK_Issue_Pickup
FOREIGN KEY (PickupId)
REFERENCES LaundryPickup(PickupId);

ALTER TABLE LinenIssueLog
ADD CONSTRAINT FK_Issue_Delivery
FOREIGN KEY (DeliveryId)
REFERENCES DeliveryChallan(DeliveryId);

ALTER TABLE LinenIssueLog
ADD CONSTRAINT FK_Issue_Linen
FOREIGN KEY (LinenTypeId)
REFERENCES tbl_LinenTypes(LinenTypeId);

ALTER TABLE LinenIssueLog
ADD CONSTRAINT FK_Issue_User
FOREIGN KEY (ReportedByUserId)
REFERENCES Tbl_Users(UserId);



