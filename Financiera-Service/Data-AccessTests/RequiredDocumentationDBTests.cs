namespace Data_Access.Tests
{
    [TestClass()]
    public class RequiredDocumentationDBTests
    {
        [TestMethod()]
        public void Test()
        {
            RequiredDocumentationDB requiredDocumentationDB = new RequiredDocumentationDB();
            var x = requiredDocumentationDB.addDocument();
            Assert.IsTrue(x != -1, $"{x}");
        }
    }
}