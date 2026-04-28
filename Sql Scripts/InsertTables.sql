




-------------

INSERT INTO StateMaster (StateName, CountryId, StateNameHI)
VALUES
('Andaman and Nicobar Island',1,N'अंडमान और निकोबार द्वीप'),
('Andhra Pradesh',1,N'आंध्र प्रदेश'),
('Arunachal Pradesh',1,N'अरुणाचल प्रदेश'),
('Assam',1,N'असम'),
('Bihar',1,N'बिहार'),
('Chandigarh',1,N'चंडीगढ़'),
('Chhattisgarh',1,N'छत्तीसगढ़'),
('Dadra and Nagar Haveli',1,N'दादरा और नगर हवेली'),
('Daman and Diu',1,N'दमन और दीव'),
('Delhi',1,N'दिल्ली'),
('Goa',1,N'गोआ'),
('Gujarat',1,N'गुजरात'),
('Haryana',1,N'हरियाणा'),
('Himachal Pradesh',1,N'हिमाचल प्रदेश'),
('Jammu and Kashmir',1,N'जम्मू और कश्मीर'),
('Jharkhand',1,N'झारखंड'),
('Karnataka',1,N'कर्नाटक'),
('Kerala',1,N'केरल'),
('Lakshadweep',1,N'लक्षद्वीप'),
('Madhya Pradesh',1,N'मध्य प्रदेश'),
('Maharashtra',1,N'महाराष्ट्र'),
('Manipur',1,N'मणिपुर'),
('Meghalaya',1,N'मेघालय'),
('Mizoram',1,N'मिज़ोरम'),
('Nagaland',1,N'नागालैंड'),
('Odisha',1,N'ओडिशा'),
('Puducherry',1,N'पुडुचेरी'),
('Punjab',1,N'पंजाब'),
('Rajasthan',1,N'राजस्थान'),
('Sikkim',1,N'सिक्किम'),
('Tamil Nadu',1,N'तमिलनाडु'),
('Telangana',1,N'तेलंगाना'),
('Tripura',1,N'त्रिपुरा'),
('Uttar Pradesh',1,N'उत्तर प्रदेश'),
('Uttarakhand',1,N'उत्तराखंड'),
('West Bengal',1,N'पश्चिम बंगाल');

---------------------------------



Truncate Table [DistrictMaster]



GO
SET IDENTITY_INSERT [dbo].[DistrictMaster] ON 
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (1, N'Nicobar', 1, N'निकोबार')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (2, N'North and Middle Andaman', 1, N'उत्तर एवं मध्य अंडमान')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (3, N'South Andaman', 1, N'दक्षिण अंडमान')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (4, N'Anantapur', 2, N'अनंतपुर')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (5, N'Chittoor', 2, N'चित्तूर')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (6, N'Cuddapah', 2, N'कडपा')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (7, N'East Godavari', 2, N'पूर्वी गोदावरी')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (8, N'Guntur', 2, N'गुंटूर')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (9, N'Krishna', 2, N'कृष्णा')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (10, N'Kurnool', 2, N'कुरनूल')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (11, N'Nellore', 2, N'नेल्लोर')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (12, N'Prakasam', 2, N'प्रकाशम')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (13, N'Srikakulam', 2, N'श्रीकाकुलम')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (14, N'Visakhapatnam', 2, N'विशाखापत्तनम')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (15, N'Vizianagaram', 2, N'विजयनगरम')
GO







INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (16, N'West Godavari', 2, N'पश्चिम गोदावरी')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (17, N'Anjaw', 3, N'अन्जाव')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (18, N'Changlang', 3, N'चांगलांग')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (19, N'Dibang Valley', 3, N'दिबांग घाटी')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (20, N'East Kameng', 3, N'पूर्वी कामेंग')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (21, N'East Siang', 3, N'पूर्वी सियांग')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (22, N'Kurung Kumey', 3, N'कुरुंग कुमेय')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (23, N'Lohit', 3, N'लोहित')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (24, N'Longding', 3, N'लोंगडिंग')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (25, N'Lower Dibang Valley', 3, N'लोअर दिबांग घाटी')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (26, N'Lower Subansiri', 3, N'लोअर सुबानसिरी')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (27, N'Papum Pare', 3, N'पापम पेरे')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (28, N'Tawang', 3, N'तवांग')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (29, N'Tirap', 3, N'तिरप')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (30, N'Upper Siang', 3, N'ऊपरी सियांग')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (31, N'Upper Subansiri', 3, N'अपर सुबानसिरी')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (32, N'West Kameng', 3, N'पश्चिम कामेंग')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (33, N'West Siang', 3, N'पश्चिम सियांग')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (34, N'Baksa', 4, N'बक्सा')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (35, N'Barpeta', 4, N'बारपेटा')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (36, N'Bongaigaon', 4, N'बोंगईगांव')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (37, N'Cachar', 4, N'कछार')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (38, N'Chirang', 4, N'चिरांग')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (39, N'Darrang', 4, N'दरांग')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (40, N'Dhemaji', 4, N'धेमाजी')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (41, N'Dhubri', 4, N'धुबरी')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (42, N'Dibrugarh', 4, N'डिब्रूगढ़')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (43, N'Dima Hasao', 4, N'दिमा हासाओ')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (44, N'Goalpara', 4, N'गोलपाड़ा')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (45, N'Golaghat', 4, N'गोलाघाट')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (46, N'Hailakandi', 4, N'हैलाकांडी')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (47, N'Jorhat', 4, N'जोरहाट')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (48, N'Kamrup Metropolitan', 4, N'कामरूप मेट्रोपॉलिटन')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (49, N'Kamrup', 4, N'कामरूप')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (50, N'Karbi Anglong', 4, N'कार्बी आंग्लोंग')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (51, N'Karimganj', 4, N'करीमगंज')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (52, N'Kokrajhar', 4, N'कोकराझार')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (53, N'Lakhimpur', 4, N'लखीमपुर')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (54, N'Morigaon', 4, N'मोरीगांव')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (55, N'Nagaon', 4, N'नगांव')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (56, N'Nalbari', 4, N'नलबाड़ी')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (57, N'Sivasagar', 4, N'शिवसागर')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (58, N'Sonitpur', 4, N'सोनितपुर')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (59, N'Tinsukia', 4, N'तिनसुकिया')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (60, N'Udalguri', 4, N'उदलगुड़ी')
GO





INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (61, N'Araria', 5, N'अररिया')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (62, N'Arwal', 5, N'अरवल')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (63, N'Aurangabad', 5, N'औरंगाबाद')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (64, N'Banka', 5, N'बांका')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (65, N'Begusarai', 5, N'बेगूसराय')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (66, N'Bhagalpur', 5, N'भागलपुर')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (67, N'Bhojpur', 5, N'भोजपुर')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (68, N'Buxar', 5, N'बक्सर')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (69, N'Darbhanga', 5, N'दरभंगा')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (70, N'East Champaran (Motihari)', 5, N'पूर्वी चंपारण (मोतिहारी)')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (71, N'Gaya', 5, N'गया')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (72, N'Gopalganj', 5, N'गोपालगंज')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (73, N'Jamui', 5, N'जमुई')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (74, N'Jehanabad', 5, N'जहानाबाद')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (75, N'Kaimur (Bhabua)', 5, N'काइमूर (भबुआ)')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (76, N'Katihar', 5, N'कटिहार')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (77, N'Khagaria', 5, N'खगरिया')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (78, N'Kishanganj', 5, N'किशनगंज')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (79, N'Lakhisarai', 5, N'लखीसराय')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (80, N'Madhepura', 5, N'मधेपुरा')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (81, N'Madhubani', 5, N'मधुबनी')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (82, N'Munger (Monghyr)', 5, N'मुंगेर (मंगहिर)')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (83, N'Muzaffarpur', 5, N'मुजफ्फरपुर')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (84, N'Nalanda', 5, N'नालंदा')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (85, N'Nawada', 5, N'नवादा')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (86, N'Patna', 5, N'पटना')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (87, N'Purnia (Purnea)', 5, N'पूर्णिया (पूर्णिया)')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (88, N'Rohtas', 5, N'रोहतास')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (89, N'Saharsa', 5, N'सहरसा')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (90, N'Samastipur', 5, N'समस्तीपुर')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (91, N'Saran', 5, N'सरन')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (92, N'Sheikhpura', 5, N'शेखपुरा')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (93, N'Sheohar', 5, N'शिवहर')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (94, N'Sitamarhi', 5, N'सीतामढ़ी')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (95, N'Siwan', 5, N'सिवान')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (96, N'Supaul', 5, N'सुपौल')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (97, N'Vaishali', 5, N'वैशाली')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (98, N'West Champaran', 5, N'पश्चिम चंपारण')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (99, N'Chandigarh', 6, N'चंडीगढ़')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (100, N'Balod', 7, N'बालोद')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (101, N'Baloda Bazar', 7, N'बलोदा बाज़ार')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (102, N'Balrampur', 7, N'बलरामपुर')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (103, N'Bastar', 7, N'बस्तर')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (104, N'Bemetara', 7, N'बेमेतरा')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (105, N'Bijapur', 7, N'बीजापुर')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (106, N'Bilaspur', 7, N'बिलासपुर')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (107, N'Dantewada (South Bastar)', 7, N'दंतेवाड़ा (दक्षिण बस्तर)')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (108, N'Dhamtari', 7, N'धमतरी')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (109, N'Durg', 7, N'दुर्ग')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (110, N'Gariaband', 7, N'गरियाबंद')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (111, N'Janjgir-Champa', 7, N'जांजगीर-चंपा')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (112, N'Jashpur', 7, N'जशपुर')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (113, N'Kabirdham (Kawardha)', 7, N'कबीरद्धम (कवर्धा)')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (114, N'Kanker (North Bastar)', 7, N'कांकर (उत्तरी बस्तर)')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (115, N'Kondagaon', 7, N'कोंडागांव')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (116, N'Korba', 7, N'कोरबा')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (117, N'Korea (Koriya)', 7, N'कोरिया (कोरिया)')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (118, N'Mahasamund', 7, N'महासमुंद')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (119, N'Mungeli', 7, N'मुंगेली')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (120, N'Narayanpur', 7, N'नारायणपुर')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (121, N'Raigarh', 7, N'रायगढ़')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (122, N'Raipur', 7, N'रायपुर')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (123, N'Rajnandgaon', 7, N'राजनंदगांव')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (124, N'Sukma', 7, N'सुकमा')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (125, N'Surajpur', 7, N'सूरजपुर')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (126, N'Surguja', 7, N'सरगुजा')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (127, N'Dadra & Nagar Haveli', 8, N'दादरा एवं नगर हवेली')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (128, N'Daman', 9, N'दमन')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (129, N'Diu', 9, N'दीव')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (130, N'Central Delhi', 10, N'केंद्रीय दिल्ली')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (131, N'East Delhi', 10, N'पूर्व दिल्ली')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (132, N'New Delhi', 10, N'नई दिल्ली')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (133, N'North Delhi', 10, N'उत्तर दिल्ली')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (134, N'North East Delhi', 10, N'उत्तर पूर्व दिल्ली')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (135, N'North West Delhi', 10, N'उत्तर पश्चिम दिल्ली')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (136, N'South Delhi', 10, N'दक्षिण दिल्ली')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (137, N'South West Delhi', 10, N'दक्षिण पश्चिमी दिल्ली')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (138, N'West Delhi', 10, N'पश्चिम दिल्ली')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (139, N'North Goa', 11, N'उत्तरी गोवा')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (140, N'South Goa', 11, N'दक्षिण गोवा')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (141, N'Ahmedabad', 12, N'अहमदाबाद')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (142, N'Amreli', 12, N'अमरेली')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (143, N'Anand', 12, N'आनंद')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (144, N'Aravalli', 12, N'अरावली')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (145, N'Banaskantha (Palanpur)', 12, N'बनासकांथा (पालनपुर)')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (146, N'Bharuch', 12, N'भरूच')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (147, N'Bhavnagar', 12, N'भावनगर')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (148, N'Botad', 12, N'बोटाड')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (149, N'Chhota Udepur', 12, N'छोटा उदयपुर')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (150, N'Dahod', 12, N'दाहोद')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (151, N'Dangs (Ahwa)', 12, N'डेंग्स (आह)')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (152, N'Devbhoomi Dwarka', 12, N'देवभूमि द्वारिका')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (153, N'Gandhinagar', 12, N'गांधीनगर')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (154, N'Gir Somnath', 12, N'गिर सोमनाथ')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (155, N'Jamnagar', 12, N'जामनगर')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (156, N'Junagadh', 12, N'जूनागढ़')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (157, N'Kachchh', 12, N'कच्छ')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (158, N'Kheda (Nadiad)', 12, N'खेड़ा (नडियाद)')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (159, N'Mahisagar', 12, N'महिसागर')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (160, N'Mehsana', 12, N'मेहसाणा')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (161, N'Morbi', 12, N'मोरबी')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (162, N'Narmada (Rajpipla)', 12, N'नर्मदा (राजपिपला)')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (163, N'Navsari', 12, N'नवसारी')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (164, N'Panchmahal (Godhra)', 12, N'पंचमहल (गोधरा)')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (165, N'Patan', 12, N'पाटन')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (166, N'Porbandar', 12, N'पोरबंदर')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (167, N'Rajkot', 12, N'राजकोट')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (168, N'Sabarkantha (Himmatnagar)', 12, N'साबरकांठा (हिम्मतनगर)')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (169, N'Surat', 12, N'सूरत')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (170, N'Surendranagar', 12, N'सुरेंद्रनगर')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (171, N'Tapi (Vyara)', 12, N'तापी (व्यारा)')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (172, N'Vadodara', 12, N'वडोदरा')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (173, N'Valsad', 12, N'वलसाड')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (174, N'Ambala', 13, N'अंबाला')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (175, N'Bhiwani', 13, N'भिवानी')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (176, N'Faridabad', 13, N'फरीदाबाद')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (177, N'Fatehabad', 13, N'फतेहाबाद')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (178, N'Gurgaon', 13, N'गुरुग्राम')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (179, N'Hisar', 13, N'हिसार')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (180, N'Jhajjar', 13, N'झज्जर')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (181, N'Jind', 13, N'जींद')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (182, N'Kaithal', 13, N'कैथल')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (183, N'Karnal', 13, N'करनाल')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (184, N'Kurukshetra', 13, N'कुरुक्षेत्र')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (185, N'Mahendragarh', 13, N'महेंद्रगढ़')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (186, N'Mewat', 13, N'मेवात')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (187, N'Palwal', 13, N'पलवल')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (188, N'Panchkula', 13, N'पंचकुला')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (189, N'Panipat', 13, N'पानीपत')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (190, N'Rewari', 13, N'रेवाड़ी')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (191, N'Rohtak', 13, N'रोहतक')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (192, N'Sirsa', 13, N'सिरसा')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (193, N'Sonipat', 13, N'सोनीपत')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (194, N'Yamunanagar', 13, N'यमुनानगर')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (195, N'Bilaspur', 14, N'बिलासपुर')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (196, N'Chamba', 14, N'चंबा')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (197, N'Hamirpur', 14, N'हमीरपुर')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (198, N'Kangra', 14, N'कांगड़ा')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (199, N'Kinnaur', 14, N'किन्नौर')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (200, N'Kullu', 14, N'कुल्लू')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (201, N'Lahaul & Spiti', 14, N'लाहौल एवं स्पीति')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (202, N'Mandi', 14, N'मंडी')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (203, N'Shimla', 14, N'शिमला')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (204, N'Sirmaur (Sirmour)', 14, N'सिरमौर (सिरमौर)')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (205, N'Solan', 14, N'सोलन')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (206, N'Una', 14, N'उना')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (207, N'Anantnag', 15, N'अनंतनाग')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (208, N'Bandipora', 15, N'बांडीपूर')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (209, N'Baramulla', 15, N'बारामूला')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (210, N'Budgam', 15, N'बडगाम')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (211, N'Doda', 15, N'डोडा')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (212, N'Ganderbal', 15, N'गांदरबल')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (213, N'Jammu', 15, N'जम्मू')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (214, N'Kargil', 15, N'कारगिल')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (215, N'Kathua', 15, N'कठुआ')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (216, N'Kishtwar', 15, N'किश्तवाड़')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (217, N'Kulgam', 15, N'कुलगाम')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (218, N'Kupwara', 15, N'कुपवाड़ा')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (219, N'Leh', 15, N'लेह')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (220, N'Poonch', 15, N'पुंछ')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (221, N'Pulwama', 15, N'पुलवामा')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (222, N'Rajouri', 15, N'राजौरी')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (223, N'Ramban', 15, N'रामबन')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (224, N'Reasi', 15, N'रियासी')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (225, N'Samba', 15, N'सांबा')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (226, N'Shopian', 15, N'शोपियां')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (227, N'Srinagar', 15, N'श्रीनगर')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (228, N'Udhampur', 15, N'उधमपुर')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (229, N'Bokaro', 16, N'बोकारो')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (230, N'Chatra', 16, N'चतरा')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (231, N'Deoghar', 16, N'देवघर')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (232, N'Dhanbad', 16, N'धनबाद')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (233, N'Dumka', 16, N'दुमका')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (234, N'East Singhbhum', 16, N'पूर्वी सिंहभूम')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (235, N'Garhwa', 16, N'गढ़वा')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (236, N'Giridih', 16, N'गिरिडीह')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (237, N'Godda', 16, N'गोड्डा')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (238, N'Gumla', 16, N'गुमला')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (239, N'Hazaribag', 16, N'हजारीबाग')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (240, N'Jamtara', 16, N'जामताड़ा')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (241, N'Khunti', 16, N'खूंटी')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (242, N'Koderma', 16, N'कोडरमा')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (243, N'Latehar', 16, N'लातेहार')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (244, N'Lohardaga', 16, N'लोहरदगा')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (245, N'Pakur', 16, N'पाकुर')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (246, N'Palamu', 16, N'पलामू')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (247, N'Ramgarh', 16, N'रामगढ़')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (248, N'Ranchi', 16, N'रांची')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (249, N'Sahibganj', 16, N'साहिबगंज')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (250, N'Seraikela-Kharsawan', 16, N'सरायकेला-खरसावां')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (251, N'Simdega', 16, N'सिमडेगा')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (252, N'West Singhbhum', 16, N'पश्चिम सिंहभूम')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (253, N'Bagalkot', 17, N'बागलकोट')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (254, N'Bangalore Rural', 17, N'बैंगलोर ग्रामीण')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (255, N'Bangalore Urban', 17, N'बैंगलोर नगरीय')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (256, N'Belgaum', 17, N'बेलगाम')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (257, N'Bellary', 17, N'बेल्लारी')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (258, N'Bidar', 17, N'बीदर')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (259, N'Bijapur', 17, N'बीजापुर')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (260, N'Chamarajanagar', 17, N'चामराजनगर')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (261, N'Chickmagalur', 17, N'चिक्कमगलुर')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (262, N'Chikballapur', 17, N'चिकबल्लपुर')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (263, N'Chitradurga', 17, N'चित्रदुर्ग')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (264, N'Dakshina Kannada', 17, N'दक्षिण कन्नड़')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (265, N'Davangere', 17, N'दावणगेरे')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (266, N'Dharwad', 17, N'धारवाड़')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (267, N'Gadag', 17, N'गडग')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (268, N'Gulbarga', 17, N'गुलबर्गा')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (269, N'Hassan', 17, N'हसन')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (270, N'Haveri', 17, N'हावेरी')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (271, N'Kodagu', 17, N'कोडागू')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (272, N'Kolar', 17, N'कोलार')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (273, N'Koppal', 17, N'कोप्पल')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (274, N'Mandya', 17, N'मंड्या')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (275, N'Mysore', 17, N'मैसूर')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (276, N'Raichur', 17, N'रायचूर')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (277, N'Ramnagara', 17, N'रमनगरा')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (278, N'Shimoga', 17, N'शिमोगा')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (279, N'Tumkur', 17, N'तुमकुर')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (280, N'Udupi', 17, N'उडुपी')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (281, N'Uttara Kannada (Karwar)', 17, N'उत्तर कन्नड़ (कारवार)')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (282, N'Yadgir', 17, N'यादगीर')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (283, N'Alappuzha', 18, N'अलाप्पुझा')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (284, N'Ernakulam', 18, N'एर्नाकुलम')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (285, N'Idukki', 18, N'इडुक्की')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (286, N'Kannur', 18, N'कन्नूर')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (287, N'Kasaragod', 18, N'कासरगोड')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (288, N'Kollam', 18, N'कोल्लम')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (289, N'Kottayam', 18, N'कोट्टायम')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (290, N'Kozhikode', 18, N'कोझिकोड')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (291, N'Malappuram', 18, N'मलप्पुरम')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (292, N'Palakkad', 18, N'पलक्कड़')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (293, N'Pathanamthitta', 18, N'पथानामथिट्टा')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (294, N'Thiruvananthapuram', 18, N'तिरुवनंतपुरम')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (295, N'Thrissur', 18, N'त्रिशूर')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (296, N'Wayanad', 18, N'वायनाड')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (297, N'Lakshadweep', 19, N'लक्षद्वीप')
GO






INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (298, N'Alirajpur', 20, N'अलीराजपुर')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (299, N'Anuppur', 20, N'अनूपपुर')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (300, N'Ashoknagar', 20, N'अशोकनगर')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (301, N'Balaghat', 20, N'बालाघाट')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (302, N'Barwani', 20, N'बड़वानी')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (303, N'Betul', 20, N'बेतुल')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (304, N'Bhind', 20, N'भिंड')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (305, N'Bhopal', 20, N'भोपाल')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (306, N'Burhanpur', 20, N'बुरहानपुर')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (307, N'Chhatarpur', 20, N'छतरपुर')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (308, N'Chhindwara', 20, N'छिंदवाड़ा')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (309, N'Damoh', 20, N'दमोह')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (310, N'Datia', 20, N'दतिया')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (311, N'Dewas', 20, N'देवास')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (312, N'Dhar', 20, N'धार')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (313, N'Dindori', 20, N'डिंडोरी')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (314, N'Guna', 20, N'गुना')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (315, N'Gwalior', 20, N'ग्वालियर')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (316, N'Harda', 20, N'हरदा')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (317, N'Hoshangabad', 20, N'होशंगाबाद')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (318, N'Indore', 20, N'इंदौर')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (319, N'Jabalpur', 20, N'जबलपुर')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (320, N'Jhabua', 20, N'झाबुआ')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (321, N'Katni', 20, N'कटनी')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (322, N'Khandwa', 20, N'खंडवा')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (323, N'Khargone', 20, N'खरगोन')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (324, N'Mandla', 20, N'मंडला')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (325, N'Mandsaur', 20, N'मंदसौर')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (326, N'Morena', 20, N'मोरेना')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (327, N'Narsinghpur', 20, N'नरसिंहपुर')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (328, N'Neemuch', 20, N'नीमच')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (329, N'Panna', 20, N'पन्ना')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (330, N'Raisen', 20, N'रायसेन')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (331, N'Rajgarh', 20, N'राजगढ़')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (332, N'Ratlam', 20, N'रतलाम')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (333, N'Rewa', 20, N'रीवा')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (334, N'Sagar', 20, N'सागर')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (335, N'Satna', 20, N'सतना')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (336, N'Sehore', 20, N'सीहोर')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (337, N'Seoni', 20, N'सिवनी')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (338, N'Shahdol', 20, N'शाहडोल')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (339, N'Shajapur', 20, N'शाजापुर')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (340, N'Sheopur', 20, N'श्योपुर')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (341, N'Shivpuri', 20, N'शिवपुरी')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (342, N'Sidhi', 20, N'सीधी')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (343, N'Singrauli', 20, N'सिंगरौली')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (344, N'Tikamgarh', 20, N'टीकमगढ़')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (345, N'Ujjain', 20, N'उज्जैन')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (346, N'Umaria', 20, N'उमरिया')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (347, N'Vidisha', 20, N'विदिशा')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (348, N'Ahmednagar', 21, N'अहमदनगर')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (349, N'Akola', 21, N'अकोला')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (350, N'Amravati', 21, N'अमरावती')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (351, N'Aurangabad', 21, N'औरंगाबाद')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (352, N'Beed', 21, N'बीड')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (353, N'Bhandara', 21, N'भंडारा')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (354, N'Buldhana', 21, N'बुलढाना')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (355, N'Chandrapur', 21, N'चंद्रपुर')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (356, N'Dhule', 21, N'धुले')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (357, N'Gadchiroli', 21, N'गडचिरोली')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (358, N'Gondia', 21, N'गोंदिया')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (359, N'Hingoli', 21, N'हिंगोली')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (360, N'Jalgaon', 21, N'जलगांव')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (361, N'Jalna', 21, N'जलना')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (362, N'Kolhapur', 21, N'कोल्हापुर')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (363, N'Latur', 21, N'लातूर')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (364, N'Mumbai City', 21, N'मुंबई सिटी')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (365, N'Mumbai Suburban', 21, N'मुंबई उपनगर')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (366, N'Nagpur', 21, N'नागपुर')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (367, N'Nanded', 21, N'नांदेड़')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (368, N'Nandurbar', 21, N'नंदूरबार')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (369, N'Nashik', 21, N'नासिक')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (370, N'Osmanabad', 21, N'उस्मानाबाद')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (371, N'Parbhani', 21, N'परभनी')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (372, N'Pune', 21, N'पुणे')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (373, N'Raigad', 21, N'रायगढ़')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (374, N'Ratnagiri', 21, N'रत्नागिरी')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (375, N'Sangli', 21, N'सांगली')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (376, N'Satara', 21, N'सतारा')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (377, N'Sindhudurg', 21, N'सिंधुदुर्ग')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (378, N'Solapur', 21, N'सोलापुर')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (379, N'Thane', 21, N'ठाणे')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (380, N'Wardha', 21, N'वर्धा')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (381, N'Washim', 21, N'वाशिम')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (382, N'Yavatmal', 21, N'यवतमाल')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (383, N'Bishnupur', 22, N'बिश्नुपुर')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (384, N'Chandel', 22, N'चंदेल')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (385, N'Churachandpur', 22, N'चुराचांदपुर')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (386, N'Imphal East', 22, N'इंफाल ईस्ट')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (387, N'Imphal West', 22, N'इंफाल वेस्ट')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (388, N'Senapati', 22, N'सेनापति')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (389, N'Tamenglong', 22, N'तामेंगलांग')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (390, N'Thoubal', 22, N'थौबल')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (391, N'Ukhrul', 22, N'उखरूल')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (392, N'East Garo Hills', 23, N'पूर्वी गारो हिल्स')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (393, N'East Jaintia Hills', 23, N'पूर्वी जयंतिया हिल्स')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (394, N'East Khasi Hills', 23, N'पूर्वी खासी हिल्स')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (395, N'North Garo Hills', 23, N'उत्तरी गारो हिल्स')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (396, N'Ri Bhoi', 23, N'री भोई')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (397, N'South Garo Hills', 23, N'दक्षिण गारो हिल्स')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (398, N'South West Garo Hills', 23, N'दक्षिण पश्चिमी गारो हिल्स')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (399, N'South West Khasi Hills', 23, N'दक्षिण पश्चिमी खासी हिल्स')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (400, N'West Garo Hills', 23, N'वेस्ट गारो हिल्स')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (401, N'West Jaintia Hills', 23, N'पश्चिम जयन्तिया हिल्स')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (402, N'West Khasi Hills', 23, N'पश्चिम खासी हिल्स')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (403, N'Aizawl', 24, N'आइजोल')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (404, N'Champhai', 24, N'चम्फाई')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (405, N'Kolasib', 24, N'कोलासिब')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (406, N'Lawngtlai', 24, N'लवंगत्लई ')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (407, N'Lunglei', 24, N'लुंगलेई')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (408, N'Mamit', 24, N'मामित')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (409, N'Saiha', 24, N'सैहा')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (410, N'Serchhip', 24, N'सेरछिप')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (411, N'Dimapur', 25, N'दीमापुर')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (412, N'Kiphire', 25, N'कैफाइर ')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (413, N'Kohima', 25, N'कोहिमा')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (414, N'Longleng', 25, N'लोंगलेंग')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (415, N'Mokokchung', 25, N'मोकोकचुंग')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (416, N'Mon', 25, N'मोन')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (417, N'Peren', 25, N'पेरेन')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (418, N'Phek', 25, N'फेक')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (419, N'Tuensang', 25, N'ट्वेनसांग')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (420, N'Wokha', 25, N'वोखा')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (421, N'Zunheboto', 25, N'ज़ुन्हेबोटो')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (422, N'Angul', 26, N'अंगुल')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (423, N'Balangir', 26, N'बलांगीर')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (424, N'Balasore', 26, N'बालासोर')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (425, N'Bargarh', 26, N'बरगढ़')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (426, N'Bhadrak', 26, N'भद्रक')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (427, N'Boudh', 26, N'बौध')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (428, N'Cuttack', 26, N'कटक')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (429, N'Deogarh', 26, N'देवगढ़')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (430, N'Dhenkanal', 26, N'ढेन्कानाल')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (431, N'Gajapati', 26, N'गजपति')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (432, N'Ganjam', 26, N'गंजम')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (433, N'Jagatsinghapur', 26, N'जगतसिंहपुर')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (434, N'Jajpur', 26, N'जाजपुर')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (435, N'Jharsuguda', 26, N'झारसुगुडा')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (436, N'Kalahandi', 26, N'कलाहान्डी')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (437, N'Kandhamal', 26, N'कन्धमाल')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (438, N'Kendrapara', 26, N'केंद्रपाड़ा')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (439, N'Kendujhar (Keonjhar)', 26, N'केन्दुझर (केंझार)')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (440, N'Khordha', 26, N'खोर्धा')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (441, N'Koraput', 26, N'कोरापुट')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (442, N'Malkangiri', 26, N'मालकानगिरि')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (443, N'Mayurbhanj', 26, N'मयूरभंज')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (444, N'Nabarangpur', 26, N'नबरंगपुर')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (445, N'Nayagarh', 26, N'नयागढ़')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (446, N'Nuapada', 26, N'नुआपाड़ा')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (447, N'Puri', 26, N'पुरी')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (448, N'Rayagada', 26, N'रायगढ़')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (449, N'Sambalpur', 26, N'संबलपुर')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (450, N'Sonepur', 26, N'सोनेपुर')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (451, N'Sundargarh', 26, N'सुंदरगढ़')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (452, N'Karaikal', 27, N'कराईकल')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (453, N'Mahe', 27, N'माहे')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (454, N'Pondicherry', 27, N'पुदुच्चेरी')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (455, N'Yanam', 27, N'यानम')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (456, N'Amritsar', 28, N'अमृतसर')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (457, N'Barnala', 28, N'बरनाला')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (458, N'Bathinda', 28, N'भटिण्डा')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (459, N'Faridkot', 28, N'फरीदकोट')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (460, N'Fatehgarh Sahib', 28, N'फतेहगढ़ साहिब')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (461, N'Fazilka', 28, N'फाजिल्का')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (462, N'Ferozepur', 28, N'फिरोजपुर')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (463, N'Gurdaspur', 28, N'गुरदासपुर')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (464, N'Hoshiarpur', 28, N'होशियारपुर')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (465, N'Jalandhar', 28, N'जालंधर')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (466, N'Kapurthala', 28, N'कपूरथला')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (467, N'Ludhiana', 28, N'लुधियाना')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (468, N'Mansa', 28, N'मनसा')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (469, N'Moga', 28, N'मोगा')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (470, N'Muktsar', 28, N'मुक्तसर')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (471, N'Nawanshahr', 28, N'नवांशहर')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (472, N'Pathankot', 28, N'पठानकोट')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (473, N'Patiala', 28, N'पटियाला')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (474, N'Rupnagar', 28, N'रूपनगर')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (475, N'Sangrur', 28, N'संगरूर ')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (476, N'SAS Nagar (Mohali)', 28, N'एसएएस नगर (मोहाली)')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (477, N'Tarn Taran', 28, N'तरण तारण')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (478, N'Ajmer', 29, N'अजमेर')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (479, N'Alwar', 29, N'अलवर')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (480, N'Banswara', 29, N'बांसवाड़ा')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (481, N'Baran', 29, N'बरन')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (482, N'Barmer', 29, N'बाड़मेर')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (483, N'Bharatpur', 29, N'भरतपुर')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (484, N'Bhilwara', 29, N'भीलवाड़ा')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (485, N'Bikaner', 29, N'बीकानेर')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (486, N'Bundi', 29, N'बूंदी')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (487, N'Chittorgarh', 29, N'चित्तौड़गढ़')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (488, N'Churu', 29, N'चुरू')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (489, N'Dausa', 29, N'दौसा')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (490, N'Dholpur', 29, N'धौलपुर')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (491, N'Dungarpur', 29, N'डूँगरपुर')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (492, N'Hanumangarh', 29, N'हनुमानगढ़')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (493, N'Jaipur', 29, N'जयपुर')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (494, N'Jaisalmer', 29, N'जैसलमेर')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (495, N'Jalore', 29, N'जालोर')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (496, N'Jhalawar', 29, N'झालावाड़')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (497, N'Jhunjhunu', 29, N'झुंझुनू')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (498, N'Jodhpur', 29, N'जोधपुर')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (499, N'Karauli', 29, N'करौली')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (500, N'Kota', 29, N'कोटा')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (501, N'Nagaur', 29, N'नागौर')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (502, N'Pali', 29, N'पाली')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (503, N'Pratapgarh', 29, N'प्रतापगढ़')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (504, N'Rajsamand', 29, N'राजसमन्द')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (505, N'Sawai Madhopur', 29, N'सवाई माधोपुर')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (506, N'Sikar', 29, N'सीकर')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (507, N'Sirohi', 29, N'सिरोही')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (508, N'Sri Ganganagar', 29, N'श्री गंगानगर')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (509, N'Tonk', 29, N'टोंक')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (510, N'Udaipur', 29, N'उदयपुर')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (511, N'East Sikkim', 30, N'पूर्व सिक्किम')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (512, N'North Sikkim', 30, N'उत्तरी सिक्किम')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (513, N'South Sikkim', 30, N'दक्षिणी सिक्किम')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (514, N'West Sikkim', 30, N'पश्चिमी सिक्किम')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (515, N'Ariyalur', 31, N'अरियालुर')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (516, N'Chennai', 31, N'चेन्नई')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (517, N'Coimbatore', 31, N'कोयंबटूर')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (518, N'Cuddalore', 31, N'कुड्डालोर')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (519, N'Dharmapuri', 31, N'धर्मपुरी')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (520, N'Dindigul', 31, N'डिंडीगुल')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (521, N'Erode', 31, N'इरोड')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (522, N'Kanchipuram', 31, N'कांचीपुरम')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (523, N'Kanyakumari', 31, N'कन्याकूमारी')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (524, N'Karur', 31, N'करूर')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (525, N'Krishnagiri', 31, N'कृष्णागिरी')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (526, N'Madurai', 31, N'मदुरै')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (527, N'Nagapattinam', 31, N'नागपट्टिनम')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (528, N'Namakkal', 31, N'नामक्कल')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (529, N'Nilgiris', 31, N'नीलगिरी')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (530, N'Perambalur', 31, N'पेरम्बलुर')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (531, N'Pudukkottai', 31, N'पुदुक्कोट्टई')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (532, N'Ramanathapuram', 31, N'रामनाथपुरम')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (533, N'Salem', 31, N'सलेम')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (534, N'Sivaganga', 31, N'शिवगंगा')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (535, N'Thanjavur', 31, N'तंजावुर')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (536, N'Theni', 31, N'थेनी ')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (537, N'Thoothukudi (Tuticorin)', 31, N'तूतूकुड़ी (तूतीकोरिन)')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (538, N'Tiruchirappalli', 31, N'तिरुचिरापल्ली')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (539, N'Tirunelveli', 31, N'तिरूनेल्वेली')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (540, N'Tiruppur', 31, N'तिरुपूर')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (541, N'Tiruvallur', 31, N'तिरूवल्लुर')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (542, N'Tiruvannamalai', 31, N'तिरुवन्नमलई')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (543, N'Tiruvarur', 31, N'तिरूवारुर')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (544, N'Vellore', 31, N'वेल्लोर')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (545, N'Viluppuram', 31, N'विलुप्पुरम्')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (546, N'Virudhunagar', 31, N'विरुधुनगर')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (547, N'Adilabad', 32, N'अदिलाबाद')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (548, N'Hyderabad', 32, N'हैदराबाद')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (549, N'Karimnagar', 32, N'करीमनगर')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (550, N'Khammam', 32, N'खम्मम')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (551, N'Mahabubnagar', 32, N'महबूबनगर')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (552, N'Medak', 32, N'मेडक')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (553, N'Nalgonda', 32, N'नल्गोंडा')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (554, N'Nizamabad', 32, N'निजामाबाद')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (555, N'Rangareddy', 32, N'रंगारेड्डी')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (556, N'Warangal', 32, N'वारंगल')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (557, N'Dhalai', 33, N'धलाई')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (558, N'Gomati', 33, N'गोमती')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (559, N'Khowai', 33, N'खोवाई')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (560, N'North Tripura', 33, N'उत्तरी त्रिपुरा')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (561, N'Sepahijala', 33, N'सिपाहीजला')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (562, N'South Tripura', 33, N'दक्षिण त्रिपुरा')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (563, N'Unakoti', 33, N'उनाकोटी')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (564, N'West Tripura', 33, N'पश्चिमी त्रिपुरा')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (565, N'Agra', 34, N'आगरा')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (566, N'Aligarh', 34, N'अलीगढ़')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (567, N'Prayagraj', 34, N'प्रयागराज')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (568, N'Ambedkar Nagar', 34, N'अम्बेडकर नगर')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (569, N'Auraiya', 34, N'औरैया')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (570, N'Azamgarh', 34, N'आजमगढ़')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (571, N'Baghpat', 34, N'बागपत')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (572, N'Bahraich', 34, N'बहराइच')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (573, N'Ballia', 34, N'बलिया')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (574, N'Balrampur', 34, N'बलरामपुर')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (575, N'Banda', 34, N'बांदा')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (576, N'Barabanki', 34, N'बाराबंकी')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (577, N'Bareilly', 34, N'बरेली')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (578, N'Basti', 34, N'बस्ती')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (579, N'Sambhal', 34, N'संभल')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (580, N'Bijnor', 34, N'बिजनौर')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (581, N'Budaun', 34, N'बदायूँ')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (582, N'Bulandshahr', 34, N'बुलंदशहर')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (583, N'Chandauli', 34, N'चंदौली')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (584, N'Amethi', 34, N'अमेठी')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (585, N'Chitrakoot', 34, N'चित्रकूट')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (586, N'Deoria', 34, N'देवरिया')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (587, N'Etah', 34, N'एटा')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (588, N'Etawah', 34, N'इटावा')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (589, N'Ayodhya', 34, N'अयोध्या')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (590, N'Farrukhabad', 34, N'फर्रुखाबाद')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (591, N'Fatehpur', 34, N'फतेहपुर')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (592, N'Firozabad', 34, N'फिरोजाबाद')





GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (593, N'Gautam Buddha Nagar', 34, N'गौतम बुद्ध नगर')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (594, N'Ghaziabad', 34, N'गाज़ियाबाद')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (595, N'Ghazipur', 34, N'गाजीपुर')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (596, N'Gonda', 34, N'गोंडा')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (597, N'Gorakhpur', 34, N'गोरखपुर')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (598, N'Hamirpur', 34, N'हमीरपुर')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (599, N'Hardoi', 34, N'हरदोई')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (600, N'Hathras', 34, N'हाथरस')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (601, N'Jalaun', 34, N'जालौन')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (602, N'Jaunpur', 34, N'जौनपुर')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (603, N'Jhansi', 34, N'झांसी')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (604, N'J.P.Nagar (Amroha)', 34, N'अमरोहा')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (605, N'Kannauj', 34, N'कन्नौज')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (606, N'Kanpur Dehat', 34, N'कानपुर देहात')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (607, N'Kanpur Nagar', 34, N'कानपुर नगर')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (608, N'Kasganj', 34, N'कासगंज')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (609, N'Kaushambi', 34, N'कौशाम्बी')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (610, N'Kushi Nagar', 34, N'कुशीनगर')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (611, N'Lakhimpur - Kheri', 34, N'लखीमपुर-खीरी')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (612, N'Lalitpur', 34, N'ललितपुर')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (613, N'Lucknow', 34, N'लखनऊ')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (614, N'Maharajganj', 34, N'महाराजगंज')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (615, N'Mahoba', 34, N'महोबा')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (616, N'Mainpuri', 34, N'मैनपुरी')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (617, N'Mathura', 34, N'मथुरा')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (618, N'Mau', 34, N'मऊ')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (619, N'Meerut', 34, N'मेरठ')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (620, N'Mirzapur', 34, N'मिर्जापुर')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (621, N'Moradabad', 34, N'मुरादाबाद')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (622, N'Muzaffarnagar', 34, N'मुजफ्फरनगर')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (623, N'Hapur', 34, N'हापुड़')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (624, N'Pilibhit', 34, N'पीलीभीत')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (625, N'Shamli', 34, N'शामली')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (626, N'Pratapgarh', 34, N'प्रतापगढ़')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (627, N'RaeBareli', 34, N'रायबरेली')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (628, N'Rampur', 34, N'रामपुर')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (629, N'Saharanpur', 34, N'सहारनपुर')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (630, N'Sant Kabir Nagar', 34, N'संत कबीर नगर')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (631, N'Bhadohi', 34, N'भदोही')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (632, N'Shahjahanpur', 34, N'शाहजहांपुर')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (633, N'Shravasti', 34, N'श्रावस्ती')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (634, N'Siddharth Nagar', 34, N'सिद्धार्थ नगर')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (635, N'Sitapur', 34, N'सीतापुर')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (636, N'Sonbhadra', 34, N'सोनभद्र')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (637, N'Sultanpur', 34, N'सुल्तानपुर')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (638, N'Unnao', 34, N'उन्नाव')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (639, N'Varanasi', 34, N'वाराणसी')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (640, N'Almora', 35, N'अल्मोड़ा')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (641, N'Bageshwar', 35, N'बागेश्वर')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (642, N'Chamoli', 35, N'चमोली')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (643, N'Champawat', 35, N'चम्पावत')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (644, N'Dehradun', 35, N'देहरादून')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (645, N'Haridwar', 35, N'हरीद्वार')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (646, N'Nainital', 35, N'नैनीताल')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (647, N'Pauri Garhwal', 35, N'पौड़ी गढ़वाल')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (648, N'Pithoragarh', 35, N'पिथोरागढ़')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (649, N'Rudraprayag', 35, N'रुद्रप्रयाग')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (650, N'Tehri Garhwal', 35, N'टिहरी गढ़वाल')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (651, N'Udham Singh Nagar', 35, N'उधम सिंह नगर')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (652, N'Uttarkashi', 35, N'उत्तरकाशी')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (653, N'Bankura', 36, N'बांकुड़ा')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (654, N'Birbhum', 36, N'बीरभूम')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (655, N'Burdwan (Bardhaman)', 36, N'बर्दवान (बर्धमान)')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (656, N'Cooch Behar', 36, N'कूच बिहार')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (657, N'Dakshin Dinajpur (South Dinajpur)', 36, N'दक्षिण दिनाजपुर (दक्षिण दिनाजपुर)')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (658, N'Darjeeling', 36, N'दार्जिलिंग')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (659, N'Hooghly', 36, N'हुगली')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (660, N'Howrah', 36, N'हावड़ा')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (661, N'Jalpaiguri', 36, N'जलपाईगुड़ी')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (662, N'Kolkata', 36, N'कोलकाता')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (663, N'Malda', 36, N'मालदा')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (664, N'Murshidabad', 36, N'मुर्शिदाबाद')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (665, N'Nadia', 36, N'नादिया')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (666, N'North 24 Parganas', 36, N'उत्तर 24 परगना')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (667, N'Paschim Medinipur (West Medinipur)', 36, N'पश्चिम मेदिनीपुर (पश्चिम मेदिनीपुर)')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (668, N'Purba Medinipur (East Medinipur)', 36, N'पूर्व मेदिनीपुर (पूर्व मेदिनीपुर)')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (669, N'Purulia', 36, N'पुरुलिया')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (670, N'South 24 Parganas', 36, N'दक्षिण 24 परगना')
GO
INSERT [dbo].[DistrictMaster] ([DistrictId], [DistrictName], [StateId], [DistrictNameHI] ) VALUES (671, N'Uttar Dinajpur (North Dinajpur)', 36, N'उत्तर दिनाजपुर (उत्तर दिनाजपुर)')
GO
SET IDENTITY_INSERT [dbo].[DistrictMaster] OFF
GO


--------------------------------------------------------------------

TRUNCATE TABLE tbl_Wards;
INSERT INTO tbl_Wards (WardName, CreatedDate, IsActive)
VALUES
('General Ward', GETDATE(), 1),
('Surgical Ward', GETDATE(), 1),
('OT/Operation Theatre', GETDATE(), 1),
('ICU', GETDATE(), 1),
('Maternity/Gynae', GETDATE(), 1),
('Emergency', GETDATE(), 1),
('OPD', GETDATE(), 1),
('Labour Room', GETDATE(), 1),
('TB Ward', GETDATE(), 1),
('Other', GETDATE(), 1);