using System;
using System.Collections.Generic;

namespace Data_AccessTests.Entities;

public partial class Client
{
    public string rfc { get; set; } = null!;

    public string name { get; set; } = null!;

    public DateOnly birthday { get; set; }

    public string houseAddress { get; set; } = null!;

    public string workAddress { get; set; } = null!;

    public string phoneNumber1 { get; set; } = null!;

    public string phoneNumber2 { get; set; } = null!;

    public string mail { get; set; } = null!;

    public bool state { get; set; }

    public virtual ICollection<BankAccount> BankAccounts { get; set; } = new List<BankAccount>();

    public virtual ICollection<Credit> Credits { get; set; } = new List<Credit>();
}
