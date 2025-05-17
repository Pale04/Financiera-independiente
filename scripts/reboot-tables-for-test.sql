Use independent_financial;

--Delete all data from all tables
delete from [Payment];
delete from [Document];
delete from [Credit];
delete from [RequiredDocumentation];
delete from [CreditPolicy];
delete from [Creditcondition];
delete from [BankAccount];
delete from [PersonalReference];
delete from [Client];
delete from [Employee];
delete from [Subsidiary];

--Reboot indexes to 0
DBCC CHECKIDENT ('Payment', RESEED, 0);
DBCC CHECKIDENT ('Document', RESEED, 0);
DBCC CHECKIDENT ('Credit', RESEED, 0);
DBCC CHECKIDENT ('RequiredDocumentation', RESEED, 0);
DBCC CHECKIDENT ('CreditPolicy', RESEED, 0);
DBCC CHECKIDENT ('CreditCondition', RESEED, 0);
DBCC CHECKIDENT ('BankAccount', RESEED, 0);
DBCC CHECKIDENT ('PersonalReference', RESEED, 0);
DBCC CHECKIDENT ('Employee', RESEED, 0);
DBCC CHECKIDENT ('Subsidiary', RESEED, 0);

--Create user accounts for test purposes
insert into Subsidiary(Address,state) values('Principal',1);
