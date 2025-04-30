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
            string password;
            string connectionString = string.Empty;

            switch (role)
            {
                case ConnectionRole.Reader:
                    password = Environment.GetEnvironmentVariable("IndependentFinancial_ReaderPwd");
                    connectionString = string.Format(connectionStringPlaceholder, server, "financialReader", password);
                    break;
                case ConnectionRole.Administrator:
                    password = Environment.GetEnvironmentVariable("IndependentFinancial_AdministratorPwd");
                    connectionString = string.Format(connectionStringPlaceholder, server, "financialAdmin", password);
                    break;
                case ConnectionRole.AccountModificator:
                    //TODO
                    break;
                case ConnectionRole.Analyst:
                    //TODO
                    break;
                case ConnectionRole.CollectionsAgent:
                    //TODO
                    break;
                case ConnectionRole.LoanOfficer:
                    //TODO
                    break;
            }

            return connectionString;
        }
    }
}
