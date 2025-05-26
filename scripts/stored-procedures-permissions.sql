USE independent_financial;

GRANT EXECUTE ON get_user_role TO financialReader;
GRANT EXECUTE ON GetCreditsByDateRange TO financialReader;

GRANT EXEC ON DeactivateClient TO financialLoanOfficer;

GRANT EXEC ON UpdatePaymentAsNotCollected TO financialCollectionsAgent;
GRANT EXEC ON UpdatePaymentAsCollected TO financialCollectionsAgent;
GRANT EXEC ON CreatePayment TO financialCollectionsAgent;

GO
