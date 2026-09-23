-- Bill approval flow ek hi rakha gaya hai:
--   Draft -> HospitalSubmitted -> HospitalApproved -> CMSApproved / CMSRejected
--   (HospitalRejected / CMSRejected -> Provider edit karke dobara submit karta hai)
--
-- Purana "Submitted" status (Provider -> seedha CMS) ab use nahi hota.
-- Aise bills ko Hospital verification queue mein wapas bhejo.

-- 1. Pehle dekho kitne bills affected hain
SELECT Id, HospitalId, ProviderId, BillingMonth, BillingYear, Status
FROM MonthlyBills
WHERE Status = 'Submitted';

-- 2. Update + workflow log
BEGIN TRAN;

INSERT INTO BillWorkflowLog (BillId, FromStatus, ToStatus, ActionBy, ActionAt, Remarks)
SELECT Id, 'Submitted', 'HospitalSubmitted', CreatedBy, GETDATE(),
       'Approval flow migration: Hospital verification ke liye bheja'
FROM MonthlyBills
WHERE Status = 'Submitted';

UPDATE MonthlyBills
SET Status = 'HospitalSubmitted'
WHERE Status = 'Submitted';

COMMIT;
