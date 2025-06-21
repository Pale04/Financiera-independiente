using Business_logic;
using DomainClasses;

namespace Tests;

[TestClass]
public class AccountManagerTests
{
    private static EmployeeClass _employee;
    private static EmployeeClass _employee2 = new()
    {
        User = "testUser2",
        Password = "Abcd12345.",
        Name = "Test2",
        Mail = "test2@email.com",
        Address = "Test Address 2",
        PhoneNumber = "98732165",
        Birthday = DateOnly.Parse("2000-06-02"),
        Role = "admin",
        SucursalId = 1
    };
    private static AccountManager _accountManager;

    [TestInitialize]
    public void Initialize(TestContext context)
    {
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

        _accountManager.CreateAccount(_employee);
    }

    [TestMethod]
    public void CreateAccountSuccessfulTest()
    {
        Assert.AreEqual(0, _accountManager.CreateAccount(_employee2));
    }

    [TestMethod]
    public void CreateAccountDuplicateTest()
    {
        Assert.AreEqual(3, _accountManager.CreateAccount(_employee));
    }

    [TestMethod]
    public void CreateAccountInvalidDataTest()
    {
        EmployeeClass employee = new()
        {
            User = "testUser",
            Password = "Abcd12345.",
            Name = "Test",
            Mail = "nocorreo",
            Address = "Test Address",
            PhoneNumber = "telefonoraro",
            Birthday = DateOnly.Parse("2004-06-02"),
            Role = "admin",
            SucursalId = 1
        };

        Assert.AreEqual(2, _accountManager.CreateAccount(employee));
    }

    [TestMethod]
    public void SendEmailSuccessfulTest()
    {
        _employee.Password = null;
        int result = _accountManager.SendEmail(_employee);
        Assert.AreEqual(0, result, "SendEmailSuccessfulTest: Expected 0 when email is sent successfully.");
    }

    [TestMethod]
    public void SendEmailInvalidFieldsTest()
    {
        EmployeeClass invalidEmployee = new();
        int result = _accountManager.SendEmail(invalidEmployee);
        Assert.AreEqual(2, result, "SendEmailInvalidFieldsTest: Expected 2 when employee fields are invalid.");
    }

    [TestMethod]
    public void SendEmailFailedTest()
    {
        EmployeeClass faultyEmployee = new() { User = "nonexistent" };
        int result = _accountManager.SendEmail(faultyEmployee);
        Assert.AreEqual(1, result, "SendEmailFailedTest: Expected 1 when server communication fails.");
    }

    [TestMethod]
    public void ChangePasswordSuccessfulTest()
    {
        int result = _accountManager.ChangePassword(_employee, "NewPassword123!", "NewPassword123!");
        Assert.AreEqual(0, result, "Expected 0 when password changes successfully.");
    }

    [TestMethod]
    public void ChangePasswordInvalidFieldsTest()
    {
        int result = _accountManager.ChangePassword(_employee, "abc", "xyz");
        Assert.AreEqual(2, result, "Expected 2 when passwords don't match or are invalid.");
    }

    [TestMethod]
    public void ChangePasswordFailedTest()
    {
        EmployeeClass nonexistent = new() { User = "ghost" };
        int result = _accountManager.ChangePassword(nonexistent, "Valid123!", "Valid123!");
        Assert.AreEqual(1, result, "Expected 1 when communication error occurs.");
    }

    [TestMethod]
    public void IsValidSuccessfulTest()
    {
        bool result = _accountManager.IsPasswordValid("ValidPass123!");
        Assert.IsTrue(result, "Expected true for valid password.");
    }

    [TestMethod]
    public void IsValidInvalidFieldTest()
    {
        bool result = _accountManager.IsPasswordValid("pass#word");
        Assert.IsFalse(result, "Expected false for password with forbidden character.");
    }

    [TestMethod]
    public void IsValidFailedTest()
    {
        bool result = _accountManager.IsPasswordValid("   ");
        Assert.IsFalse(result, "Expected false for whitespace-only password.");
    }

    [TestMethod]
    public void IsCodeValidSuccessfulTest()
    {
        string code = "123456";
        bool result = _accountManager.isCodeValid(_employee.User, code);
        Assert.IsTrue(result, "Expected true for valid verification code.");
    }

    [TestMethod]
    public void IsCodeValidInvalidFieldsTest()
    {
        bool result = _accountManager.isCodeValid("ghost", "fakecode");
        Assert.IsFalse(result, "Expected false for invalid user and code.");
    }

    [TestMethod]
    public void IsCodeValidEmptyCodeTest()
    {
        bool result = _accountManager.isCodeValid(_employee.User, "");
        Assert.IsFalse(result, "Expected false for empty verification code.");
    }

    [TestMethod]
    public void IsCodeValidFailedTest()
    {
        bool result = _accountManager.isCodeValid("ghost", null);
        Assert.IsFalse(result, "Expected false when communication fails or code is null.");
    }
}
