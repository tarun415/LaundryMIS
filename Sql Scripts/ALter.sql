





-- Increase FirmName size
ALTER TABLE tbl_Providers
Add  FirmName NVARCHAR(150);

ALTER TABLE tbl_Providers
ADD NoOfBeds INT NULL;

ALTER TABLE tbl_Providers
ADD RatePerBed INT NULL;


ALTER TABLE tbl_Providers
 Add CreatedDate DATETIME NOT NULL;

 
ALTER TABLE tbl_Providers
 Add CreatedDBY NVARCHAR(50) NOT NULL;

-- Add default values
ALTER TABLE tbl_Providers
ADD DEFAULT GETDATE() FOR CreatedDate;





