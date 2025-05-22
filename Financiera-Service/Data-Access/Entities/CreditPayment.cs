using System;
using System.Collections.Generic;

namespace Data_Access.Entities;

public partial class CreditPayment
{
    public int id { get; set; }

    public int capital { get; set; }

    public byte duration { get; set; }

    public string beneficiary { get; set; } = null!;

    public string state { get; set; } = null!;

    public int registrer { get; set; }

    public DateTime registryDate { get; set; }

    public int conditionId { get; set; }

    public int interestRate { get; set; }

    public int paymentsPerMonth { get; set; }

    public int IVA { get; set; }
}
