using FinancieraServer.DataContracts;
using FinancieraServer.Interfaces;
using Data_Access;
using Data_Access.Entities;
using System.Data.Common;

namespace FinancieraServer.ServiceImplementations
{
    public partial class CatalogService : ICatalogService
    {
        private ILogger<CatalogService> _logger;

        public CatalogService(ILogger<CatalogService> logger)
        {
            _logger = logger;
        }

        public ResponseWithContent<List<RequiredDocumentDC>> GetRequiredDocumentationByPagination(int pageSize, int lastId)
        {
            RequiredDocumentationDB requiredDocumentationDB = new();
            List<RequiredDocumentDC> requiredDocuments = [];
            List<RequiredDocumentation> databaseRequiredDocuements = [];

            try
            {
                databaseRequiredDocuements = requiredDocumentationDB.GetByPagination(pageSize, lastId);
            }
            catch (DbException error)
            {
                _logger.LogWarning("Error while attempting to get RequiredDocumentations {error}", error);
                return new ResponseWithContent<List<RequiredDocumentDC>>
                {
                    StatusCode = 1,
                    Message = "Error while attempting to get RequiredDocumentations",
                };
            }
            
            foreach (RequiredDocumentation document in databaseRequiredDocuements)
            {
                requiredDocuments.Add(new RequiredDocumentDC
                {
                    Id = document.id,
                    Name = document.name,
                    FileType = document.fileType,
                    State = document.state
                });
            }

            return new ResponseWithContent<List<RequiredDocumentDC>> {
                StatusCode = 0,
                Data = requiredDocuments
            };
        }
    }
}
