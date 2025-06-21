using Data_Access;
using Data_Access.Entities;
using Data_AccessTests.Helpers;
using Microsoft.EntityFrameworkCore;

namespace Data_AccessTests
{
    [TestClass]
    public class SubsibidiaryDBTests
    {
        private static SubsidiaryDB _subsidiaryDB;

        [ClassInitialize]
        public static void Initialize(TestContext context)
        {
            _subsidiaryDB = new SubsidiaryDB();
            CleanDatabase();

            using (var db = new independent_financialContext(ConnectionStringGenerator.GetConnectionString()))
            {
                db.Subsidiaries.Add(new()
                {
                    address = "Calle del skibidipapu #32, Ciudad Olmeca, Mexico",
                    state = true
                });
            }
        }

        private static void CleanDatabase()
        {
            using (var db = new independent_financialContext(ConnectionStringGenerator.GetConnectionString()))
            {
                db.Database.ExecuteSqlRaw("DELETE FROM Payment");
                db.Database.ExecuteSqlRaw("DELETE FROM Document");
                db.Database.ExecuteSqlRaw("DELETE FROM Credit");
                db.Database.ExecuteSqlRaw("DELETE FROM RequiredDocumentation");
                db.Database.ExecuteSqlRaw("DELETE FROM CreditPolicy");
                db.Database.ExecuteSqlRaw("DELETE FROM Creditcondition");
                db.Database.ExecuteSqlRaw("DELETE FROM BankAccount");
                db.Database.ExecuteSqlRaw("DELETE FROM PersonalReference");
                db.Database.ExecuteSqlRaw("DELETE FROM Client");
                db.Database.ExecuteSqlRaw("DELETE FROM Employee");
                db.Database.ExecuteSqlRaw("DELETE FROM Subsidiary");
            }
        }

        [TestMethod]
        public static void UpdateStateSuccessfulTest()
        {
            int result = _subsidiaryDB.UpdateState(1, true);
            Assert.AreEqual(1, result, "Expected 1 for successful update.");
        }

        [TestMethod]
        public static void UpdateStateFailedfulTest()
        {
            int result = _subsidiaryDB.UpdateState(9999, true);
            Assert.AreEqual(0, result, "Expected 0 for non-existent subsidiary ID.");
        }

        [TestMethod]
        public static void AddNewSuccessfulTest()
        {
            int result = _subsidiaryDB.Add("Av. Xalapa #333, Xalapa, Ver, Mexico");
            Assert.AreEqual(1, result, "Expected 1 for successful creation of subsidiary.");
        }

        [TestMethod]
        public static void UpdateAddressSuccessfulTest() 
        {
            int result = _subsidiaryDB.UpdateAddress(1, "Av. Xalapa #333, Xalapa, Ver, Mexico");
            Assert.AreEqual(1, result, "Expected 1 for successful update of subsidiary.");
        }

        [TestMethod]
        public static void UpdateAddressFailedTest()
        {
            int result = _subsidiaryDB.UpdateAddress(9999, "Av. Xalapa #333, Xalapa, Ver, Mexico");
            Assert.AreEqual(0, result, "Expected 0 for non-existent subsidiary ID.");
        }

        [TestMethod]
        public static void FindExistingSuccessfulTest()
        {
            Assert.IsTrue(_subsidiaryDB.Exists("Av. Xalapa #333, Xalapa, Ver, Mexico"));
        }

        [TestMethod]
        public static void FindExistingFailedTest()
        {
            Assert.IsFalse(_subsidiaryDB.Exists("Hola mundo"));
        }

        [TestMethod]
        public static void FindAnotherExistingSuccessfulTest()
        {
            Subsidiary nonExistingSubsidiary = new()
            {
                address = "Calle del skibidipapu #32, Ciudad Olmeca, Mexico",
                state = true,
                id = 999
            };

            Assert.IsTrue(_subsidiaryDB.AnotherExists(nonExistingSubsidiary));
        }

        [TestMethod]
        public static void FindAnotherExistingFailedTest()
        {
            Subsidiary existingSubsidiary = new()
            {
                address = "Calle del skibidipapu #32, Ciudad Olmeca, Mexico",
                state = true,
                id = 1
            };

            Assert.IsFalse(_subsidiaryDB.AnotherExists(existingSubsidiary));
        }
    }
}
