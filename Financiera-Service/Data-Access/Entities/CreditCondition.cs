using System;
using System.Collections.Generic;

namespace Data_Access.Entities;

public partial class CreditCondition
{
    public int id { get; set; }

    public bool state { get; set; }

    public int interestRate { get; set; }

    public int IVA { get; set; }

    public int paymentsPerMonth { get; set; }

    public int registrer { get; set; }

    public virtual ICollection<Credit> Credits { get; set; } = new List<Credit>();

    public virtual Employee registrerNavigation { get; set; } = null!;
}
