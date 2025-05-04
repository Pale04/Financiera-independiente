using FinancieraServer.Interfaces;

namespace FinancieraServer.ServiceImplementations
{
    public partial class CatalogService : ICatalogService
    {
        private ILogger<CatalogService> _logger;

        public CatalogService(ILogger<CatalogService> logger)
        {
            _logger = logger;
        }
    }
}
