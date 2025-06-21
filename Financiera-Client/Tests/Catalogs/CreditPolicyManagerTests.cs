using Business_logic;
using Business_logic.Catalogs;
using DomainClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.Catalogs
{
    [TestClass]
    class CreditPolicyManagerTests
    {
        private static CreditPolicyManager _creditPolicyManager;
        private static AccountManager _accountManager;
        private static EmployeeClass _employee;
        private static Policy _policy1;
        private static Policy _policy2;

        [ClassInitialize]
        public void Initialize(TestContext testContext)
        {
            _accountManager = new();
            _creditPolicyManager = new();

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

            _policy1 = new()
            {
                Id = 1,
                Title = "PolicyTest 1",
                Description = "This is a test",
                Registrer = _employee.Id,
                State = true,
                ExpireDate = DateOnly.FromDateTime(DateTime.Now)
            };

            _policy2 = new()
            {
                Id = 2,
                Title = "PolicyTest Not one",
                Description = "This is a test again",
                Registrer = _employee.Id,
                State = true,
                ExpireDate = DateOnly.FromDateTime(DateTime.Now)
            };

            _accountManager.CreateAccount(_employee);
            _creditPolicyManager.AddPolicy(_policy1);
            _creditPolicyManager.AddPolicy(_policy2);
        }

        [TestMethod()]
        public void AddPolicySuccessfulTest()
        {
            Policy policy = new()
            {
                Title = "Valid Test Policy",
                Description = "This is a valid policy.",
                Registrer = _employee.Id,
                State = true,
                ExpireDate = DateOnly.FromDateTime(DateTime.Now.AddYears(5))
            };

            int result = _creditPolicyManager.AddPolicy(policy);

            Assert.AreEqual(0, result, "Policy should be added successfully with status code 0.");
        }

        [TestMethod()]
        public void AddPolicyInvalidFieldsTest()
        {
            Policy policy = new()
            {
                Title = "",
                Description = "Valid Description",
                Registrer = _employee.Id
            };

            int result = _creditPolicyManager.AddPolicy(policy);

            Assert.AreEqual(2, result, "Policy with invalid fields should return status code 2.");
        }

        [TestMethod()]
        public void AddPolicyEmptyPolicyTest()
        {
            Policy policy = new();
            int result = _creditPolicyManager.AddPolicy(policy);
            Assert.AreEqual(2, result, "Empty policy should return status code 2.");
        }

        [TestMethod()]
        public void UpdatePolicySuccessfulTest()
        {
            Policy updatedPolicy = new()
            {
                Id = _policy1.Id,
                Title = "Updated Title",
                Description = "Updated Description",
                Registrer = _employee.Id
            };

            int result = _creditPolicyManager.UpdatePolicy(updatedPolicy);

            Assert.AreEqual(0, result, "Valid policy update should return status code 0.");
        }

        [TestMethod()]
        public void UpdatePolicyInvalidFieldsTest()
        {
            Policy updatedPolicy = new()
            {
                Id = _policy1.Id,
                Title = "",
                Description = "Still invalid",
                Registrer = _employee.Id
            };

            int result = _creditPolicyManager.UpdatePolicy(updatedPolicy);

            Assert.AreEqual(2, result, "Updating policy with invalid fields should return status code 2.");
        }

        [TestMethod()]
        public void UpdatePolicyEmptyTest()
        {
            Policy emptyPolicy = new();
            int result = _creditPolicyManager.UpdatePolicy(emptyPolicy);
            Assert.AreEqual(2, result, "Updating empty policy should return status code 2.");
        }

        [TestMethod()]
        public void UpdatePolicyStateSuccessfulTest()
        {
            int result = _creditPolicyManager.UpdatePolicyState(_policy2.Id, false);
            Assert.AreEqual(0, result, "State update of existing policy should return status code 0.");
        }

        [TestMethod()]
        public void UpdatePolicyStateFailedTest()
        {
            int result = _creditPolicyManager.UpdatePolicyState(9999, false);
            Assert.AreEqual(1, result, "State update of non-existent policy should return status code 1.");
        }

        [TestMethod()]
        public void GetActivePoliciesSuccessfulTest()
        {
            var result = _creditPolicyManager.GetActivePolicies();
            Assert.IsNotNull(result, "The list of active policies should not be null.");
            Assert.IsTrue(result.Count > 0, "There should be at least one active policy.");
        }

        [TestMethod()]
        public void GetActivePoliciesEmptyTest()
        {
            _creditPolicyManager.UpdatePolicyState(_policy1.Id, false);
            _creditPolicyManager.UpdatePolicyState(_policy2.Id, false);
            var result = _creditPolicyManager.GetActivePolicies();

            Assert.IsNotNull(result, "The result list should not be null.");
            Assert.AreEqual(0, result.Count, "The result should be an empty list when no active policies exist.");
        }

        
    }
}
