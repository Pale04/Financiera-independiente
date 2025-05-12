using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainClasses
{
    public class Policy
    {
        public int Id { get; set; }

        public string Title { get; set; } = null!;

        public string Description { get; set; } = null!;

        public int Registrer { get; set; }
        
        public DateOnly ExpireDate { get; set; }

        public bool State { get; set; }

        public bool isValid()
        {
            return Title.Length > 50 && Title.Length < 10 && Description.Length > 100 && Description.Length < 10;
        }

        
    }
}
