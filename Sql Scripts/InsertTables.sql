




-------------
INSERT INTO StateMaster (StateName)
VALUES ('Uttar Pradesh');
INSERT INTO DistrictMaster (DistrictName, StateID)
VALUES
('Lucknow', 1),
('Kanpur Nagar', 1),
('Agra', 1),
('Varanasi', 1),
('Prayagraj', 1);
INSERT INTO CityMaster (CityName, DistrictID)
VALUES
('Lucknow', 1),
('Malihabad', 1),
('Kanpur', 2),
('Kalyanpur', 2),
('Agra City', 3),
('Varanasi City', 4);


------------------------

INSERT INTO tbl_Providers
(ProviderName, FirmName, NoOfBeds, RatePerBed, CreatedDate, CreatedDBY, IsActive)
VALUES
('M/s Maa Kanakdurga Enterprises', 'Maa Kanakdurga Enterprises', 1625, 12540, GETDATE(), 'Admin', 1),
('M/s Cantt Dry Cleaner', 'Cantt Dry Cleaner', 1815, 12540, GETDATE(), 'Admin', 1),
('M/s Shree Enterprises', 'Shree Enterprises', 2431, 12540, GETDATE(), 'Admin', 1),
('M/s Mr. Johnny Care Services India Pvt. Ltd.', 'Mr. Johnny Care Services India Pvt. Ltd.', 3384, 12540, GETDATE(), 'Admin', 1);