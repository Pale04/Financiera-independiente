using Data_Access;
using Data_Access.Entities;
using Data_AccessTests.Helpers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_AccessTests
{
    [TestClass]
    class CreditPoliciesDBTests
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
            id = 1,
            title = "Mexicano/a",
            description = "El solicitante debe tener nacionalidad mexicana",
            registrer = 1,
            effectiveDate = DateOnly.Parse("2030-05-21"),
            state = true
        };

        public CreditPoliciesDBTests()
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


        [TestMethod()]
        public void GetByPageTest()
        {
            var p = _creditPolicyDB.GetByPageNext(6, 0);
            Assert.IsTrue(p.Count > 0, "GetByPageTest");
        }

        [TestMethod()]
        public void ExistsSuccesfulTest()
        {
            Data_Access.Entities.CreditPolicy policy = new()
            {
                id = _creditPolicy1.id,
                title = _creditPolicy1.title,

            };
        }

        [TestMethod()]
        public void ExistsFailTest()
        {

        }

        [TestMethod()]
        public void AddPolicySuccessTest()
        {

        }

        [TestMethod()]
        public void AddPolicyFailTest()
        {

        }

        [TestMethod()]
        public void UpdatePolicySuccessTest()
        {

        }

        [TestMethod()]
        public void UpdatePolicyFailTest()
        {

        }

        [TestMethod()]
        public void UpdatePolicyStateSuccessTest()
        {

        }

        [TestMethod()]
        public void UpdatePolicyStateFailTest()
        {

        }

        [TestMethod()]
        public void IdExistsSuccessTest()
        {

        }

        [TestMethod()]
        public void TdExistsFailTest()
        {

        }


    }
}
