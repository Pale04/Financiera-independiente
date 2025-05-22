namespace Data_Access
{
    internal enum ConnectionRole
    {
        Reader,
        Administrator,
        AccountModificator,
        Analyst,
        CollectionsAgent,
        LoanOfficer
    }

    internal static class ConnectionStringGenerator
    {
        public static string GetConnectionString(ConnectionRole role)
        {
            string connectionStringPlaceholder = "server={0};uid={1};pwd={2};database=independent_financial;TrustServerCertificate=True";
            string server = Environment.GetEnvironmentVariable("SqlServer_Name");
            string password = "";
            string user = "";
            string connectionString = string.Empty;

            switch (role)
            {
                case ConnectionRole.Reader:
                    password = Environment.GetEnvironmentVariable("IndependentFinancial_ReaderPwd");
                    user = "financialReader";
                    break;
                case ConnectionRole.Administrator:
                    password = Environment.GetEnvironmentVariable("IndependentFinancial_AdministratorPwd");
                    user = "financialAdmin";
                    break;
                case ConnectionRole.AccountModificator:
                    password = Environment.GetEnvironmentVariable("IndependentFinancial_AccountModificatorPwd");
                    user = "financialAccountModificator";
                    break;
                case ConnectionRole.Analyst:
                    password = Environment.GetEnvironmentVariable("IndependentFinancial_AnalystPwd");
                    user = "financialAnalist";
                    break;
                case ConnectionRole.CollectionsAgent:
                    password = Environment.GetEnvironmentVariable("IndependentFinancial_CollectionsAgentPwd");
                    user = "financialCollectionsAgent";
                    break;
                case ConnectionRole.LoanOfficer:
                    password = Environment.GetEnvironmentVariable("IndependentFinancial_LoanOfficerPwd");
                    user = "financialLoanOfficer";
                    break;
            }
            
            return string.Format(connectionStringPlaceholder, server, user, password);
        }
    }
}
