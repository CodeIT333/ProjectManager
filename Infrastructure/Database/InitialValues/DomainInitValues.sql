DELETE FROM Programmers
DELETE FROM Projects
DELETE FROM Customers
DELETE FROM ProjectManagers
DELETE FROM ProgrammerProjects

-- Insert Customers
INSERT INTO Customers (Id, Name, Email, Phone) VALUES
    ('dc13ed6b-844b-45a2-84ce-b012f99f640c', 'Tech Corp', 'contact@techcorp.com', '123-456-7890'),
    ('77159744-4cf8-4895-b1f4-cf621c0e4d40', 'Innovate Solutions', 'info@innovatesol.com', '234-567-8901');

-- Insert Project Managers
INSERT INTO ProjectManagers (Id, Name, DateOfBirth, Email, Phone, Address_Country, Address_ZipCode, Address_County, Address_Settlement, Address_Street, Address_HouseNumber, Address_Door)
VALUES 
    ('06f7be92-4fe0-47b1-ba2e-112e0dbc4422', 'Olivia Mitchell', '1983-08-25', 'olivia.mitchell@corp.com', '987-654-3210', 'USA', '20005', 'Washington DC', 'Washington', 'Pennsylvania Ave', '1600', NULL),
    ('4eb4e50c-4961-4a62-b44a-bba954c7607d', 'Liam Carter', '1985-11-03', 'liam.carter@solutions.com', '876-543-2109', 'USA', '10001', 'New York', 'New York City', '5th Avenue', '23A', '5');

-- Insert Projects
INSERT INTO Projects (Id, ProjectManagerId, CustomerId, StartDate, Description) VALUES
    ('61133527-5822-4391-b4d0-d3250a10153b', '06f7be92-4fe0-47b1-ba2e-112e0dbc4422', 'dc13ed6b-844b-45a2-84ce-b012f99f640c', '2024-01-01', 'AI Research Initiative'),
    ('22ba94c3-b569-440f-85b1-c5bbe2fdfcb2', '4eb4e50c-4961-4a62-b44a-bba954c7607d', '77159744-4cf8-4895-b1f4-cf621c0e4d40', '2024-02-15', 'Blockchain Security Project'),
    ('baf486f3-cc8f-4932-b9a9-2112279d852e', '06f7be92-4fe0-47b1-ba2e-112e0dbc4422', 'dc13ed6b-844b-45a2-84ce-b012f99f640c', '2024-03-10', 'Cloud Infrastructure Overhaul');

-- Insert Programmers
INSERT INTO Programmers (Id, Name, DateOfBirth, Phone, Email, Role, IsIntern, ProjectManagerId, Address_Country, Address_ZipCode, Address_County, Address_Settlement, Address_Street, Address_HouseNumber, Address_Door)
VALUES 
    ('4c6ecce3-a375-4521-8429-9e2d1deb1f01', 'Ethan Wright', '1990-05-15', '555-1234', 'john@example.com', 1, 0, '06f7be92-4fe0-47b1-ba2e-112e0dbc4422', 'USA', '10001', 'New York', 'New York City', '5th Avenue', '21A', '12'),
    ('d5f66844-4559-4466-af6f-dbd00c2e7409', 'Sophia Adams', '1995-08-20', '555-5678', 'jane@example.com', 2, 1, '4eb4e50c-4961-4a62-b44a-bba954c7607d', 'Canada', 'M5H 2N2', 'Ontario', 'Toronto', 'King Street', '99B', NULL),
    ('7cdb47de-6071-4b90-9b1f-5357772deba2', 'Daniel Rivera', '1993-09-12', '555-1111', 'alice@example.com', 1, 0, NULL, 'Germany', '10117', 'Berlin', 'Berlin', 'Friedrichstr.', '43', NULL),
    ('2fe13a21-feab-4278-bb08-a1978a492b29', 'Emma Collins', '1989-11-28', '555-2222', 'bob@example.com', 2, 0, NULL, 'France', '75001', 'Paris', 'Paris', 'Champs-Élysées', '8', NULL);
-- Insert Programmer-Project Relations
INSERT INTO ProgrammerProjects (ProgrammerId, ProjectId) VALUES
    ('4c6ecce3-a375-4521-8429-9e2d1deb1f01', '61133527-5822-4391-b4d0-d3250a10153b'),
    ('4c6ecce3-a375-4521-8429-9e2d1deb1f01', '22ba94c3-b569-440f-85b1-c5bbe2fdfcb2'),
    ('d5f66844-4559-4466-af6f-dbd00c2e7409', '22ba94c3-b569-440f-85b1-c5bbe2fdfcb2'),
    ('d5f66844-4559-4466-af6f-dbd00c2e7409', 'baf486f3-cc8f-4932-b9a9-2112279d852e');

