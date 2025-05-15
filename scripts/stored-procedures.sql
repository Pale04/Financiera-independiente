USE independent_financial;
GO

CREATE PROCEDURE get_user_role
  @user_id int
AS
BEGIN
  SELECT role FROM Employee WHERE id = @user_id;
END
GO

--Validate any credit condition when the client state is updated to deactivate
CREATE PROCEDURE DeactivateClient
	@rfc CHAR(13)
AS
BEGIN
	IF EXISTS (SELECT * FROM Credit WHERE beneficiary = @rfc AND state = 'active')
	BEGIN
		SELECT 'Result' = -1
	END
	ELSE IF EXISTS (SELECT * FROM Credit WHERE beneficiary = @rfc AND state = 'requested')
	BEGIN
		UPDATE Credit SET state = 'rejected' WHERE beneficiary = @rfc AND state = 'requested'
		SELECT 'Result' = 1
	END;
	UPDATE Client SET state = 0 WHERE rfc = @rfc
	SELECT 'Result' = 1
END;
GO

--Mark a payment with 'not_collected' state
CREATE PROCEDURE UpdatePaymentAsNotCollected
	@id int,
	@collectionDate	Date,
	@amount DECIMAL(7,2),
	@registrer int,
	@creditId int
AS
BEGIN
	UPDATE Payment SET state = 'not_collected', registrer = @registrer WHERE id = @id

	IF EXISTS(SELECT 1 FROM Payment WHERE creditId = @creditId AND state = 'pending') 
	BEGIN
		--Add charge to next payment
		DECLARE @nextPaymentId int
		SELECT TOP 1 @nextPaymentId = id FROM Payment WHERE creditId = @creditId AND state = 'pending' ORDER BY collectionDate ASC
		UPDATE Payment SET amount =	amount + @amount + 200 WHERE id = @nextPaymentId
	END
	ELSE
	BEGIN
		--Create another payment because the last was not collected
		DECLARE @newAmount DECIMAL(7,2)
		SET @newAmount = @amount + 200
		DECLARE @paymentsPerMonth int
		SELECT TOP 1 @paymentsPerMonth = paymentsPerMonth FROM CreditCondition WHERE id = (SELECT TOP 1 conditionId FROM Credit WHERE id = @creditId)
		EXEC CreatePayment
			@previousPaymentCollectionDate = @collectionDate, 
			@paymentsPerMonth = @paymentsPerMonth, 
			@amount = @newAmount, 
			@registrer = registrer, 
			@creditId = @creditId
	END;

	--Verify if the credit can be 'not_chargeable'
	--Falta una columna en la tabla de credit donde se lleve una suma cuando un pago no se cobre
	--para validar si hay 3 consecutivos.
END;
GO

CREATE PROCEDURE UpdatePaymentAsCollected
	@id int,
	@registrer int,
	@creditId int
AS
BEGIN
	UPDATE Payment SET state = 'collected', registrer = @registrer WHERE id = @id

	--Verify if the credit has ended
	IF NOT EXISTS (SELECT * FROM Payment WHERE creditId = @creditId AND state = 'pending')
	BEGIN
		UPDATE Credit SET state = 'ended' WHERE id = @creditId
	END;
END;
GO

--Create a new payment
CREATE PROCEDURE CreatePayment
	@previousPaymentCollectionDate Date,
	@paymentsPerMonth int, 
	@amount DECIMAL(7,2), 
	@registrer int, 
	@creditId int
AS
BEGIN
	DECLARE @collectionDate Date
	IF (@paymentsPerMonth = 1)
	BEGIN
		SET @collectionDate = DATEADD(MONTH,1,@previousPaymentCollectionDate)
	END
	ELSE IF (@paymentsPerMonth = 2)
	BEGIN
		SET @collectionDate = DATEADD(DAY,14,@previousPaymentCollectionDate)
	END
	ELSE
	BEGIN
		SET @collectionDate = DATEADD(DAY,7,@previousPaymentCollectionDate)
	END
	INSERT INTO Payment(collectionDate,amount,state,registrer,creditId) 
	VALUES (@collectionDate,@amount,'pending',@registrer,@creditId);
END;