using Business_logic.Catalogs;
using DomainClasses;

namespace Catalogs.Tests
{
    [TestClass()]
    public class RequiredDocumentationManagerTests
    {
        private static RequiredDocumentationManager _requiredDocumentationManager;
        private static RequiredDocument _requiredDocument1;
        private static RequiredDocument _requiredDocument2;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            _requiredDocumentationManager = new RequiredDocumentationManager();
            _requiredDocument1 = new()
            {
                Id = 1,
                Name = "CURP",
                FileType = FileType.image
            };
            _requiredDocument2 = new()
            {
                Id = 2,
                Name = "Acta de nacimiento",
                FileType = FileType.pdf
            };
            _requiredDocumentationManager.AddRequiredDocument(_requiredDocument1);
            _requiredDocumentationManager.AddRequiredDocument(_requiredDocument2);
        }

        [TestMethod()]
        public void AddRequiredDocumentTest()
        {
            RequiredDocument newDocument = new()
            {
                Name = "Test Document",
                FileType = FileType.image,
            };

            int result = _requiredDocumentationManager.AddRequiredDocument(newDocument);
            Assert.AreEqual(0, result, "AddRequiredDocumentTest");
        }

        [TestMethod()]
        public void AddRequiredDocumentWithEmptyFieldsTest()
        {
            RequiredDocument newDocument = new()
            {
                Name = ""
            };

            Assert.ThrowsException<Exception>(() => _requiredDocumentationManager.AddRequiredDocument(newDocument), "AddRequiredDocumentWithEmptyFieldsTest");
        }

        [TestMethod()]
        public void AddRequiredDocumentDuplicatedTest()
        {
            RequiredDocument newDocument = new()
            {
                Name = _requiredDocument2.Name,
                FileType = _requiredDocument2.FileType
            };

            Assert.ThrowsException<Exception>(() => _requiredDocumentationManager.AddRequiredDocument(newDocument), "AddRequiredDocumentDuplicateTest");
        }

        [TestMethod()]
        public void UpdateRequiredDocumentTest()
        {
            RequiredDocument updatedDocument = new()
            {
                Id = _requiredDocument1.Id,
                Name = "Certificado de preparatoria",
                FileType = FileType.pdf
            };

            int result = _requiredDocumentationManager.UpdateRequiredDocument(updatedDocument);
            Assert.AreEqual(0, result, "UpdateRequiredDocumentTest");
        }

        [TestMethod()]
        public void UpdateRequiredDocumentWithEmptyFieldsTest()
        {
            RequiredDocument updatedDocument = new()
            {
                Id = 0,
                Name = ""
            };

            Assert.ThrowsException<Exception>(() => _requiredDocumentationManager.UpdateRequiredDocument(updatedDocument), "UpdateRequiredDocumentWithEmptyFieldsTest");
        }

        [TestMethod()]
        public void UpdateRequiredDocumentDuplicatedTest()
        {
            RequiredDocument updatedDocument = new()
            {
                Id = _requiredDocument1.Id,
                Name = _requiredDocument2.Name,
                FileType = _requiredDocument2.FileType
            };

            Assert.ThrowsException<Exception>(() => _requiredDocumentationManager.UpdateRequiredDocument(updatedDocument), "UpdateRequiredDocumentDuplicateTest");
        }

        [TestMethod()]
        public void UpdateRequieredDocumentStateTest()
        {
            RequiredDocument updatedDocument = new()
            {
                Id = _requiredDocument1.Id,
                Status = false,
            };

            int result = _requiredDocumentationManager.UpdateRequireDocumentStatus(updatedDocument);
            Assert.AreEqual(0, result, "UpdateRequieredDocumentStateTest");
        }
    }
}