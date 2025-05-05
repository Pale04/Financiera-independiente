using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancieraServer.DataContracts
{
    [DataContract]
    public class SubsidiaryDC
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Address { get; set; }

        [DataMember]
        public bool State { get; set; }
    }
}
