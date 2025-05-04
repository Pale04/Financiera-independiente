using FinancieraServer.DataContracts;
using Data_Access;
using Data_Access.Entities;
using System.Data.Common;

namespace FinancieraServer.ServiceImplementations
{
    public partial class CatalogService
    {
        public Response AddRequiredDocument(RequiredDocumentDC requiredDocument)
        {
            if(string.IsNullOrWhiteSpace(requiredDocument.Name) || string.IsNullOrWhiteSpace(requiredDocument.FileType))
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

        public ResponseWithContent<List<RequiredDocumentDC>> GetRequiredDocumentationByPaginationNext(int pageSize, int lastId)
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
                databaseRequiredDocuements = requiredDocumentationDB.GetByPaginationNext(pageSize, lastId);
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

        public ResponseWithContent<List<RequiredDocumentDC>> GetRequiredDocumentationByPaginationPrevious(int pageSize, int firstId)
        {
            if (pageSize <= 0 || firstId < 0)
            {
                _logger.LogInformation("Attempt of get required documentation with {0} page size and {1} first id", pageSize, firstId);
                return new ResponseWithContent<List<RequiredDocumentDC>>(2, "Invalid page size or last ID");
            }

            RequiredDocumentationDB requiredDocumentationDB = new();
            List<RequiredDocumentation> databaseRequiredDocuements = new();

            try
            {
                databaseRequiredDocuements = requiredDocumentationDB.GetByPaginationPrevious(pageSize, firstId);
            }
            catch (DbException error)
            {
                _logger.LogWarning("Error while attempting to get RequiredDocumentations {0}", error);
                return new ResponseWithContent<List<RequiredDocumentDC>>(1, "Error while attempting to get RequiredDocumentations");
            }

            List<RequiredDocumentDC> requiredDocuments = new();
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
                    _logger.LogInformation("Attempt of update required documentation with document existing values");
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
