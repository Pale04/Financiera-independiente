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

        public Response AddRequiredDocument(RequiredDocumentDC requiredDocument)
        {
            if(string.IsNullOrEmpty(requiredDocument.Name) || string.IsNullOrEmpty(requiredDocument.FileType))
            {
                _logger.LogInformation("Attempt of add required documentation with empty name or file type");
                return new Response(2, "Invalid name or file type");
            }

            RequiredDocumentationDB requiredDocumentationDB = new();
            RequiredDocumentation document = new()
            {
                name = requiredDocument.Name,
                fileType = requiredDocument.FileType
            };

            try
            {
                if (requiredDocumentationDB.Exists(document))
                {
                    _logger.LogInformation("Attempt of add required documentation with existing name {name} and file type {fileType}", requiredDocument.Name, requiredDocument.FileType);
                    return new Response(3, "Required documentation already exists");
                }
                requiredDocumentationDB.Add(requiredDocument.Name, requiredDocument.FileType);
            }
            catch (DbException error)
            {
                _logger.LogWarning("Error while attempting to add RequiredDocumentation {error}", error);
                return new Response(1, "Error while attempting to add RequiredDocumentation");
            }

            return new Response(0);
        }

        public ResponseWithContent<List<RequiredDocumentDC>> GetRequiredDocumentationByPagination(int pageSize, int lastId)
        {
            if (pageSize <= 0 || lastId < 0)
            {
                _logger.LogInformation("Attempt of get required documentation with {pageSize} page size and {lastId} last id", pageSize, lastId);
                return new ResponseWithContent<List<RequiredDocumentDC>>(2,"Invalid page size or last ID");
            }

            RequiredDocumentationDB requiredDocumentationDB = new();
            List<RequiredDocumentation> databaseRequiredDocuements = [];

            try
            {
                databaseRequiredDocuements = requiredDocumentationDB.GetByPagination(pageSize, lastId);
            }
            catch (DbException error)
            {
                _logger.LogWarning("Error while attempting to get RequiredDocumentations {error}", error);
                return new ResponseWithContent<List<RequiredDocumentDC>>(1, "Error while attempting to get RequiredDocumentations");
            }

            List<RequiredDocumentDC> requiredDocuments = [];
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

            return new ResponseWithContent<List<RequiredDocumentDC>>(0, requiredDocuments);
        }

        public Response UpdateRequiredDocument(RequiredDocumentDC requiredDocument)
        {
            if (requiredDocument.Id < 1)
            {
                _logger.LogInformation("Attempt of update required documentation with invalid id {id}", requiredDocument.Id);
                return new Response(2, "Invalid ID");
            }

            RequiredDocumentationDB requiredDocumentationDB = new();
            RequiredDocumentation document = new()
            {
                id = requiredDocument.Id,
                name = requiredDocument.Name,
                fileType = requiredDocument.FileType
            };

            try
            {
                if (requiredDocumentationDB.AnotherExists(document))
                {
                    _logger.LogInformation("Attempt of update required documentation with document existing name {name} and file type {fileType}", requiredDocument.Name, requiredDocument.FileType);
                    return new Response(3, "Required documentation already exists");
                }
                requiredDocumentationDB.Update(requiredDocument.Id, requiredDocument.Name, requiredDocument.FileType);
            }
            catch (DbException error)
            {
                _logger.LogWarning("Error while attempting to update RequiredDocumentation {error}", error);
                return new Response(1, "Error while attempting to update RequiredDocumentation");
            }

            return new Response(0);
        }

        public Response UpdateRequiredDocumentState(int id, bool isActive)
        {
            if (id < 1)
            {
                _logger.LogInformation("Attempt of update required documentation with invalid id {id}", id);
                return new Response(2, "Invalid ID");
            }

            RequiredDocumentationDB requiredDocumentationDB = new();

            try
            {
                requiredDocumentationDB.UpdateState(id, isActive);
            }
            catch (DbException error)
            {
                _logger.LogWarning("Error while attempting to update RequiredDocumentation status {error}", error);
                return new Response(1, "Error while attempting to update RequiredDocumentation status");
            }

            return new Response(0);
        }
    }
}
