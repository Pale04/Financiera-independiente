using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainClasses
{
    public class Credit
    {
        public int Id { get; set; }

        public string State { get; set; } = null!;

        public byte Duration { get; set; }

        public int Capital { get; set; }

        public string Beneficiary { get; set; } = null!;

        public int ConditionId { get; set; }
    }
}
