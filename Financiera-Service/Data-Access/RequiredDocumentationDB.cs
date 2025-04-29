using Data_Access.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_Access
{
    public class RequiredDocumentationDB
    {
        public int addDocument()
        {
            int result = -1;
            using (var context = new independent_financialContext(ConnectionsStringGenerator.GetConnectionString(ConnectionRole.Administrator)))
            {
                var document = new RequiredDocumentation
                {
                    name = "Test",
                    state = true,
                    fileType = "pdf"
                };

                context.RequiredDocumentations.Add(document);
                result = context.SaveChanges();
            }

            return result;
        }
    }
}
