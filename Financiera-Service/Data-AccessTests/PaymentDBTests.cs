using Data_Access;
using Data_Access.Entities;
using Data_AccessTests.Helpers;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Data_AccessTests;

[TestClass]
public class PaymentDBTests
{
    private readonly PaymentDB _paymentDB;
    private static Data_Access.Entities.Payment _payment1 = new()
    {
        id = 1,
        amount = 100000,
        creditId = 1,
        state = "collected",
        collectionDate = DateOnly.FromDateTime(DateTime.Today),
        registrer = 1

    };

    private Data_Access.Entities.Credit _credit1 = new()
    {
        id = 1,
        beneficiary = "1231234561237",
        capital = 50000,
        conditionId = 1,
        state = "requested",
        duration = 10,
        registryDate = DateTime.Now
    };

    private Data_Access.Entities.Employee _employee1 = new()
    {
        id = 100,
        user = "david",
        password = "1234",
        name = "david carrion",
        mail = "correo2@ejemplo.com",
        address = "fei",
        phoneNumber = "3219879872",
        birthday = DateOnly.Parse("2000/11/11"),
        role = "analist",
        sucursalId = 1
    };

    public PaymentDBTests()
    {
        _paymentDB = new PaymentDB();
    }

    [ClassInitialize]
    public void SetDatabase(TestContext testContext)
    {
        using (var context = new Data_Access.Entities.independent_financialContext(ConnectionStringGenerator.GetConnectionString()))
        {
            context.Database.ExecuteSqlRaw("DELETE FROM Employee");
            context.Database.ExecuteSqlRaw("DELETE FROM Payment");
            context.Database.ExecuteSqlRaw("DELETE FROM Credit");
            context.Employees.Add(_employee1);
            context.Credits.Add(_credit1);
            context.Payments.Add(_payment1);
            context.SaveChanges();
        }
    }

    [ClassCleanup]
    public void CleanDatabase(TestContext testContext)
    {
        using (var context = new Data_Access.Entities.independent_financialContext(ConnectionStringGenerator.GetConnectionString()))
        {
            context.Database.ExecuteSqlRaw("DELETE FROM Employee");
            context.Database.ExecuteSqlRaw("DELETE FROM Payment");
            context.Database.ExecuteSqlRaw("DELETE FROM Credit");
        }
    }

    [TestMethod]
    public void AddPaymentSuccessfulTest()
    {
        var newPayment = new Payment
        {
            amount = 120000,
            creditId = 1,
            registrer = 1,
            collectionDate = DateOnly.FromDateTime(DateTime.Today.AddDays(1))
        };

        int result = _paymentDB.AddPayment(newPayment);

        Assert.AreEqual(0, result, "The payment should be added successfully.");
    }

    [TestMethod]
    public void AddPaymentInvalidFieldsTest()
    {
        var invalidPayment = new Payment
        {
            creditId = 1
        };

        Assert.ThrowsException<DbUpdateException>(() =>
        {
            _paymentDB.AddPayment(invalidPayment);
        }, "The payment should not be added due to missing required fields.");
    }

    [TestMethod]
    public void AddPaymentEmptyTest()
    {
        Payment? nullPayment = null;

        Assert.ThrowsException<ArgumentNullException>(() =>
        {
            _paymentDB.AddPayment(nullPayment!);
        }, "The payment parameter is null and should throw an exception.");
    }

    [TestMethod]
    public void GetFromDateRangeSuccessfulRange()
    {
        var startDate = DateOnly.FromDateTime(DateTime.Today.AddDays(-1));
        var endDate = DateOnly.FromDateTime(DateTime.Today.AddDays(1));

        var payments = _paymentDB.GetFromDateRange(startDate, endDate);

        Assert.IsTrue(payments.Count > 0, "Payments should be returned within the valid date range.");
    }

    [TestMethod]
    public void GetFromDateRangeInvalidDateTest()
    {
        var startDate = DateOnly.FromDateTime(DateTime.Today.AddDays(10));
        var endDate = DateOnly.FromDateTime(DateTime.Today.AddDays(20));

        var payments = _paymentDB.GetFromDateRange(startDate, endDate);

        Assert.IsTrue(payments.Count == 0, "No payments should be returned for a future date range.");
    }

    [TestMethod]
    public void GetFromDateRangeFailedTest()
    {
        var startDate = DateOnly.MinValue;
        var endDate = DateOnly.MinValue;

        Assert.ThrowsException<SqlException>(() =>
        {
            _paymentDB.GetFromDateRange(startDate, endDate);
        }, "Invalid input or DB error should throw a SqlException.");
    }
}
