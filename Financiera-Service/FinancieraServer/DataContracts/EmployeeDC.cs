using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FinancieraServer.DataContracts
{
    [DataContract]
    public class EmployeeDC
    {
        [DataMember]
        public int id { get; set; }

        [DataMember]
        public string user { get; set; } = null!;

        [DataMember]
        public string password { get; set; } = null!;

        [DataMember]
        public string name { get; set; }

        [DataMember]
        public string mail { get; set; }

        [DataMember]
        public string phone { get; set; }

        [DataMember]
        public string address { get; set; }

        [DataMember]
        public DateOnly birthday { get; set; }

        [DataMember]
        public int subsidiaryId { get; set; }

        [DataMember]
        public string role { get; set; }

        public bool isValid()
        {
            Regex mailRgx = new Regex("/^[\\w\\-\\.]+@([\\w-]+\\.)+[\\w-]{2,}$/gm");
            Regex phoneRgx = new Regex("\\d{10}");

            DateTime currentDate = DateTime.Now;

            return string.IsNullOrEmpty(name) ||
                string.IsNullOrEmpty(mail) ||
                mailRgx.IsMatch(mail) ||
                string.IsNullOrEmpty(phone) ||
                phoneRgx.IsMatch(phone) ||
                phone.Length == 10 ||
                string.IsNullOrEmpty(address) ||
                DateTime.Compare(birthday.ToDateTime(TimeOnly.MinValue), currentDate) < 0;
        }
    }
}
