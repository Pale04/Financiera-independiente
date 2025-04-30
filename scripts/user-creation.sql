--Necesitas registrar las 6 cuentas antes de ejecutar el script

USE independent_financial;

-- Se crea un ROL con solo permisos de selección en todas la tablas
IF NOT EXISTS (SELECT 1 FROM sys.database_principals WHERE name = 'financial_reader' AND type = 'R')
BEGIN
    CREATE ROLE financial_reader;
END

DECLARE @TableName SYSNAME;
DECLARE TableCursor CURSOR FOR
SELECT TABLE_NAME
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

-- Se crea un usuario para la cuenta que solo tendrá permisos de selección y hereda los permisos del rol que los tiene
CREATE USER financialReader FOR login financialReader;
ALTER ROLE financial_reader ADD MEMBER financialReader;
GRANT VIEW DEFINITION TO financialReader;

--Se crea un usuario para cada cuenta, añadiendo solo permisos necesarios
CREATE USER financialAdmin FOR login financialAdmin;
GRANT INSERT, SELECT, UPDATE ON OBJECT::CreditPolicy TO financialAdmin;
GRANT INSERT, SELECT, UPDATE ON OBJECT::CreditCondition TO financialAdmin;
GRANT INSERT, SELECT, UPDATE ON OBJECT::Subsidiary TO financialAdmin;
GRANT INSERT, SELECT, UPDATE ON OBJECT::RequiredDocumentation TO financialAdmin;
GRANT INSERT, SELECT ON OBJECT::Employee TO financialAdmin;

CREATE USER financialAnalist FOR login financialAnalist;
GRANT SELECT, UPDATE ON OBJECT::Credit TO financialAnalist;

CREATE USER financialLoanOfficer FOR login financialLoanOfficer;
GRANT INSERT, SELECT ON OBJECT::Credit TO financialLoanOfficer;
GRANT INSERT, SELECT, UPDATE ON OBJECT::Document TO financialLoanOfficer;
GRANT INSERT, SELECT, UPDATE ON OBJECT::BankAccount TO financialLoanOfficer;
GRANT INSERT, SELECT, UPDATE ON OBJECT::Client TO financialLoanOfficer;

CREATE USER financialCollectionsAgent FOR login financialCollectionsAgent;
GRANT INSERT, SELECT, UPDATE ON OBJECT::Payment TO financialCollectionsAgent;

CREATE USER financialAccountModificator FOR login financialAccountModificator;
GRANT SELECT, UPDATE ON OBJECT::Employee TO financialAccountModificator;
GO
