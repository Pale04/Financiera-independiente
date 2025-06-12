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
    private static CustomerManager _customerManager;

    private static Credit _credit2 = new Credit()
    {
        Beneficiary = "4564564564561",
        Capital = 50000,
        ConditionId = 1,
        State = "requested",
        Duration = 12
    };

    private static Customer _customer = new()
    {
        Rfc = "1231234561237",
        Name = "Jese Carrion",
        BirthDate = DateOnly.Parse("2004-06-02"),
        HouseAddress = "casa de jese",
        WorkAddress = "trabajo de jese",
        PhoneNumber1 = "7897897893",
        PhoneNumber2 = "3213211231",
        Email = "ejemlpo@ejemplo.com",
        State = true
    };

    [TestInitialize]
    public void Initialize(TestContext context)
    {
        _accountManager = new();
        _creditConditionManager = new();
        _creditManager = new();
        _requiredDocumentationManager = new RequiredDocumentationManager();
        _customerManager = new();

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
            SucursalId = 1,
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

        List<PersonalReference> references = new();
        references.Add(new()
        {
            Name = "David carrion",
            PhoneNumber = "9879877771",
            Relationship = "Hermano",
            CustomerRfc = "1231234561237"
        });
        references.Add(new()
        {
            Name = "Fulano carrion",
            PhoneNumber = "9579877771",
            Relationship = "Hermano",
            CustomerRfc = "1231234561237"
        });

        List<BankAccount> bankAccounts = new();
        bankAccounts.Add(new()
        {
            Clabe = "777788889999444411",
            Purpose = "collect",
            CustomerRfc = "1231234561237"
        });
        bankAccounts.Add(new()
        {
            Clabe = "777788889999544411",
            Purpose = "receive",
            CustomerRfc = "1231234561237"
        });

        _customer.PersonalReferences = references.ToArray();
        _customer.BankAccounts = bankAccounts.ToArray();

        _documents.Add(_requiredDocument1);
        _documents.Add(_requiredDocument2);
        _accountManager.CreateAccount(_employee);
        _creditConditionManager.AddCreditCondition(_creditCondition1);
        _creditManager.Add(_credit, _documents);
        _customerManager.AddCustomer(_customer);
    }

    [TestMethod]
    public void GetCreditRequestsTest()
    {
        List<CreditRequestSummary> credits = _creditManager.GetCreditRequests();

        Assert.Equals(1, credits.Count);
        Assert.Equals(_customer.Name, credits.First().ClientName);
    }

    [TestMethod]
    public void CreateCreditRequestTest()
    {
        int result = _creditManager.Add(_credit2, _documents).Item2;
        Assert.AreEqual(0, result);
    }

    [TestMethod]
    public void CreateEmptyCreditRequestTest()
    {
        Credit credit = new();

        int result = _creditManager.Add(credit, _documents).Item2;
        Assert.AreEqual(1, result);
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
