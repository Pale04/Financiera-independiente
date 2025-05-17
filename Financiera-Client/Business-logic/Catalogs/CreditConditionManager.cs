using CatalogServiceReference;
using DomainClasses;
using System.Drawing;
using System.ServiceModel;
using static System.Net.Mime.MediaTypeNames;

namespace Business_logic.Catalogs
{
    public class CreditConditionManager
    {
        public List<DomainClasses.CreditCondition> GetByPagination(int size, int markId, bool next)
        {
            CatalogServiceClient client = new();
            ResponseWithContentOfArrayOfCreditConditionDC1nk_PiFui response;

            try
            {
                response = client.GetCreditConditionsByPagination(size, markId, next);
            }
            catch (CommunicationException error)
            {
                //TODO: Log the error
                throw new Exception(ErrorMessages.ServerError);
            }

            if (response.StatusCode == 1)
            {
                throw new Exception(ErrorMessages.ServerError);
            }
            else if (response.StatusCode == 2)
            {
                throw new Exception(ErrorMessages.BadRequest);
            }

            List<CreditConditionDC> serializedConditionsList = response.Data.ToList();
            List<DomainClasses.CreditCondition> creditConditions = new();
            foreach (CreditConditionDC condition in serializedConditionsList)
            {
                creditConditions.Add(new DomainClasses.CreditCondition
                {
                    Id = condition.Id,
                    State = condition.State,
                    InterestRate = condition.InterestRate,
                    IVA = condition.IVA,
                    PaymentsPerMonth = condition.PaymentsPerMonth
                });
            }

            return creditConditions;
        }


        public List<DomainClasses.CreditCondition> GetActive() {
            CatalogServiceClient client = new();
            ResponseWithContentOfArrayOfCreditConditionDC1nk_PiFui response;

            try
            {
                response = client.GetActiveCreditConditions();
            }
            catch (CommunicationException error)
            {
                //TODO: Log the error
                throw new Exception(ErrorMessages.ServerError);
            }

            if (response.StatusCode == 1)
            {
                throw new Exception(ErrorMessages.ServerError);
            }
            else if (response.StatusCode == 2)
            {
                throw new Exception(ErrorMessages.BadRequest);
            }

            List<CreditConditionDC> serializedConditionsList = response.Data.ToList();
            List<DomainClasses.CreditCondition> creditConditions = new();
            foreach (CreditConditionDC condition in serializedConditionsList)
            {
                creditConditions.Add(new DomainClasses.CreditCondition
                {
                    Id = condition.Id,
                    State = condition.State,
                    InterestRate = condition.InterestRate,
                    IVA = condition.IVA,
                    PaymentsPerMonth = condition.PaymentsPerMonth
                });
            }

            return creditConditions;
        }

        public int AddCreditCondition(DomainClasses.CreditCondition creditCondition)
        {
            if (!creditCondition.IsValidForCreation())
            {
                throw new Exception(ErrorMessages.InvalidFields);
            }

            CatalogServiceClient client = new();
            Response response;

            try
            {
                response = client.AddCreditCondition(new CreditConditionDC
                {
                    InterestRate = creditCondition.InterestRate,
                    IVA = creditCondition.IVA,
                    PaymentsPerMonth = creditCondition.PaymentsPerMonth,
                    RegistrerId = creditCondition.RegistrerId,
                });
            }
            catch (CommunicationException error)
            {
                //  TODO: Log the error
                throw new Exception(ErrorMessages.ServerError);
            }

            switch (response.StatusCode)
            {
                case 1:
                    //TODO: Log the error
                    throw new Exception(ErrorMessages.ServerError);
                case 2:
                    //TODO: Log the error
                    throw new Exception(ErrorMessages.BadRequest);
                case 3:
                    throw new Exception(ErrorMessages.DuplicatedCreditCondition);
                default:
                    return 0;
            }
        }

        public int UpdateCreditCondition(DomainClasses.CreditCondition creditCondition)
        {
            if (!creditCondition.IsValidForUpdate())
            {
                throw new Exception(ErrorMessages.InvalidFields);
            }

            CatalogServiceClient client = new();
            Response response;

            try
            {
                response = client.UpdateCreditCondition(new CreditConditionDC
                {
                    Id = creditCondition.Id,
                    InterestRate = creditCondition.InterestRate,
                    IVA = creditCondition.IVA,
                    PaymentsPerMonth = creditCondition.PaymentsPerMonth
                });
            }
            catch (CommunicationException error)
            {
                //TODO: Log the error
                throw new Exception(ErrorMessages.ServerError);
            }

            switch (response.StatusCode)
            {
                case 1:
                    //TODO: Log the error
                    throw new Exception(ErrorMessages.ServerError);
                case 2:
                    //TODO: Log the error
                    throw new Exception(ErrorMessages.BadRequest);
                case 3:
                    throw new Exception(ErrorMessages.DuplicatedCreditCondition);
                default:
                    return 0;
            }
        }

        public int UpdateCreditConditionState(int id, bool state)
        {
            CatalogServiceClient client = new();
            Response response;

            try
            {
                response = client.UpdateCreditConditionState(id, state);
            }
            catch (CommunicationException error)
            {
                //TODO: Log the error
                throw new Exception(ErrorMessages.ServerError);
            }

            switch (response.StatusCode)
            {
                case 1:
                    //TODO: Log the error
                    throw new Exception(ErrorMessages.ServerError);
                case 2:
                    //TODO: Log the error
                    throw new Exception(ErrorMessages.BadRequest);
                default:
                    return 0;
            }
            
        }
    }
}
