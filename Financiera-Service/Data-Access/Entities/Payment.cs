using System;
using System.Collections.Generic;

namespace Data_Access.Entities;

public partial class Payment
{
    public int id { get; set; }

    public DateOnly collectionDate { get; set; }

    public decimal amount { get; set; }

    public string state { get; set; } = null!;

    public int registrer { get; set; }

    public int creditId { get; set; }

    public virtual Credit credit { get; set; } = null!;

    public virtual Employee registrerNavigation { get; set; } = null!;
}
