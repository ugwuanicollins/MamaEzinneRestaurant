INSERT INTO Country VALUES ('Nigeria',1,0,GETDATE());


INSERT INTO State VALUES (1,'Abia',1,0);
INSERT INTO State VALUES (1,'Akwa-Ibom',1,0);
INSERT INTO State VALUES (1,'Anambra',1,0);
INSERT INTO State VALUES (1,'Bauchi',1,0);
INSERT INTO State VALUES (1,'Bayelsa',1,0);
INSERT INTO State VALUES (1,'Benue',1,0);
INSERT INTO State VALUES (1,'Borno',1,0);
INSERT INTO State VALUES (1,'Cross River',1,0);
INSERT INTO State VALUES (1,'Delta',1,0);
INSERT INTO State VALUES (1,'Ebonyi',1,0);
INSERT INTO State VALUES (1,'Edo',1,0);
INSERT INTO State VALUES (1,'Ekiti',1,0);
INSERT INTO State VALUES (1,'Enugu',1,0);
INSERT INTO State VALUES (1,'Gombe',1,0);
INSERT INTO State VALUES (1,'Imo',1,0);
INSERT INTO State VALUES (1,'Jigawa',1,0);
INSERT INTO State VALUES (1,'Kaduna',1,0);
INSERT INTO State VALUES (1,'Kano',1,0);
INSERT INTO State VALUES (1,'Katsina',1,0);
INSERT INTO State VALUES (1,'Kebbi',1,0);
INSERT INTO State VALUES (1,'Kogi',1,0);
INSERT INTO State VALUES (1,'Kwara',1,0);
INSERT INTO State VALUES (1,'Lagos',1,0);
INSERT INTO State VALUES (1,'Nasarawa',1,0);
INSERT INTO State VALUES (1,'Niger',1,0);
INSERT INTO State VALUES (1,'Ogun',1,0);
INSERT INTO State VALUES (1,'Ondo',1,0);
INSERT INTO State VALUES (1,'Osun',1,0);
INSERT INTO State VALUES (1,'Oyo',1,0);
INSERT INTO State VALUES (1,'Plateau',1,0);
INSERT INTO State VALUES (1,'Rivers',1,0);
INSERT INTO State VALUES (1,'Sokoto',1,0);
INSERT INTO State VALUES (1,'Taraba',1,0);
INSERT INTO State VALUES (1,'Yobe',1,0);
INSERT INTO State VALUES (1,'Zamfara',1,0);
INSERT INTO State VALUES (1,'Abuja',1,0);



INSERT INTO AspNetRoles VALUES ( NEWID(),'SuperAdmin','SUPERADMIN',NEWID() );
INSERT INTO AspNetRoles VALUES ( NEWID(),'User','USER',NEWID());  
INSERT INTO AspNetRoles VALUES ( NEWID(),'CompanyAdmin','COMPANYADMIN',NEWID() );


INSERT INTO CommonDropDowns VALUES (1,GETDATE(),'Female',1,0);
INSERT INTO CommonDropDowns VALUES (1,GETDATE(),'Male',1,0);

INSERT INTO CommonDropDowns VALUES (2,GETDATE(),'Christianity',1,0);
INSERT INTO CommonDropDowns VALUES (2,GETDATE(),'Islamic',1,0);
INSERT INTO CommonDropDowns VALUES (2,GETDATE(),'Judaism',1,0);
INSERT INTO CommonDropDowns VALUES (2,GETDATE(),'Paganism',1,0);


INSERT INTO CommonDropDowns VALUES (3,GETDATE(),'Agent',1,0);
INSERT INTO CommonDropDowns VALUES (3,GETDATE(),'Bank',1,0);
INSERT INTO CommonDropDowns VALUES (3,GETDATE(),'Bussiness',1,0);
INSERT INTO CommonDropDowns VALUES (3,GETDATE(),'Hospital',1,0);
INSERT INTO CommonDropDowns VALUES (3,GETDATE(),'School',1,0);
INSERT INTO CommonDropDowns VALUES (3,GETDATE(),'Security',1,0);
INSERT INTO CommonDropDowns VALUES (3,GETDATE(),'Transport',1,0);
INSERT INTO CommonDropDowns VALUES (3,GETDATE(),'Telecommunication',1,0);
INSERT INTO CommonDropDowns VALUES (3,GETDATE(),'software Technology',1,0);
