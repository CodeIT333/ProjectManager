DELETE FROM ProgrammerProjects
DELETE FROM Customers
DELETE FROM ProjectManagers
DELETE FROM Projects
DELETE FROM Programmers

-- Insert Customers
INSERT INTO Customers (Id, Name, Email, Phone) VALUES
    ('8cb26438-0907-4d52-8df6-0506a7cbc9aa', 'Customer A', 'customerA@example.com', '123-456-7890'),
    ('a39906e9-967f-4f2e-88f0-3795603f804a', 'Customer B', 'customerB@example.com', '234-567-8901');

-- Insert Project Managers
INSERT INTO ProjectManagers (Id, Name, DateOfBirth, Email, Phone, Address_Country, Address_ZipCode, Address_County, Address_Settlement, Address_Street, Address_HouseNumber, Address_Door)
VALUES 
    ('21dd2bdc-42b7-4c9b-965a-f7e1a6255fc6', 'Manager X', CAST('1983-08-25' AS DATE), 'managerX@example.com', '987-654-3210', 'USA', '20005', 'Washington DC', 'Washington', 'Pennsylvania Ave', '1600', NULL),
    ('b74a9386-6ee5-4c42-9b95-99e344191008', 'Manager Y', CAST('1985-11-03' AS DATE), 'managerY@example.com', '876-543-2109', 'USA', '10001', 'New York', 'New York City', '5th Avenue', '23A', '5');

-- Insert Projects
INSERT INTO Projects (Id, ProjectManagerId, CustomerId, StartDate, Description) VALUES
    ('bbea3bd5-521c-46e3-8461-34e12df40622', '21dd2bdc-42b7-4c9b-965a-f7e1a6255fc6', '8cb26438-0907-4d52-8df6-0506a7cbc9aa', '2024-01-01', 'Project Alpha'),
    ('bc1efc1e-94ff-4bdf-8f37-c7c62b8f2809', 'b74a9386-6ee5-4c42-9b95-99e344191008', 'a39906e9-967f-4f2e-88f0-3795603f804a', '2024-02-15', 'Project Beta'),
    ('f21685d0-ba2e-40ae-a0fb-4c6874a410cd', '21dd2bdc-42b7-4c9b-965a-f7e1a6255fc6', '8cb26438-0907-4d52-8df6-0506a7cbc9aa', '2024-03-10', 'Project Gamma');

-- Insert Programmers
INSERT INTO Programmers (Id, Name, DateOfBirth, Phone, Email, Role, IsIntern, Address_Country, Address_ZipCode, Address_County, Address_Settlement, Address_Street, Address_HouseNumber, Address_Door)
VALUES 
    ('37a5af97-f1ef-4cb9-a9e0-93821d430a5d', 'John Doe', CAST('1990-05-15' AS DATE), '555-1234', 'john@example.com', 1, 0, 'USA', '10001', 'New York', 'New York City', '5th Avenue', '21A', '12'),
    ('49da933b-18bc-4bdf-99e9-2cdb5eae1c10', 'Jane Smith', CAST('1995-08-20' AS DATE), '555-5678', 'jane@example.com', 2, 1, 'Canada', 'M5H 2N2', 'Ontario', 'Toronto', 'King Street', '99B', NULL );

-- Insert Programmer-Project Relations
INSERT INTO ProgrammerProjects (ProgrammerId, ProjectId) VALUES
    ('37a5af97-f1ef-4cb9-a9e0-93821d430a5d', 'bbea3bd5-521c-46e3-8461-34e12df40622'),
    ('37a5af97-f1ef-4cb9-a9e0-93821d430a5d', 'bc1efc1e-94ff-4bdf-8f37-c7c62b8f2809'),
    ('49da933b-18bc-4bdf-99e9-2cdb5eae1c10', 'bc1efc1e-94ff-4bdf-8f37-c7c62b8f2809'),
    ('49da933b-18bc-4bdf-99e9-2cdb5eae1c10', 'f21685d0-ba2e-40ae-a0fb-4c6874a410cd');


