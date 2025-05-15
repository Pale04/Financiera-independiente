using Data_Access;
using FinancieraServer.DataContracts;
using Data_Access.Entities;
using System.Data.Common;

namespace FinancieraServer.ServiceImplementations
{
    public partial class CatalogService
    {
        public ResponseWithContent<List<CreditConditionDC>> GetCreditConditionsByPagination(int pageSize, int markId, bool next)
        {
            if (pageSize <= 0 || markId < 0)
            {
                _logger.LogInformation("Attempt of get credit conditions with {pageSize} page size and {markId} mark id", pageSize, markId);
                return new ResponseWithContent<List<CreditConditionDC>>(2, "Invalid page size or mark ID");
            }

            CreditConditionDB creditConditionDB = new();
            List<CreditCondition> databaseCreditConditions = new();

            try
            {
                databaseCreditConditions = next ? creditConditionDB.GetByPaginationNext(pageSize, markId) : creditConditionDB.GetByPaginationPrevious(pageSize, markId);
            }
            catch (DbException error)
            {
                _logger.LogWarning("Error while attempting to get CreditConditions {error}", error);
                return new ResponseWithContent<List<CreditConditionDC>>(1, "Error while attempting to get CreditConditions");
            }

            List<CreditConditionDC> creditConditions = new();
            foreach (var creditCondition in databaseCreditConditions)
            {
                creditConditions.Add(new CreditConditionDC
                {
                    Id = creditCondition.id,
                    State = creditCondition.state,
                    InterestRate = creditCondition.interestRate,
                    IVA = creditCondition.IVA,
                    PaymentsPerMonth = creditCondition.paymentsPerMonth
                });
            }

            return new ResponseWithContent<List<CreditConditionDC>>(0, creditConditions);
        }

        public ResponseWithContent<List<CreditConditionDC>> GetActiveCreditConditions()
        {
            CreditConditionDB creditConditionDB = new();
            List<CreditCondition> databaseCreditConditions = new();

            try
            {
                creditConditionDB.GetActive();
            }
            catch (DbException error)
            {
                _logger.LogWarning("Error while attempting to get CreditConditions {error}", error);
                return new ResponseWithContent<List<CreditConditionDC>>(1, "Error while attempting to get CreditConditions");
            }

            List<CreditConditionDC> creditConditions = new();
            foreach (var creditCondition in databaseCreditConditions)
            {
                creditConditions.Add(new CreditConditionDC
                {
                    Id = creditCondition.id,
                    State = creditCondition.state,
                    InterestRate = creditCondition.interestRate,
                    IVA = creditCondition.IVA,
                    PaymentsPerMonth = creditCondition.paymentsPerMonth
                });
            }

            return new ResponseWithContent<List<CreditConditionDC>>(0, creditConditions);
        }

        public Response AddCreditCondition(CreditConditionDC creditCondition)
        {
            if (creditCondition.InterestRate < 0 || creditCondition.IVA < 0 || creditCondition.PaymentsPerMonth < 1)
            {
                _logger.LogInformation("Attempt of add credit condition with invalid values");
                return new Response(2, "Invalid atributes");
            }

            CreditConditionDB creditConditionDB = new();
            CreditCondition newCreditCondition = new()
            {
                interestRate = creditCondition.InterestRate,
                IVA = creditCondition.IVA,
                paymentsPerMonth = creditCondition.PaymentsPerMonth,
                registrer = creditCondition.RegistrerId
            };

            try
            {
                if (creditConditionDB.Exits(newCreditCondition))
                {
                    _logger.LogInformation("Attempt of add credit condition with existing values");
                    return new Response(3, "Credit condition already exists");
                }
                creditConditionDB.Add(newCreditCondition);
            }
            catch (DbException error)
            {
                _logger.LogWarning("Error while attempting to add CreditCondition {error}", error);
                return new Response(1, "Error while attempting to add CreditCondition");
            }

            return new Response(0);
        }

        public Response UpdateCreditCondition(CreditConditionDC creditCondition)
        {
            if (creditCondition.Id < 1 || creditCondition.InterestRate < 0 || creditCondition.IVA < 0 || creditCondition.PaymentsPerMonth < 1)
            {
                _logger.LogInformation("Attempt of update credit condition with invalid values");
                return new Response(2, "Invalid atributes");
            }

            CreditConditionDB creditConditionDB = new();
            CreditCondition updatedCreditCondition = new()
            {
                id = creditCondition.Id,
                interestRate = creditCondition.InterestRate,
                IVA = creditCondition.IVA,
                paymentsPerMonth = creditCondition.PaymentsPerMonth
            };

            try
            {
                if (creditConditionDB.AnotherExists(updatedCreditCondition))
                {
                    _logger.LogInformation("Attempt of update credit condition with existing values");
                    return new Response(3, "Credit condition already exists");
                }
                creditConditionDB.Update(updatedCreditCondition);
            }
            catch (DbException error)
            {
                _logger.LogWarning("Error while attempting to update CreditCondition {error}", error);
                return new Response(1, "Error while attempting to update CreditCondition");
            }

            return new Response(0);
        }

        public Response UpdateCreditConditionState(int id, bool state)
        {
            if (id < 1)
            {
                _logger.LogInformation("Attempt of update credit condition with invalid id {id}", id);
                return new Response(2, "Invalid ID");
            }

            CreditConditionDB creditConditionDB = new();

            try
            {
                creditConditionDB.UpdateState(id, state);
            }
            catch (DbException error)
            {
                _logger.LogWarning("Error while attempting to update CreditCondition {error}", error);
                return new Response(1, "Error while attempting to update CreditCondition");
            }

            return new Response(0);
        }
    }
}
