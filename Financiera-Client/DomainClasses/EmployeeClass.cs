using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainClasses
{
    public class EmployeeClass
    {
        public int Id { get; set; }

        public string User { get; set; } = null!;

        public string Password { get; set; } = null!;

        public string Name { get; set; } = null!;

        public string Mail { get; set; } = null!;

        public string Address { get; set; } = null!;

        public string PhoneNumber { get; set; } = null!;

        public DateOnly Birthday { get; set; }

        public string Role { get; set; } = null!;

        public int SucursalId { get; set; }

        public bool IsValidForLogin()
        {
            return !string.IsNullOrWhiteSpace(User) && !string.IsNullOrWhiteSpace(Password);
        }

        public bool IsValidforPasswordReset()
        {
            return !string.IsNullOrWhiteSpace(User);
        }
    }
}
