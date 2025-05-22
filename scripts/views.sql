USE independent_financial;
GO

--Payment layout
CREATE VIEW PaymentLayout AS SELECT dbo.Payment.id, dbo.Client.name, dbo.Payment.collectionDate, dbo.Payment.amount, dbo.BankAccount.clabe
FROM dbo.Payment INNER JOIN dbo.Credit ON dbo.Payment.creditId = dbo.Credit.id 
	INNER JOIN dbo.Client ON dbo.Credit.beneficiary = dbo.Client.rfc 
	INNER JOIN dbo.BankAccount ON dbo.Client.rfc = dbo.BankAccount.clientId
WHERE (dbo.BankAccount.purpose = 'collect') AND (dbo.Payment.state = 'pending');
go

CREATE VIEW CreditRequest AS SELECT dbo.Credit.id, dbo.Client.name, dbo.Credit.capital, dbo.Credit.duration, dbo.CreditCondition.interestRate
FROM dbo.Credit INNER JOIN dbo.Client ON dbo.Client.rfc = dbo.Credit.beneficiary
INNER JOIN dbo.CreditCondition ON dbo.Credit.conditionId = dbo.CreditCondition.id
WHERE dbo.Credit.state = 'requested';

GO


--new
CREATE VIEW CreditPayment AS SELECT dbo.Credit.id, dbo.Credit.capital, dbo.Credit.duration, dbo.Credit.beneficiary,dbo.Credit.state,
dbo.Credit.registrer, dbo.Credit.registryDate,dbo.Credit.conditionId, 
dbo.CreditCondition.interestRate, dbo.CreditCondition.paymentsPerMonth, dbo.CreditCondition.IVA
FROM dbo.Credit 
INNER JOIN dbo.CreditCondition ON dbo.Credit.conditionId = dbo.CreditCondition.id
WHERE dbo.Credit.state = 'active';

GO
