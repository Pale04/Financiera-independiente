using System;
using System.Collections.Generic;

namespace Data_Access.Entities;

public partial class CreditRequest
{
    public int id { get; set; }

    public string name { get; set; } = null!;

    public int capital { get; set; }

    public byte duration { get; set; }

    public int interestRate { get; set; }
}
