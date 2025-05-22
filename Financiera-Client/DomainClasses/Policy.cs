using System;
using System.Collections.Generic;
using System.ComponentModel;
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
            if (string.IsNullOrWhiteSpace(Title) || string.IsNullOrWhiteSpace(Description))
                return false;

            var trimmedTitle = Title.Trim();
            var trimmedDescription = Description.Trim();

            return trimmedTitle.Length >= 10 && trimmedTitle.Length <= 50 &&
                   trimmedDescription.Length >= 10 && trimmedDescription.Length <= 100;
        }

    }
}
