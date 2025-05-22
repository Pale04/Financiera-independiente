using System;
using System.Collections.Generic;

namespace Data_Access.Entities;

public partial class PaymentLayout
{
    public int id { get; set; }

    public string name { get; set; } = null!;

    public DateOnly collectionDate { get; set; }

    public decimal amount { get; set; }

    public string clabe { get; set; } = null!;
}
