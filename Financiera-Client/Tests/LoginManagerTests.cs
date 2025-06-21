using Business_logic;
using DomainClasses;

namespace Tests;

[TestClass]
public class LoginManagerTests
{
    LoginManager _loginManager;
    private static AccountManager _accountManager;
    private static EmployeeClass _employee;

    [ClassInitialize]
    public void Initialize(TestContext context)
    {
        _accountManager = new();
        _loginManager = new();

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

    [TestMethod()]
    public void LoginSuccessTest()
    {
        int result = _loginManager.Login(_employee);
        Assert.AreEqual(0, result, "Valid credentials should return status code 0.");
    }

    [TestMethod()]
    public void LoginInvalidCredentials()
    {
        EmployeeClass invalidEmployee = new()
        {
            User = "testUser",
            Password = "WrongPassword123"
        };

        int result = _loginManager.Login(invalidEmployee);
        Assert.AreEqual(2, result, "Incorrect password should return status code 2.");
    }

    [TestMethod()]
    public void LoginEmptyFields()
    {
        EmployeeClass emptyEmployee = new();

        int result = _loginManager.Login(emptyEmployee);
        Assert.AreEqual(1, result, "Empty fields should return status code 1.");
    }

    [TestMethod()]
    public void GetSessionInfoSuccessfulTest()
    {
        var sessionInfo = _loginManager.getSessionInfo(_employee.User);
        Assert.IsNotNull(sessionInfo, "Session info should not be null for a valid user.");
        Assert.AreEqual(_employee.User, sessionInfo.User, "Usernames should match.");
        Assert.AreEqual(_employee.Role, sessionInfo.Role, "Roles should match.");
    }

    [TestMethod()]
    public void GetSessionInfoFailedTest()
    {
        Assert.ThrowsException<Exception>(
        () => _loginManager.getSessionInfo("nonexistentUser123"),
        "Expected an exception when retrieving session info for a non-existent user."
    );
    }

    [TestMethod()]
    public void LogoutSuccessfulTest()
    {
        _loginManager.Login(_employee); // Ensure session exists
        int result = _loginManager.Logout(_employee.User);
        Assert.AreEqual(0, result, "Successful logout should return status code 0.");
    }

    [TestMethod()]
    public void LogoutFailedTest()
    {
        Assert.ThrowsException<Exception>(
        () => _loginManager.Logout("invalidUserXYZ"),
        "Expected an exception when trying to logout a non-existent user."
    );
    }

}
