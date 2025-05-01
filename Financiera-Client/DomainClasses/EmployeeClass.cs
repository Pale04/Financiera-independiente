using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainClasses
{
    public class EmployeeClass
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

        public bool isValidForLogin()
        {
            return !string.IsNullOrWhiteSpace(user) && !string.IsNullOrWhiteSpace(password);
        }

        public bool isValidforPasswordReset()
        {
            return !string.IsNullOrWhiteSpace(user);
        }
    }
}
