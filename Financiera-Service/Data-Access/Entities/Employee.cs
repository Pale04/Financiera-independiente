using System;
using System.Collections.Generic;

namespace Data_Access.Entities;

public partial class Employee
{
    public int id { get; set; }

    public string user { get; set; } = null!;

    public string password { get; set; } = null!;

    public string name { get; set; } = null!;

    public string mail { get; set; } = null!;

    public string address { get; set; } = null!;

    public string phoneNumber { get; set; } = null!;

    public DateOnly birthday { get; set; }

    public string role { get; set; } = null!;

    public int sucursalId { get; set; }

    public virtual ICollection<CreditCondition> CreditConditions { get; set; } = new List<CreditCondition>();

    public virtual ICollection<CreditPolicy> CreditPolicies { get; set; } = new List<CreditPolicy>();

    public virtual ICollection<Credit> Credits { get; set; } = new List<Credit>();

    public virtual ICollection<Document> Documents { get; set; } = new List<Document>();

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual Subsidiary sucursal { get; set; } = null!;
}
