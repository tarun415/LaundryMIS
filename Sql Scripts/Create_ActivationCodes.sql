-- One-time activation codes. A hospital or vendor that is already on the tender list
-- registers by choosing itself from the list and entering the code Admin gave it.
-- Only a SHA-256 hash of the code is stored; the code is shown to Admin once, when issued.
-- Safe to re-run.

IF OBJECT_ID('dbo.ActivationCodes', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.ActivationCodes
    (
        Id           INT IDENTITY(1,1) PRIMARY KEY,
        EntityType   NVARCHAR(10) NOT NULL,                -- 'Hospital' | 'Provider'
        EntityId     INT          NOT NULL,                -- HospitalId / ProviderId
        CodeHash     CHAR(64)     NOT NULL,                -- SHA-256 hex of the normalised code
        IsActive     BIT          NOT NULL CONSTRAINT DF_ActivationCodes_IsActive DEFAULT (1),  -- 0 = replaced by a newer code
        CreatedOn    DATETIME     NOT NULL CONSTRAINT DF_ActivationCodes_CreatedOn DEFAULT (GETDATE()),
        CreatedBy    INT          NULL,                    -- Admin user id
        UsedOn       DATETIME     NULL,
        UsedByUserId INT          NULL,
        CONSTRAINT CK_ActivationCodes_EntityType CHECK (EntityType IN ('Hospital', 'Provider'))
    );

    CREATE UNIQUE INDEX UX_ActivationCodes_CodeHash ON dbo.ActivationCodes (CodeHash);
    CREATE INDEX IX_ActivationCodes_Entity ON dbo.ActivationCodes (EntityType, EntityId, IsActive);
END
GO
