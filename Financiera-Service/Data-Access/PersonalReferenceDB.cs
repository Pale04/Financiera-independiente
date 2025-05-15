using Data_Access.Entities;

namespace Data_Access
{
    public class PersonalReferenceDB
    {
        public int Add(PersonalReference personalReference)
        {
            int result = 0;
            using (var context = new independent_financialContext(ConnectionStringGenerator.GetConnectionString(ConnectionRole.LoanOfficer)))
            {
                context.PersonalReferences.Add(personalReference);
                context.SaveChanges();
            }
            return result;
        }

        public int Update(PersonalReference personalReference)
        {
            int result = 0;
            using (var context = new independent_financialContext(ConnectionStringGenerator.GetConnectionString(ConnectionRole.LoanOfficer)))
            {
                var existingPersonalReference = context.PersonalReferences.Find(personalReference.id);
                if (existingPersonalReference != null)
                {
                    existingPersonalReference.name = personalReference.name;
                    existingPersonalReference.phoneNumber = personalReference.phoneNumber;
                    existingPersonalReference.relationship = personalReference.relationship;
                    result = context.SaveChanges();
                }
            }
            return result;
        }
    }
}
