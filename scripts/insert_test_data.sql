EXEC sys.sp_msforeachtable 'ALTER TABLE ? NOCHECK CONSTRAINT ALL'
EXEC sys.sp_msforeachtable 'DELETE FROM ?'
EXEC sys.sp_MSForEachTable 'ALTER TABLE ? CHECK CONSTRAINT ALL'

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

insert into Subsidiary (address, state) values ('hola mundo', 1);
insert into Employee ([user], password, name, mail, address, phoneNumber, birthday, role, sucursalId)
	values ('pale', '1234', 'pale', 'correo@ejemplo.com', 'fei', '1237897892', '2004/12/12', 'adviser', ident_current('Subsidiary')),
	('admin', '1234', 'david', 'correo@ejemplo.com', 'fei', '1122334455', '2000/11/21', 'admin', ident_current('Subsidiary')),
	('max', '1234', 'max', 'correo@ejemplo.com', 'fei', '7897897893', '2004/11/11', 'analist', ident_current('Subsidiary'));

insert into Client (rfc, name, birthday, houseAddress, workAddress, phoneNumber1, phoneNumber2, mail, state)
	values ('1231231231237', 'david carrion', '2000/11/21', 'dirección de mi casa', 'dirección de mi trabajo', '2288121212', '2281562389', 'correo@ejemplo.com', 1);
insert into BankAccount (clabe, purpose, clientId)
	values ('123456789123789456', 'receive', '1231231231237'), ('123789456123789456', 'collect', '1231231231237');
insert into PersonalReference (name, phoneNumber, relationship, clientRfc)
	values ('cuauhtemoc', '1231231238', '4564564568', '1231231231237'),
	('fernando', '1231231237', '7897897891', '1231231231237');

insert into CreditCondition (state, interestRate, IVA, paymentsPerMonth, registrer)
	values (1, 12, 16, 2, ident_current('Employee')), (1, 15, 16, 1, IDENT_CURRENT('Employee')), 
	(1, 10, 16, 4, IDENT_CURRENT('Employee')), (0, 10, 16, 4, IDENT_CURRENT('Employee'));

insert into RequiredDocumentation (name, state, fileType) values ('Identificación', 1, 'image'), ('Curp', 1, 'pdf'), ('pato', 0, 'pdf');
