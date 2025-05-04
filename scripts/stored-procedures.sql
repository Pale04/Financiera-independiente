USE independent_financial;

CREATE PROCEDURE get_user_role
  @user_id int
AS
BEGIN
  SELECT role FROM Employee WHERE id = @user_id;
END
GO
