using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;

namespace DomainClasses;

public enum FileType
{
    pdf,
    image
}

public class RequiredDocument
{
    public int Id { get; set; }
    public string Name { get; set; }
    public FileType FileType { get; set; }
    public bool Status { get; set; }

    public bool IsValidForCreation()
    {
        return !string.IsNullOrWhiteSpace(Name);
    }

    public bool IsValidForUpdate()
    {
        return Id > 0 && !string.IsNullOrWhiteSpace(Name);
    }
}
