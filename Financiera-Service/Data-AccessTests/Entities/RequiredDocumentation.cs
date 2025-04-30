using System;
using System.Collections.Generic;

namespace Data_AccessTests.Entities;

public partial class RequiredDocumentation
{
    public int id { get; set; }

    public string name { get; set; } = null!;

    public bool state { get; set; }

    public string fileType { get; set; } = null!;

    public virtual ICollection<Document> Documents { get; set; } = new List<Document>();
}
