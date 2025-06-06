using Data_Access.Entities;

namespace FinancieraServer.DataContracts { 
    public class CreditPaymentDC
    {
        public int id { get; set; }

        public string state { get; set; } = null!;

        public byte duration { get; set; }

        public int capital { get; set; }

        public string beneficiary { get; set; } = null!;

        public DateTime registryDate { get; set; }

        public int registrer { get; set; }

        public int conditionId { get; set; }

        public int interestRate { get; set; }

        public int IVA { get; set; }

        public int paymentsPerMonth { get; set; }
    }
}