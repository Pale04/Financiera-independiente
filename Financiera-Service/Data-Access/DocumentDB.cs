﻿using Data_Access.Entities;

namespace Data_Access
{
    public class DocumentDB
    {
        public List<Document> GetByCreditId(int creditId)
        {
            using (var context = new independent_financialContext(ConnectionStringGenerator.GetConnectionString(ConnectionRole.Reader)))
            {
                return context.Documents.Where(d => d.creditId == creditId).ToList();
            }
        }

        public int Add(Document document)
        {
            int result = 0;

            using (var context = new independent_financialContext(ConnectionStringGenerator.GetConnectionString(ConnectionRole.LoanOfficer)))
            {
                context.Add(document);
                result = context.SaveChanges();
            }

            return result;
        }

        public int ReplaceDocument(Document newDocument, int oldDocumentId)
        {
            int result = 0;
            using (var context = new independent_financialContext(ConnectionStringGenerator.GetConnectionString(ConnectionRole.LoanOfficer)))
            {
                Document document = context.Documents.Find(oldDocumentId);

                if (document != null)
                {
                    document.active = false;
                    context.Add(newDocument);
                    result = context.SaveChanges();
                }
            }

            return result;
        }
    }
}