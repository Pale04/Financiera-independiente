--Payment layout
CREATE VIEW PaymentLayout AS SELECT dbo.Payment.id, dbo.Client.name, dbo.Payment.collectionDate, dbo.Payment.amount, dbo.BankAccount.clabe
FROM dbo.Payment INNER JOIN dbo.Credit ON dbo.Payment.creditId = dbo.Credit.id 
	INNER JOIN dbo.Client ON dbo.Credit.beneficiary = dbo.Client.rfc 
	INNER JOIN dbo.BankAccount ON dbo.Client.rfc = dbo.BankAccount.clientId
WHERE (dbo.BankAccount.purpose = 'collect') AND (dbo.Payment.state = 'pending');
GO