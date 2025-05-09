using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancieraServer.DataContracts
{
    [DataContract]
    public class CreditRequestDC
    {
        [DataMember]
        public CreditDC Credit { get; set; }
        
        [DataMember]
        public CreditDocumentDC[] Documents { get; set; }
    }
}
