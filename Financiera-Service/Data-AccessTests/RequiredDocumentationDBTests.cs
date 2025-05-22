using Data_AccessTests.Entities;
using Data_AccessTests.Helpers;
using Data_Access;
using Microsoft.EntityFrameworkCore;

namespace Data_AccessTests
{
    [TestClass]
    public class RequiredDocumentationDBTests
    {
        private readonly RequiredDocumentationDB _requiredDocumentationDB;
        private static RequiredDocumentation _testDocument1 = new()
        {
            name = "INE",
            state = true,
            fileType = "pdf"
        };
        private static RequiredDocumentation _testDocument2 = new()
        {
            name = "RFC",
            state = true,
            fileType = "pdf"
        };
        private static RequiredDocumentation _testDocument3 = new()
        {
            name = "CURP",
            state = true,
            fileType = "image"
        };

        public RequiredDocumentationDBTests()
        {
            _requiredDocumentationDB = new RequiredDocumentationDB();
        }

        [ClassInitialize]
        public static void LoadDatabase(TestContext testContext)
        {
            using(var context = new independent_financialContext(ConnectionStringGenerator.GetConnectionString()))
            {
                context.Database.ExecuteSqlRaw("DELETE FROM RequiredDocumentation");
                context.RequiredDocumentations.Add(_testDocument1);
                context.RequiredDocumentations.Add(_testDocument2);
                context.RequiredDocumentations.Add(_testDocument3);
                context.SaveChanges();
            }
        }

        [ClassCleanup]
        public static void CleanDataBase()
        {
            using (var context = new independent_financialContext(ConnectionStringGenerator.GetConnectionString()))
            {
                context.Database.ExecuteSqlRaw("DELETE FROM RequiredDocumentation");
            }
        }

        [TestMethod()]
        public void GetByPaginationSuccessfulTest()
        {
            var x = _requiredDocumentationDB.GetByPaginationNext(5, 0);
            Assert.IsTrue(x.Count > 0, "GetByPaginationTest");
        }

        [TestMethod()]
        public void ExistsSuccessfulTest()
        {
            Data_Access.Entities.RequiredDocumentation document = new()
            {
                name = _testDocument1.name,
                fileType = _testDocument1.fileType,
            };
            bool x = _requiredDocumentationDB.Exists(document);
            Assert.IsTrue(x, "ExistsTest");
        }

        [TestMethod()]
        public void ExistsFalseTest()
        {
            Data_Access.Entities.RequiredDocumentation document = new()
            {
                name = "IFE",
                fileType = _testDocument1.fileType,
            };
            bool x = _requiredDocumentationDB.Exists(document);
            Assert.IsFalse(x, "ExistsFalseTest");
        }

        [TestMethod()]
        public void AnotherExistsSuccessfulTest()
        {
            Data_Access.Entities.RequiredDocumentation document = new()
            {
                id = 10,
                name = _testDocument1.name,
                fileType = _testDocument1.fileType,
            };
            bool x = _requiredDocumentationDB.AnotherExists(document);
            Assert.IsTrue(x, "AnotherExistsTest");
        }

        [TestMethod()]
        public void AnotherExistsFalseTest()
        {
            Data_Access.Entities.RequiredDocumentation document = new()
            {
                id = _testDocument1.id,
                name = _testDocument1.name,
                fileType = _testDocument1.fileType,
            };
            bool x = _requiredDocumentationDB.AnotherExists(document);
            Assert.IsFalse(x, "AnotherExistsFalseTest");
        }

        [TestMethod()]
        public void AddSuccessfulTest()
        {
            int x = _requiredDocumentationDB.Add("CURP", "image");
            Assert.IsTrue(x == 1, "AddTest");
        }

        [TestMethod()]
        public void UpdateSuccessfulTest()
        {
            int x = _requiredDocumentationDB.Update(2, "Acta de nacimiento", "image");
            Assert.IsTrue(x == 1, "UpdateTest");
        }

        [TestMethod()]
        public void UpdateStateSuccessfulTest()
        {
            int x = _requiredDocumentationDB.UpdateState(3, !_testDocument3.state);
            Assert.IsTrue(x == 1, "UpdateStateTest");
        }

    }
}