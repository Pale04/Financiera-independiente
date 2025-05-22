using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainClasses
{
    public class Document
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public DateTime RegistryDate { get; set; }

        public int Registrer { get; set; }

        public int DocumentationId { get; set; }

        public int CreditId { get; set; }

        public byte[] File {  get; set; }
    }
}
