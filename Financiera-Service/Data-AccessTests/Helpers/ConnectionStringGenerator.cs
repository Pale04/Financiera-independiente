using Microsoft.VisualStudio.TestPlatform.ObjectModel;

namespace Data_AccessTests.Helpers
{
    internal static class ConnectionStringGenerator
    {
        public static string GetConnectionString()
        {
            return $"server={Environment.GetEnvironmentVariable("SqlServer_Name")};uid=financialTester;pwd={Environment.GetEnvironmentVariable("IndependentFinancial_TesterPwd")};database=independent_financial;TrustServerCertificate=True";
        }
    }
}
