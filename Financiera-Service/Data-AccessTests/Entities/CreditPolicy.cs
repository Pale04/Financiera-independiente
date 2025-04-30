using System;
using System.Collections.Generic;

namespace Data_AccessTests.Entities;

public partial class CreditPolicy
{
    public int id { get; set; }

    public bool state { get; set; }

    public string title { get; set; } = null!;

    public string description { get; set; } = null!;

    public DateOnly effectiveDate { get; set; }

    public int registrer { get; set; }

    public virtual Employee registrerNavigation { get; set; } = null!;
}
