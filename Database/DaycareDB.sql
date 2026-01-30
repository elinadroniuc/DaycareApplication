-- Создание контейнера и доменов
CREATE DATABASE DAYCARE;
GO
USE DAYCARE;

CREATE TYPE fullName_type FROM VARCHAR(50) NOT NULL;
CREATE TYPE phoneNumber_type FROM CHAR(8) NOT NULL;
CREATE TYPE result_type FROM CHAR(3) NOT NULL;

-- Создание родительских таблиц
CREATE TABLE Parent(
    p_id INT PRIMARY KEY,
    p_fullName fullName_type,
    p_passportDetails VARCHAR(100),
    p_email VARCHAR(50),
    p_phoneNumber phoneNumber_type
);

CREATE TABLE Child(
    c_id INTEGER PRIMARY KEY,
    c_fullName fullName_type,
    c_birthdayDate DATE NOT NULL,
    c_birthCertificate VARCHAR(100)
);

-- Создание родительско-дочерних таблиц
CREATE TABLE Application(
    id_application INT PRIMARY KEY,
    c_id INTEGER,
    p_id INT,
    FOREIGN KEY (c_id) REFERENCES Child(c_id),
    FOREIGN KEY (p_id) REFERENCES Parent(p_id),
    requiredEnteringDate DATE NOT NULL
);

-- Создание дочерних таблиц
CREATE TABLE Appointment (
    id_application INT NOT NULL,
    FOREIGN KEY (id_application) REFERENCES Application(id_application),
    applicationResult result_type,
    paymentAmount MONEY,
    enteringDate DATE
);

-- Вставка данных в таблицу Parent
INSERT INTO Parent (p_id, p_fullName, p_passportDetails, p_email, p_phoneNumber)
VALUES
(1, 'John Smith', '1234567890', 'john@example.com', '12345678'),
(2, 'Alice Johnson', '9876543210', 'alice@example.com', '87654321'),
(3, 'Michael Brown', '5678912340', 'michael@example.com', '45678901'),
(4, 'Emily Davis', '4321098765', 'emily@example.com', '34567890'),
(5, 'Daniel Martinez', '9876543210', 'daniel@example.com', '23456789');

-- Вставка данных в таблицу Child
INSERT INTO Child (c_id, c_fullName, c_birthdayDate, c_birthCertificate)
VALUES
(10, 'Emma Smith', '2018-05-10', 'ABC123456'),
(20, 'Oliver Johnson', '2017-08-15', 'DEF654321'),
(30, 'Sophia Brown', '2019-02-03', 'GHI987654'),
(40, 'Noah Davis', '2018-11-25', 'JKL321098'),
(50, 'Ava Martinez', '2018-09-30', 'NMO456789');

-- Вставка данных в таблицу Application
INSERT INTO Application (id_application, c_id, p_id, requiredEnteringDate)
VALUES
(11, 10, 1, '2024-03-01'),
(22, 20, 2, '2024-03-05'),
(33, 30, 3, '2024-03-07'),
(44, 40, 4, '2024-03-15'),
(55, 50, 5, '2024-03-18');

-- Вставка данных в таблицу Appointment
INSERT INTO Appointment (id_application, applicationResult, paymentAmount, enteringDate)
VALUES
(11, 'YES', 100.00, '2024-03-01'),
(22, 'NO', 0.00, NULL),
(33, 'YES', 150.00, '2024-03-10'),
(44, 'NO', 0.00, NULL),
(55, 'YES', 120.00, '2024-03-20');

CREATE PROCEDURE AddParent
    @p_id INT, 
    @p_fullName fullName_type,
    @p_passportDetails VARCHAR(100),
    @p_email VARCHAR(50),
    @p_phoneNumber phoneNumber_type
AS
BEGIN
    BEGIN TRY
        IF EXISTS (SELECT 1 FROM Parent WHERE p_id = @p_id)
        BEGIN
            PRINT 'Parent with this ID already exists!';
            RETURN; 
        END

        IF EXISTS (SELECT 1 FROM Parent WHERE p_passportDetails = @p_passportDetails)
        BEGIN
            PRINT 'Parent with this passport already exists!';
            RETURN; 
        END

        IF EXISTS (SELECT 1 FROM Parent WHERE p_email = @p_email)
        BEGIN
            PRINT 'Parent with this email already exists!';
            RETURN; 
        END

        IF EXISTS (SELECT 1 FROM Parent WHERE p_phoneNumber = @p_phoneNumber)
        BEGIN
            PRINT 'Parent with this phone number already exists!';
            RETURN; 
        END

        INSERT INTO Parent (p_id, p_fullName, p_passportDetails, p_email, p_phoneNumber)
        VALUES (@p_id, @p_fullName, @p_passportDetails, @p_email, @p_phoneNumber);

        PRINT 'New parent added successfully.';
    END TRY
    BEGIN CATCH
        PRINT 'Error occurred while adding new parent: ' + ERROR_MESSAGE();
    END CATCH
END;

EXEC AddParent 
    @p_id = 12, 
    @p_fullName = 'Jane Doe', 
    @p_passportDetails = '9876541234', 
    @p_email = 'jane.doe@example.com', 
    @p_phoneNumber = '87654322';

	SELECT * FROM Parent

CREATE PROCEDURE DeleteParent
    @p_id INT
AS 
BEGIN
    BEGIN TRY
        IF NOT EXISTS (SELECT 1 FROM Parent WHERE p_id = @p_id)
        BEGIN
            PRINT 'Parent does not exist!';
            RETURN;
        END

        DELETE FROM Appointment 
        WHERE id_application IN (SELECT id_application FROM Application WHERE p_id = @p_id);

        DELETE FROM Application 
        WHERE p_id = @p_id;

        DELETE FROM Parent 
        WHERE p_id = @p_id;

        PRINT 'Parent and related applications have been deleted successfully.';
    END TRY
    BEGIN CATCH
        PRINT 'Error occurred while deleting parent: ' + ERROR_MESSAGE();
    END CATCH
