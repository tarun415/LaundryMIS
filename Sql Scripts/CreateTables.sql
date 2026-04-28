DROP TABLE StateMaster

CREATE TABLE StateMaster
(
    StateId INT IDENTITY(1,1) PRIMARY KEY,
    StateName NVARCHAR(150),
    CountryId INT,
    StateNameHI NVARCHAR(150)
);

CREATE TABLE DistrictMaster (
    DistrictID INT IDENTITY(1,1) PRIMARY KEY,
    DistrictName NVARCHAR(100) NOT NULL,
    StateID INT NOT NULL,
    IsActive BIT DEFAULT 1,
    CreatedDate DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (StateID) REFERENCES StateMaster(StateID)
);

