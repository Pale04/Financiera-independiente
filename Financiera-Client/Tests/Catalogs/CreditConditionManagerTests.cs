using Business_logic;
using Business_logic.Catalogs;
using DomainClasses;

namespace Tests.Catalogs
{
    [TestClass()]
    public class CreditConditionManagerTests
    {
        private static CreditConditionManager _creditConditionManager;
        private static AccountManager _accountManager;
        private static CreditCondition _creditCondition1;
        private static CreditCondition _creditCondition2;
        private static EmployeeClass _employee;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            _accountManager = new();
            _creditConditionManager = new();

            _employee = new()
            {
                Id = 1,
                User = "testUser",
                Password = "Abcd12345.",
                Name = "Test",
                Mail = "test@email.com",
                Address = "Test Address",
                PhoneNumber = "1234567890",
                Birthday = DateOnly.FromDateTime(DateTime.Now),
                Role = "admin",
                SucursalId = 1
            };
            _creditCondition1 = new()
            {
                Id = 1,
                State = true,
                InterestRate = 50,
                IVA = 16,
                PaymentsPerMonth = 1,
                RegistrerId = _employee.Id,
            };
            _creditCondition2 = new()
            {
                Id = 2,
                State = true,
                InterestRate = 7,
                IVA = 16,
                PaymentsPerMonth = 4,
                RegistrerId = _employee.Id,
            };

            _accountManager.CreateAccount(_employee);
            _creditConditionManager.AddCreditCondition(_creditCondition1);
            _creditConditionManager.AddCreditCondition(_creditCondition2);
        }

        [TestMethod()]
        public void AddCreditConditionTest()
        {
            CreditCondition newCondition = new()
            {
                InterestRate = 10,
                IVA = 16,
                PaymentsPerMonth = 2,
                RegistrerId = _employee.Id,
            };

            int result = _creditConditionManager.AddCreditCondition(newCondition);
            Assert.AreEqual(0, result, "AddCreditConditionTest");
        }

        [TestMethod()]
        public void AddCreditConditionEmptyFieldsTest()
        {
            CreditCondition newCondition = new();
            Assert.ThrowsException<Exception>(() => _creditConditionManager.AddCreditCondition(newCondition), "AddCreditConditionEmptyFieldsTest");
        }

        [TestMethod()]
        public void AddCreditConditionDuplicatedTest()
        {
            CreditCondition newCondition = new()
            {
                InterestRate = _creditCondition2.InterestRate,
                IVA = _creditCondition2.IVA,
                PaymentsPerMonth = _creditCondition2.PaymentsPerMonth,
                RegistrerId = _employee.Id,
            };
            Assert.ThrowsException<Exception>(() => _creditConditionManager.AddCreditCondition(newCondition), "AddCreditConditionDuplicatedTest");
        }

        [TestMethod()]
        public void UpdateCreditConditionTest()
        {
            CreditCondition updatedCreditCondition = new()
            {
                Id = _creditCondition1.Id,
                InterestRate = 1,
                IVA = 17,
                PaymentsPerMonth = 2
            };
            int result = _creditConditionManager.UpdateCreditCondition(updatedCreditCondition);
            Assert.AreEqual(0, result, "UpdateCreditConditionTest");
        }

        [TestMethod()]
        public void UpdateCreditCondtionWithEmptyFields()
        {
            CreditCondition updatedCreditCondition = new()
            {
                Id = _creditCondition1.Id,
            };
            Assert.ThrowsException<Exception>(() => _creditConditionManager.UpdateCreditCondition(updatedCreditCondition), "UpdateCreditCondtionWithEmptyFields");
        }

        [TestMethod()]
        public void UpdateCreditConditionDuplicatedTest()
        {
            CreditCondition updatedCreditCondtion = new()
            {
                Id = _creditCondition1.Id,
                InterestRate = _creditCondition2.InterestRate,
                IVA = _creditCondition2.IVA,
                PaymentsPerMonth = _creditCondition2.PaymentsPerMonth
            };
            Assert.ThrowsException<Exception>(() => _creditConditionManager.UpdateCreditCondition(updatedCreditCondtion), "UpdateCreditConditionDuplicatedTest");
        }

        [TestMethod()]
        public void UpdateCreditConditionStateTest()
        {
            int result = _creditConditionManager.UpdateCreditConditionState(_creditCondition1.Id, !_creditCondition1.State);
            Assert.AreEqual(0, result, "UpdateCreditConditionStateTest");
        }
    }
}