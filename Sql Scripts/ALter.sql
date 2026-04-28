





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

Alter Table tbl_Wards Add CreatedDate DateTime default getdate()

Alter Table tbl_Wards  Add IsActive bit default 1 
Alter Table  DistrictMaster  Add DistrictNameHI nvarchar(250) null;

Alter Table Tbl_Users Add Username nvarchar(150) null;

Alter Table Tbl_Hospitals  drop column city
Alter Table Tbl_Hospitals Add DistrictId int null 





