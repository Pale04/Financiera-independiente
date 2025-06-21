using Data_Access;
using Data_Access.Entities;
using Data_AccessTests.Helpers;
using Microsoft.EntityFrameworkCore;

namespace Data_AccessTests
{
    [TestClass]
    public class CreditPolicyDBTests
    {
        private readonly CreditPolicyDB _creditPolicyDB;
        private static Data_Access.Entities.CreditPolicy _creditPolicy1 = new()
        {
            id = 1,
            title = "Mayor de edad",
            description = "El solicitante debe ser mayor de 18 años.",
            registrer = 1,
            effectiveDate = DateOnly.Parse("2030-05-21"),
            state = true
        };

        private static Data_Access.Entities.CreditPolicy _creditPolicy2 = new()
        {
            id = 2,
            title = "Menor de 60 años",
            description = "El solicitante debe tener 60 años o menos",
            registrer = 1,
            effectiveDate = DateOnly.Parse("2030-05-15"),
            state = true
        };

        private static Data_Access.Entities.CreditPolicy _creditPolicy3 = new()
        {
            id = 3,
            title = "Mexicano/a",
            description = "El solicitante debe tener nacionalidad mexicana",
            registrer = 1,
            effectiveDate = DateOnly.Parse("2030-05-21"),
            state = true
        };

        public CreditPolicyDBTests()
        {
            _creditPolicyDB = new CreditPolicyDB();
        }


        [ClassInitialize]
        public void SetDatabase(TestContext testContext)
        {
            using (var context = new independent_financialContext(ConnectionStringGenerator.GetConnectionString()))
            {
                context.Database.ExecuteSqlRaw("DELETE FROM CreditPolicy");
                context.CreditPolicies.Add(_creditPolicy1);
                context.CreditPolicies.Add(_creditPolicy2);
                context.CreditPolicies.Add(_creditPolicy3);
                context.SaveChanges();
            }
        }

        [ClassCleanup]
        public void CleanDatabase(TestContext testContext)
        {
            using (var context = new independent_financialContext(ConnectionStringGenerator.GetConnectionString()))
            {
                context.Database.ExecuteSqlRaw("DELETE FROM CreditPolicy");
            }
        }


        [TestMethod]
        public void GetByPageTest()
        {
            var p = _creditPolicyDB.GetByPageNext(6, 0);
            Assert.IsTrue(p.Count > 0, "GetByPageTest");
        }

        [TestMethod]
        public void ExistsSuccesfulTest()
        {
            Data_Access.Entities.CreditPolicy policy = new()
            {
                title = _creditPolicy1.title,
                description = _creditPolicy1.title
            };

            var exists = _creditPolicyDB.Exists(policy);
            Assert.IsTrue(exists, "Failed to found an existing policy");
        }

        [TestMethod]
        public void ExistsFailTest()
        {
            var policy = new Data_Access.Entities.CreditPolicy
            {
                title = "Not Exists",
                description = "Fake description"
            };

            var exists = _creditPolicyDB.Exists(policy);
            Assert.IsFalse(exists, "Cannot found something doesnt exists");
        }

        [TestMethod]
        public void AddPolicySuccessTest()
        {
            var newPolicy = new Data_Access.Entities.CreditPolicy
            {
                title = "New Policy",
                description = "Policy Added in tests",
                registrer = 1
            };

            var result = _creditPolicyDB.AddCreditPolicy(newPolicy);
            Assert.AreEqual(0, result, "The policie should be added");
        }

        [TestMethod]
        public void AddPolicyFailTest()
        {
            var result = _creditPolicyDB.AddCreditPolicy(null);
            Assert.AreEqual(1, result, "Cannot Add null");
        }

        [TestMethod]
        public void UpdatePolicySuccessTest()
        {
            var updatedPolicy = new Data_Access.Entities.CreditPolicy
            {
                id = _creditPolicy1.id,
                title = "Actualizada",
                description = "Descripción actualizada",
                effectiveDate = DateOnly.FromDateTime(DateTime.Today),
                registrer = 2
            };

            var result = _creditPolicyDB.UpdateCreditPolicy(updatedPolicy);
            Assert.AreEqual(0, result, "Policy failed to update");
        }

        [TestMethod]
        public void UpdatePolicyFailTest()
        {
            var nonExistentPolicy = new Data_Access.Entities.CreditPolicy
            {
                id = 9999,
                title = "Not exists",
                description = "Not a real policy",
                effectiveDate = DateOnly.FromDateTime(DateTime.Today),
                registrer = 1
            };

            var result = _creditPolicyDB.UpdateCreditPolicy(nonExistentPolicy);
            Assert.AreEqual(4, result, "Shoulded return code 4");
        }

        [TestMethod]
        public void UpdatePolicyStateSuccessTest()
        {
            var result = _creditPolicyDB.UpdateStateCreditPolicy(false, _creditPolicy2.id);
            Assert.AreEqual(0, result, "The policy state shoulded been updated");
        }

        [TestMethod]
        public void UpdatePolicyStateFailTest()
        {
            var result = _creditPolicyDB.UpdateStateCreditPolicy(false, 9999); // ID no existente
            Assert.AreEqual(4, result, "Failed to return code 4");
        }

        [TestMethod]
        public void IdExistsSuccessTest()
        {
            var result = _creditPolicyDB.IdExists(_creditPolicy1.id);
            Assert.IsTrue(result, "Failed to get an existing policy id");
        }

        [TestMethod]
        public void IdExistsFailTest()
        {
            var result = _creditPolicyDB.IdExists(9999);
            Assert.IsFalse(result, "Should not found an non existing ID");
        }
    }
}
