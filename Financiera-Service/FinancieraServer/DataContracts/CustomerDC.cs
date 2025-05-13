using Data_Access.Entities;

namespace FinancieraServer.DataContracts
{
    [DataContract]
    public class CustomerDC
    {
        public CustomerDC() { }

        public CustomerDC(Client entity)
        {
            Rfc = entity.rfc;
            Name = entity.name;
            Birthdate = entity.birthday.ToString("yyyy-MM-dd");
            HouseAdress = entity.houseAddress;
            WorkAddress = entity.workAddress;
            PhoneNumber1 = entity.phoneNumber1;
            PhoneNumber2 = entity.phoneNumber2;
            Mail = entity.mail;
            State = entity.state;
            BankAccounts = entity.BankAccounts.Select(b => new BankAccountDC
            {
                Id = b.id,
                Clabe = b.clabe,
                Purpose = b.purpose.Equals("receive") ? BankAccountType.Receive : BankAccountType.Collect,
                CustomerRfc = b.clientId,
            }).ToArray();
            PersonalReferences = entity.PersonalReferences.Select(p => new PersonalReferenceDC
            {
                Id = p.id,
                Name = p.name,
                PhoneNumber = p.phoneNumber,
                Relationship = p.relationship,
                CustomerRfc = p.clientRfc
            }).ToArray();
        }

        [DataMember]
        public string Rfc { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public String Birthdate { get; set; }
        [DataMember]
        public string HouseAdress { get; set; }
        [DataMember]
        public string WorkAddress { get; set; }
        [DataMember]
        public string PhoneNumber1 { get; set; }
        [DataMember]
        public string PhoneNumber2 { get; set; }
        [DataMember]
        public string Mail { get; set; }
        [DataMember]
        public bool State { get; set; }
        [DataMember]
        public BankAccountDC[] BankAccounts { get; set; }
        [DataMember]
        public PersonalReferenceDC[] PersonalReferences { get; set; }

        public bool IsvalidForCreation()
        {
            return !string.IsNullOrWhiteSpace(Rfc)
                && !string.IsNullOrWhiteSpace(Name)
                && !string.IsNullOrWhiteSpace(Birthdate)
                && !string.IsNullOrWhiteSpace(HouseAdress)
                && !string.IsNullOrWhiteSpace(WorkAddress)
                && !string.IsNullOrWhiteSpace(PhoneNumber1)
                && !string.IsNullOrWhiteSpace(PhoneNumber2)
                && !string.IsNullOrWhiteSpace(Mail)
                && BankAccounts != null && BankAccounts.Length == 2
                && PersonalReferences != null && PersonalReferences.Length == 2;
        }

        public bool IsValidForUpdate()
        {
            return !string.IsNullOrWhiteSpace(Rfc)
                && !string.IsNullOrWhiteSpace(Name)
                && !string.IsNullOrWhiteSpace(Birthdate)
                && !string.IsNullOrWhiteSpace(HouseAdress)
                && !string.IsNullOrWhiteSpace(WorkAddress)
                && !string.IsNullOrWhiteSpace(PhoneNumber1)
                && !string.IsNullOrWhiteSpace(PhoneNumber2)
                && !string.IsNullOrWhiteSpace(Mail);
        }
    }
}
