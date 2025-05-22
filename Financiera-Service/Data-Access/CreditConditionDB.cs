using Data_Access.Entities;

namespace Data_Access
{
    public class CreditConditionDB
    {
        public List<CreditCondition> GetByPaginationNext(int pageSize, int lastId)
        {
            List<CreditCondition> creditConditions = new();
            using (var context = new independent_financialContext(ConnectionStringGenerator.GetConnectionString(ConnectionRole.Reader)))
            {
                creditConditions = context.CreditConditions
                    .OrderBy(d => d.id)
                    .Where(d => d.id > lastId)
                    .Take(pageSize)
                    .ToList();
            }
            return creditConditions;
        }

        public List<CreditCondition> GetByPaginationPrevious(int pageSize, int firstId)
        {
            List<CreditCondition> creditConditions = new();
            using (var context = new independent_financialContext(ConnectionStringGenerator.GetConnectionString(ConnectionRole.Reader)))
            {
                creditConditions = context.CreditConditions
                    .OrderBy(d => d.id)
                    .Where(d => d.id < firstId)
                    .ToList()
                    .TakeLast(pageSize)
                    .ToList();
            }
            return creditConditions;
        }

        public bool Exits(CreditCondition creditCondition)
        {
            using (var context = new independent_financialContext(ConnectionStringGenerator.GetConnectionString(ConnectionRole.Reader)))
            {
                return context.CreditConditions.Any(d => d.interestRate == creditCondition.interestRate && d.IVA == creditCondition.IVA && d.paymentsPerMonth == creditCondition.paymentsPerMonth);
            }
        }

        public bool AnotherExists(CreditCondition creditCondition)
        {
            using (var context = new independent_financialContext(ConnectionStringGenerator.GetConnectionString(ConnectionRole.Reader)))
            {
                return context.CreditConditions.Any(d => d.interestRate == creditCondition.interestRate && d.IVA == creditCondition.IVA && d.paymentsPerMonth == creditCondition.paymentsPerMonth && d.id != creditCondition.id);
            }
        }

        public int Add(CreditCondition creditCondition)
        {
            int result = 0;
            using (var context = new independent_financialContext(ConnectionStringGenerator.GetConnectionString(ConnectionRole.Administrator)))
            {
                creditCondition.state = true;
                context.CreditConditions.Add(creditCondition);
                result = context.SaveChanges();
            }
            return result;
        }

        public int Update(CreditCondition creditCondition)
        {
            int result = 0;
            using (var context = new independent_financialContext(ConnectionStringGenerator.GetConnectionString(ConnectionRole.Administrator)))
            {
                var existingCreditCondition = context.CreditConditions.Find(creditCondition.id);
                if (existingCreditCondition != null)
                {
                    existingCreditCondition.interestRate = creditCondition.interestRate;
                    existingCreditCondition.IVA = creditCondition.IVA;
                    existingCreditCondition.paymentsPerMonth = creditCondition.paymentsPerMonth;
                    result = context.SaveChanges();
                }
            }
            return result;
        }

        public bool UpdateState(int id, bool state)
        {
            int result = 0;
            using (var context = new independent_financialContext(ConnectionStringGenerator.GetConnectionString(ConnectionRole.Administrator)))
            {
                var creditCondition = context.CreditConditions.Find(id);
                if (creditCondition != null)
                {
                    creditCondition.state = state;
                    result = context.SaveChanges();
                }
            }
            return result > 0;
        }

        public List<CreditCondition> GetActive()
        {
            using (var context = new independent_financialContext(ConnectionStringGenerator.GetConnectionString(ConnectionRole.Reader)))
            {
                return context.CreditConditions
                    .OrderBy(d => d.id)
                    .Where(d => d.state == true)
                    .ToList();
            }
        }
    }
}