END;

EXEC DeleteParent @p_id = 12;

SELECT * FROM Parent;

CREATE PROCEDURE UpdateParent
    @p_id INT,
    @p_fullName fullName_type,
    @p_passportDetails VARCHAR(100),
    @p_email VARCHAR(50),
    @p_phoneNumber phoneNumber_type
AS
BEGIN
    BEGIN TRY
        IF NOT EXISTS (SELECT 1 FROM Parent WHERE p_id = @p_id)
        BEGIN
            PRINT 'Parent does not exist!';
            RETURN;
        END

        IF EXISTS (SELECT 1 FROM Parent WHERE p_email = @p_email AND p_id <> @p_id)
        BEGIN
            PRINT 'This email is already in use by another parent!';
            RETURN; 
        END

        IF EXISTS (SELECT 1 FROM Parent WHERE p_phoneNumber = @p_phoneNumber AND p_id <> @p_id)
        BEGIN
            PRINT 'This phone number is already in use by another parent!';
            RETURN;
        END

        UPDATE Parent
        SET p_fullName = @p_fullName,
            p_passportDetails = @p_passportDetails,
            p_email = @p_email,
            p_phoneNumber = @p_phoneNumber
        WHERE p_id = @p_id;

        PRINT 'Parent information updated successfully.';
    END TRY
    BEGIN CATCH
        PRINT 'Error occurred while updating parent information: ' + ERROR_MESSAGE();
    END CATCH
END;

EXEC UpdateParent 
    @p_id = 1, 
    @p_fullName = 'Johanna Doe', 
    @p_passportDetails = '1234567890', 
    @p_email = 'johanna.doe@example.com', 
    @p_phoneNumber = '12345678';

	SELECT * FROM Parent;


CREATE PROCEDURE AddChild
    @c_id INT, 
    @c_fullName fullName_type,
    @c_birthdayDate DATE,
    @c_birthCertificate VARCHAR(100)
	
	AS
BEGIN
    BEGIN TRY
        IF EXISTS (SELECT 1 FROM Child WHERE c_id = @c_id)
        BEGIN
            PRINT 'Child with this ID already exists!';
            RETURN; 
        END

        IF EXISTS (SELECT 1 FROM Child WHERE c_birthCertificate = @c_birthCertificate)
        BEGIN
            PRINT 'Child with this certificate already exists!';
            RETURN; 
        END

        INSERT INTO Child (c_id, c_fullName, c_birthdayDate, c_birthCertificate)
        VALUES (@c_id, @c_fullName, @c_birthdayDate, @c_birthCertificate);

        PRINT 'New child added successfully.';
    END TRY
    BEGIN CATCH
        PRINT 'Error occurred while adding new child: ' + ERROR_MESSAGE();
    END CATCH
END;


EXEC AddChild 
    @c_id = 170, 
    @c_fullName = 'Joahn Richard', 
    @c_birthdayDate = '2016-02-14', 
    @c_birthCertificate = 'SMF154786';

	SELECT * FROM Child

CREATE PROCEDURE AddApplication
    @id_application INT,
    @c_id INT,
    @p_id INT,
    @requiredEnteringDate DATE
AS
BEGIN
    BEGIN TRY
        IF EXISTS (SELECT 1 FROM Application WHERE id_application = @id_application)
        BEGIN
            PRINT 'Application with this ID already exists!';
            RETURN;
        END
        
        IF NOT EXISTS (SELECT 1 FROM Child WHERE c_id = @c_id)
        BEGIN
            PRINT 'Child does not exist!';
            RETURN;
        END

        IF NOT EXISTS (SELECT 1 FROM Parent WHERE p_id = @p_id)
        BEGIN
            PRINT 'Parent does not exist!';
            RETURN;
        END

        INSERT INTO Application (id_application, c_id, p_id, requiredEnteringDate)
        VALUES (@id_application, @c_id, @p_id, @requiredEnteringDate);

        PRINT 'New application added successfully.';
    END TRY
    BEGIN CATCH
        PRINT 'Error occurred while adding new application: ' + ERROR_MESSAGE();
    END CATCH
END;

EXEC AddApplication 
    @id_application = 1613, 
    @c_id = 30, 
    @p_id = 3, 
    @requiredEnteringDate = '2024-03-01';


	SELECT * FROM Application

CREATE PROCEDURE AddAppointment
    @id_application INT,
    @applicationResult result_type,
    @paymentAmount MONEY,
    @enteringDate DATE
AS
BEGIN
    BEGIN TRY
        IF NOT EXISTS (SELECT 1 FROM Application WHERE id_application = @id_application)
        BEGIN
            PRINT 'Application does not exist!';
            RETURN;
        END

        IF EXISTS (SELECT 1 FROM Appointment WHERE id_application = @id_application)
        BEGIN
            PRINT 'Appointment for this application already exists!';
            RETURN;
        END

        INSERT INTO Appointment (id_application, applicationResult, paymentAmount, enteringDate)
        VALUES (@id_application, @applicationResult, @paymentAmount, @enteringDate);

        PRINT 'New appointment added successfully.';
    END TRY
    BEGIN CATCH
        PRINT 'Error occurred while adding new appointment: ' + ERROR_MESSAGE();
    END CATCH
END;

EXEC AddAppointment 
    @id_application = 1613, 
    @applicationResult = 'YES', 
    @paymentAmount = 200.00, 
    @enteringDate = '2024-04-01';

	SELECT * FROM Appointment
