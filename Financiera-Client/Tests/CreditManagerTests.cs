using Business_logic;
using Business_logic.Catalogs;
using DomainClasses;

namespace Tests;

[TestClass]
public class CreditManagerTests
{
    private static CreditManager _creditManager;
    private static EmployeeClass _employee;
    private static AccountManager _accountManager;
    private static CreditConditionManager _creditConditionManager;
    private static Credit _credit;
    private static CreditCondition _creditCondition1;
    private static RequiredDocumentationManager _requiredDocumentationManager;
    private static Document _requiredDocument1;
    private static Document _requiredDocument2;
    private static List<Document> _documents;

    [TestInitialize]
    public void Initialize(TestContext context)
    {
        _accountManager = new();
        _creditConditionManager = new();
        _creditManager = new();
        _requiredDocumentationManager = new RequiredDocumentationManager();

        _requiredDocument1 = new()
        {
            Id = 1,
            Name = "CURP"
        };
        _requiredDocument2 = new()
        {
            Id = 2,
            Name = "Acta de nacimiento"
        };
        _employee = new()
        {
            Id = 1,
            User = "testUser",
            Password = "Abcd12345.",
            Name = "Test",
            Mail = "test@email.com",
            Address = "Test Address",
            PhoneNumber = "1234567890",
            Birthday = DateOnly.Parse("2004-06-02"),
            Role = "admin",
            SucursalId = 1
        };

        _credit = new()
        {
            Id = 1,
            Beneficiary = "1231234561237",
            Capital = 50000,
            ConditionId = 1,
            State = "requested",
            Duration = 10
        };

        _creditCondition1 = new()
        {
            Id = 1,
            State = true,
            InterestRate = 50,
            IVA = 16,
            PaymentsPerMonth = 1,
            RegistrerId = 1
        };

        _documents.Add(_requiredDocument1);
        _documents.Add(_requiredDocument2);
        _accountManager.CreateAccount(_employee);
        _creditConditionManager.AddCreditCondition(_creditCondition1);
        _creditManager.Add(_credit, _documents);

    }

    [TestMethod]
    public void DeterminateRequestSuccessTest()
    {
        int result = _creditManager.DeterminateResquest(_credit, true);
        Assert.AreEqual(0, result, "Expected status code 0 for successful credit determination.");
    }

    [TestMethod]
    public void DeterminateRequestFailedTest()
    {
        int result = _creditManager.DeterminateResquest(null, false);
        Assert.AreEqual(2, result, "Expected status code 2 when the credit object is null.");
    }

    [TestMethod]
    public void GetCreditConditionSuccessTest()
    {
        CreditCondition condition = _creditManager.GetCreditCondition(_credit.Id);
        Assert.IsNotNull(condition, "Expected non-null credit condition for valid credit ID.");
        Assert.AreEqual(_creditCondition1.InterestRate, condition.InterestRate, "Interest rate mismatch.");
        Assert.AreEqual(_creditCondition1.IVA, condition.IVA, "IVA mismatch.");
        Assert.AreEqual(_creditCondition1.PaymentsPerMonth, condition.PaymentsPerMonth, "Payments per month mismatch.");
    }

    [TestMethod]
    public void GetCreditConditionFailedTest()
    {
        Assert.ThrowsException<Exception>(() => _creditManager.GetCreditCondition(9999), "Expected exception for non-existent credit ID.");
    }
}
