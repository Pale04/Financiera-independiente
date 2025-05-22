using System;
using System.Collections.Generic;

namespace Data_Access.Entities;

public partial class Document
{
    public int id { get; set; }

    public string name { get; set; } = null!;

    public bool active { get; set; }

    public DateTime registryDate { get; set; }

    public int registrer { get; set; }

    public int documentationId { get; set; }

    public int creditId { get; set; }

    public virtual Credit credit { get; set; } = null!;

    public virtual RequiredDocumentation documentation { get; set; } = null!;

    public virtual Employee registrerNavigation { get; set; } = null!;
}
