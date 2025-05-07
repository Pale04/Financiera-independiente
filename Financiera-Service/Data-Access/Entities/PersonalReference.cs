using System;
using System.Collections.Generic;

namespace Data_Access.Entities;

public partial class PersonalReference
{
    public int id { get; set; }

    public string name { get; set; } = null!;

    public string phoneNumber { get; set; } = null!;

    public string relationship { get; set; } = null!;

    public string clientRfc { get; set; } = null!;

    public virtual Client clientRfcNavigation { get; set; } = null!;
}
