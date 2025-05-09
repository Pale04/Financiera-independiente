using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancieraServer.DataContracts
{
    [DataContract]
    public class CreditDC
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string State { get; set; }
        [DataMember]
        public int Duration { get; set; }
        [DataMember]
        public int Capital { get; set; }
        [DataMember]
        public int registrerId { get; set; }
        [DataMember]
        public int beneficiaryId { get; set; }
        [DataMember]
        public int ConditionId { get; set; }
    }
}
