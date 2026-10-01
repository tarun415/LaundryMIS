/* =========================================================
   Admin approval for Providers and Hospitals

   ApprovalStatus : Pending | Approved | Rejected
   Existing rows are marked Approved so nothing changes for them.
   New rows created from the app start as Pending.
========================================================= */

IF COL_LENGTH('tbl_Providers', 'ApprovalStatus') IS NULL
    ALTER TABLE tbl_Providers ADD
        ApprovalStatus  NVARCHAR(20)  NOT NULL CONSTRAINT DF_Providers_ApprovalStatus DEFAULT 'Approved',
        ApprovalRemarks NVARCHAR(500) NULL,
        ApprovedBy      INT           NULL,
        ApprovedOn      DATETIME      NULL;
GO

IF COL_LENGTH('tbl_Hospitals', 'ApprovalStatus') IS NULL
    ALTER TABLE tbl_Hospitals ADD
        ApprovalStatus  NVARCHAR(20)  NOT NULL CONSTRAINT DF_Hospitals_ApprovalStatus DEFAULT 'Approved',
        ApprovalRemarks NVARCHAR(500) NULL,
        ApprovedBy      INT           NULL,
        ApprovedOn      DATETIME      NULL;
GO
