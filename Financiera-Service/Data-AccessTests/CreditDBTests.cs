using Data_Access;
using Data_Access.Entities;
using Data_AccessTests.Helpers;
using Microsoft.EntityFrameworkCore;

namespace Data_AccessTests;

[TestClass]
public class CreditDBTests
{
    private static CreditDB _creditDB;
    private readonly CreditCondition _creditCondition1 = new ()
    {
                id = 1,
                state = true,
                interestRate = 50,
                IVA = 16,
                paymentsPerMonth = 1,
                registrer = 1
    };
    private readonly Credit _credit = new()
    {
        id = 1,
        beneficiary = "1231234561237",
        capital = 50000,
        conditionId = 1,
        state = "requested",
        duration = 10,
        registryDate = DateTime.Now,
    };

    [ClassInitialize]
    public static void Initialize(TestContext context)
    {
        _creditDB = new CreditDB();

        using var db = new independent_financialContext(ConnectionStringGenerator.GetConnectionString());
        db.Database.ExecuteSqlRaw("DELETE FROM CreditPayments");
        db.Database.ExecuteSqlRaw("DELETE FROM Credits");
        db.Database.ExecuteSqlRaw("DELETE FROM CreditConditions");

        db.CreditConditions.Add(new CreditCondition
        {
            id = 1,
            state = true,
            interestRate = 50,
            IVA = 16,
            paymentsPerMonth = 1,
            registrer = 1
        });

        db.Credits.Add(new Credit
        {
            id = 1,
            beneficiary = "1231234561237",
            capital = 50000,
            conditionId = 1,
            state = "requested",
            duration = 10,
            registryDate = DateTime.Now
        });

        db.CreditPayments.Add(new CreditPayment
        {
            id = 1,
            conditionId = 1,
            capital = 100000,
            duration = 24,
            beneficiary = "1231234561237",
            IVA = 16,
            paymentsPerMonth = 4,
            state = "pending",
            interestRate = 15,
            registryDate = DateTime.Now
            
        });

        db.SaveChanges();
    }

    [TestMethod]
    public void UpdateStateSuccessfulTest()
    {
        int result = _creditDB.UpdateState(1, "approved");
        Assert.AreEqual(0, result, "Expected status code 0 for successful update.");
    }

    [TestMethod]
    public void UpdateStateFailedTest()
    {
        int result = _creditDB.UpdateState(9999, "denied");
        Assert.AreEqual(1, result, "Expected status code 1 for non-existent credit ID.");
    }

    [TestMethod]
    public void ExistsByIdSuccessfulTest()
    {
        Credit credit = _creditDB.ExistsById(1);
        Assert.IsNotNull(credit, "Expected to find a credit with ID 1.");
    }

    [TestMethod]
    public void ExistsByIdFailedTest()
    {
        Credit credit = _creditDB.ExistsById(9999);
        Assert.IsNull(credit, "Expected null for non-existent credit ID.");
    }

    [TestMethod]
    public void GetCreditPaymentSuccessfulTest()
    {
        CreditPayment payment = _creditDB.GetCreditPaymentInfo(1);
        Assert.IsNotNull(payment, "Expected to find a payment with ID 1.");
    }

    [TestMethod]
    public void GetCreditPaymentFailedTest()
    {
        CreditPayment payment = _creditDB.GetCreditPaymentInfo(9999);
        Assert.IsNull(payment, "Expected null for non-existent payment ID.");
    }
}
