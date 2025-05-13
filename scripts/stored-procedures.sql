USE independent_financial;

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