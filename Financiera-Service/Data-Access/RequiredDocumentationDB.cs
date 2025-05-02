using Data_Access.Entities;

namespace Data_Access
{
    public class RequiredDocumentationDB
    {
        public List<RequiredDocumentation> GetByPaginationNext(int pageSize, int lastId)
        {
            List<RequiredDocumentation> documents = [];
            using (var context = new independent_financialContext(ConnectionStringGenerator.GetConnectionString(ConnectionRole.Reader)))
            {
                documents = context.RequiredDocumentations
                    .OrderBy(d => d.id)
                    .Where(d => d.id > lastId)
                    .Take(pageSize)
                    .ToList();
            }
            return documents;
        }

        public List<RequiredDocumentation> GetByPaginationPrevious(int pageSize, int firstId)
        {
            List<RequiredDocumentation> documents = new();
            using (var context = new independent_financialContext(ConnectionStringGenerator.GetConnectionString(ConnectionRole.Reader)))
            {
                documents = context.RequiredDocumentations
                    .OrderBy(d => d.id)
                    .Where(d => d.id < firstId)
                    .ToList()
                    .TakeLast(pageSize)
                    .ToList();
            }
            return documents;
        }

        public bool AnotherExists(RequiredDocumentation document)
        {
            using (var context = new independent_financialContext(ConnectionStringGenerator.GetConnectionString(ConnectionRole.Reader)))
            {
                return context.RequiredDocumentations.Any(d => d.name == document.name && d.id != document.id);
            }
        }

        public bool Exists(RequiredDocumentation document)
        {
            using (var context = new independent_financialContext(ConnectionStringGenerator.GetConnectionString(ConnectionRole.Reader)))
            {
                return context.RequiredDocumentations.Any(d => d.name == document.name);
            }
        }

        public int Add(string name, string fileType)
        {
            int result = 0;
            using (var context = new independent_financialContext(ConnectionStringGenerator.GetConnectionString(ConnectionRole.Administrator)))
            {
                var newDocument = new RequiredDocumentation
                {
                    name = name,
                    state = true,
                    fileType = fileType
                };

                context.RequiredDocumentations.Add(newDocument);
                result = context.SaveChanges();
            }
            return result;
        }

        public int Update(int id, string name, string fileType)
        {
            int result = 0;
            using (var context = new independent_financialContext(ConnectionStringGenerator.GetConnectionString(ConnectionRole.Administrator)))
            {
                var document = context.RequiredDocumentations.Find(id);
                if (document != null)
                {
                    document.name = name;
                    document.fileType = fileType;
                    result = context.SaveChanges();
                }
            }
            return result;
        }

        public int UpdateState(int id, bool isActive)
        {
            int result = 0;
            using (var context = new independent_financialContext(ConnectionStringGenerator.GetConnectionString(ConnectionRole.Administrator)))
            {
                var document = context.RequiredDocumentations.Find(id);
                if (document != null)
                {
                    document.state = isActive;
                    result = context.SaveChanges();
                }
            }
            return result;
        }
    }
}
