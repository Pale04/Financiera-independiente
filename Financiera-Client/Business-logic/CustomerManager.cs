using CustomerServiceReference;
using DomainClasses;
using System.ServiceModel;

namespace Business_logic
{
    public class CustomerManager
    {
        public List<Customer> GetByPagination(int size, string markRfc, bool next)
        {
            CustomerServiceClient client = new CustomerServiceClient();
            ResponseWithContentOfArrayOfCustomerDC1nk_PiFui response;

            try
            {
                response = client.GetCustomersByPagination(size, markRfc, next);
            }
            catch (CommunicationException error)
            {
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

            List<CustomerDC> serializedCustomersList = response.Data.ToList();
            List<Customer> customers = new();
            foreach (CustomerDC customer in serializedCustomersList)
            {
                customers.Add(new Customer
                {
                    Rfc = customer.Rfc,
                    Name = customer.Name,
                    BirthDate = DateOnly.Parse(customer.Birthdate),
                    HouseAddress = customer.HouseAdress,
                    WorkAddress = customer.WorkAddress,
                    PhoneNumber1 = customer.PhoneNumber1,
                    PhoneNumber2 = customer.PhoneNumber2,
                    Email = customer.Mail,
                    State = customer.State
                });
            }

            return customers;
        }
    }
}
