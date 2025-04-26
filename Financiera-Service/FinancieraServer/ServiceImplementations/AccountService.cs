using FinancieraServer.DataContracts;
using FinancieraServer.Interfaces;

namespace FinancieraServer.ServiceImplementations
{
    public class AccountService : IAccountService
    {
        private ILogger<AccountService> _logger;

        public AccountService(ILogger<AccountService> logger)
        {
            _logger = logger;
        }

        public Response createAccount()
        {
            _logger.LogInformation("Prueba superada");
            throw new NotImplementedException();
        }
    }
}
