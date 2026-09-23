-- Missing tables jo code use karta hai (MonthlyBillRepository, PaymentRepository)
-- Script dobara chalana safe hai: sirf tab create karta hai jab table nahi hai.

IF OBJECT_ID('dbo.BillWorkflowLog', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.BillWorkflowLog
    (
        Id         INT IDENTITY(1,1) PRIMARY KEY,
        BillId     INT            NOT NULL,
        FromStatus NVARCHAR(50)   NULL,
        ToStatus   NVARCHAR(50)   NOT NULL,
        ActionBy   INT            NOT NULL,
        ActionAt   DATETIME       NOT NULL CONSTRAINT DF_BillWorkflowLog_ActionAt DEFAULT (GETDATE()),
        Remarks    NVARCHAR(1000) NULL,
        CONSTRAINT FK_BillWorkflowLog_MonthlyBills
            FOREIGN KEY (BillId) REFERENCES dbo.MonthlyBills (Id)
    );

    CREATE INDEX IX_BillWorkflowLog_BillId ON dbo.BillWorkflowLog (BillId);
END
GO

IF OBJECT_ID('dbo.PaymentCalculation', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.PaymentCalculation
    (
        Id          INT IDENTITY(1,1) PRIMARY KEY,
        PaymentId   INT            NOT NULL,
        Description NVARCHAR(500)  NOT NULL,
        Amount      DECIMAL(18,2)  NOT NULL,
        CONSTRAINT FK_PaymentCalculation_PaymentMaster
            FOREIGN KEY (PaymentId) REFERENCES dbo.PaymentMaster (PaymentId)
    );

    CREATE INDEX IX_PaymentCalculation_PaymentId ON dbo.PaymentCalculation (PaymentId);
END
GO
