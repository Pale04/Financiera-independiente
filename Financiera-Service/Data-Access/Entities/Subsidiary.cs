using System;
using System.Collections.Generic;

namespace Data_Access.Entities;

public partial class Subsidiary
{
    public int id { get; set; }

    public string address { get; set; } = null!;

    public bool state { get; set; }

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
}
