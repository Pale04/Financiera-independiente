using System;
using System.Collections.Generic;

namespace Data_AccessTests.Entities;

public partial class BankAccount
{
    public int id { get; set; }

    public string clabe { get; set; } = null!;

    public string purpose { get; set; } = null!;

    public string clientId { get; set; } = null!;

    public virtual Client client { get; set; } = null!;
}
