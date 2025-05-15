using Data_Access.Entities;

namespace Data_Access
{
    public class BankAccountDB
    {
        public int Add(BankAccount bankAccount)
        {
            int result = 0;
            using (var context = new independent_financialContext(ConnectionStringGenerator.GetConnectionString(ConnectionRole.LoanOfficer)))
            {
                context.BankAccounts.Add(bankAccount);
                result = context.SaveChanges();
            }
            return result;
        }

        public int Update(BankAccount bankAccount)
        {
            int result = 0;
            using (var context = new independent_financialContext(ConnectionStringGenerator.GetConnectionString(ConnectionRole.LoanOfficer)))
            {
                var existingBankAccount = context.BankAccounts.Find(bankAccount.id);
                if (existingBankAccount != null)
                {
                    existingBankAccount.clabe = bankAccount.clabe;
                    existingBankAccount.purpose = bankAccount.purpose;
                    result = context.SaveChanges();
                }
            }
            return result;
        }
    }
}
