using System;
using System.Collections.Generic;

namespace Data_Access.Entities;

public partial class Credit
{
    public int id { get; set; }

    public string state { get; set; } = null!;

    public byte duration { get; set; }

    public int capital { get; set; }

    public string beneficiary { get; set; } = null!;

    public DateTime registryDate { get; set; }

    public int registrer { get; set; }

    public int conditionId { get; set; }

    public virtual ICollection<Document> Documents { get; set; } = new List<Document>();

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual Client beneficiaryNavigation { get; set; } = null!;

    public virtual CreditCondition condition { get; set; } = null!;

    public virtual Employee registrerNavigation { get; set; } = null!;
}
