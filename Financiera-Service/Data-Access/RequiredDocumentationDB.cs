using Data_Access.Entities;

namespace Data_Access
{
    public class RequiredDocumentationDB
    {
        public List<RequiredDocumentation> GetByPagination(int pageSize, int lastId)
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

        public bool Exists(string name, string fileType)
        {
            using (var context = new independent_financialContext(ConnectionStringGenerator.GetConnectionString(ConnectionRole.Reader)))
            {
                return context.RequiredDocumentations.Any(d => d.name == name && d.fileType == fileType);
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
