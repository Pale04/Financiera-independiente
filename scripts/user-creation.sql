USE DATABASE independent-financial;

CREATE USER financialReader FOR login financialReader;
GO

-- Create role able to select from all tables
IF NOT EXISTS (SELECT 1 FROM sys.database_principals WHERE name = 'financial_reader' AND type = 'R')
BEGIN
    CREATE ROLE financial_reader;
END
GO

DECLARE @TableName SYSNAME;
DECLARE TableCursor CURSOR FOR
SELECT TABLE_SCHEMA + '.' + TABLE_NAME
FROM information_schema.TABLES
WHERE TABLE_TYPE = 'BASE TABLE';

OPEN TableCursor;

FETCH NEXT FROM TableCursor INTO @TableName;

WHILE @@FETCH_STATUS = 0
BEGIN
    DECLARE @GrantStatement NVARCHAR(MAX);
    SET @GrantStatement = 'GRANT SELECT ON OBJECT::' + QUOTENAME(@TableName) + ' TO financial_reader;';
    EXEC (@GrantStatement);
    FETCH NEXT FROM TableCursor INTO @TableName;
END;

CLOSE TableCursor;
DEALLOCATE TableCursor;
GO

-- Asign role to reader user
ALTER ROLE financial_reader ADD MEMBER financialReader;
GO

-- Create user for each company role
CREATE USER financialAdmin FOR login financialAdmin;
GRANT CREATE, UPDATE ON OBJECT::CreditPolicy TO financialAdmin;
GRANT CREATE, UPDATE ON OBJECT::CreditCondition TO financialAdmin;
GRANT CREATE, UPDATE ON OBJECT::Subsidiary TO financialAdmin;
GRANT CREATE, UPDATE ON OBJECT::RequiredDocument TO financialAdmin;
GRANT CREATE ON OBJECT::Employee TO financialAdmin;

CREATE USER financialAnalist FOR login financialAnalist;
GRANT UPDATE ON OBJECT::Credit TO financialAnalist;

CREATE USER financialLoanOfficer FOR login financialLoanOfficer;
GRANT CREATE ON OBJECT::Credit TO financialLoanOfficer;
GRANT CREATE, UPDATE ON OBJECT::Document TO financialLoanOfficer;
GRANT CREATE, UPDATE ON OBJECT::BankAccount TO financialLoanOfficer;
GRANT CREATE, UPDATE ON OBJECT::Client TO financialLoanOfficer;

CREATE USER financialCollectionsAgent FOR login financialCollectionsAgent;
GRANT CREATE, UPDATE ON OBJECT::Payment TO financialCollectionsAgent;

CREATE USER financialAccountModificator FOR login financialAccountModificator;
GRANT UPDATE ON OBJECT::Employee TO financialAccountModificator;
