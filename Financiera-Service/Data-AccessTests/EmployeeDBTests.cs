using Data_AccessTests.Entities;
using Data_AccessTests.Helpers;
using Data_Access;
using Microsoft.EntityFrameworkCore;

namespace Data_AccessTests
{
    [TestClass]
    class EmployeeDBTests
    {
        private readonly EmployeeDB _employeeDB;
        
        private static Data_Access.Entities.Employee _Adviser = new()
        {
            id = 100,
            user = "pale",
            password = "1234",
            name = "david",
            mail = "correo@ejemplo.com",
            address = "fei",
            phoneNumber = "1237897892",
            birthday = DateOnly.Parse("2004/12/12"),
            role = "adviser",
            sucursalId = 1

        };

        public EmployeeDBTests()
        {
            _employeeDB = new EmployeeDB();
        }

        [ClassInitialize]
        public void SetDatabase(TestContext testContext)
        {
            using (var context = new Data_Access.Entities.independent_financialContext(ConnectionStringGenerator.GetConnectionString()))
            {
                context.Database.ExecuteSqlRaw("DELETE FROM Employee");
                context.Employees.Add(_Adviser);
                context.SaveChanges();
            }
        }

        [ClassCleanup]
        public void CleanDatabase(TestContext testContext)
        {
            using (var context = new Data_Access.Entities.independent_financialContext(ConnectionStringGenerator.GetConnectionString()))
            {
                context.Database.ExecuteSqlRaw("DELETE FROM Employee");
            }
        }

        [TestMethod()]
        public void ExistsSuccessfulTest()
        {
            Data_Access.Entities.Employee employee = new()
            {
                user = _Adviser.user,
                mail = _Adviser.mail
            };
            bool t = _employeeDB.Exists(employee);
            Assert.IsTrue(t, "Failed to find an account registered in the database");
        }

        [TestMethod()]
        public void ExistsFailTest()
        {
            Data_Access.Entities.Employee employee = new()
            {
                user = "WrongUser",
                mail = "FakeMail@mail.com"
            };
            bool t = _employeeDB.Exists(employee);
            Assert.IsFalse(t, "User should not be found with invalid credentials.");
        }

        [TestMethod()]
        public void IsCorrectPasswordSuccessfulTest()
        {
            Data_Access.Entities.Employee employee = new()
            {
                user = _Adviser.user,
                password = _Adviser.password

            };
            bool t = _employeeDB.IsCorrectPassword(employee);
            Assert.IsTrue(t, "Failed to find user with valid credentials");
        }

        [TestMethod()]
        public void IsCorrectPassowrdFailedTest()
        {
            Data_Access.Entities.Employee employee = new()
            {
                user = _Adviser.user,
                password = "WrongPassword"

            };
            bool t = _employeeDB.IsCorrectPassword(employee);
            Assert.IsFalse(t, "Password validation should fail with incorrect credentials.");
        }

        [TestMethod()]
        public void GetEmployeeDataSuccesfulTest()
        {
            Data_Access.Entities.Employee employee = new()
            {
                user = _Adviser.user,
                password = _Adviser.password

            };
            var t = _employeeDB.GetEmployeeData(employee);
            Assert.IsTrue(t != null, "Failed to get the employee data");
        }

        [TestMethod()]
        public void GetEmployeeDataFailedTest()
        {
            Data_Access.Entities.Employee employee = new()
            {
                user = _Adviser.user,
                password = _Adviser.password

            };
            var t = _employeeDB.GetEmployeeData(employee);
            Assert.IsTrue(t == null, "Employee data should not be retrievable for a non-existent employee.");
        }
    }
}
