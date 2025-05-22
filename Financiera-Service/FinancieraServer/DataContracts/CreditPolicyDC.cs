using Data_Access.Entities;

namespace FinancieraServer.DataContracts
{
    [DataContract]
    public class CreditPolicyDC
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public bool State { get; set; }

        [DataMember]
        public string Title { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public DateOnly EffectiveDate { get; set; }

        [DataMember]
        public int register { get; set; }
    }
}

