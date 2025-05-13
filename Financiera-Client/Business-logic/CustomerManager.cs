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

        public Customer GetByRfc(string rfc)
        {
            CustomerServiceClient client = new CustomerServiceClient();
            ResponseWithContentOfCustomerDC1nk_PiFui response;

            try
            {
                response = client.GetCustomerByRfc(rfc);
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
            else if (response.StatusCode == 4)
            {
                throw new Exception("4");
            }

            CustomerDC serializedCustomer = response.Data;
            return new Customer
            {
                Rfc = serializedCustomer.Rfc,
                Name = serializedCustomer.Name,
                BirthDate = DateOnly.Parse(serializedCustomer.Birthdate),
                HouseAddress = serializedCustomer.HouseAdress,
                WorkAddress = serializedCustomer.WorkAddress,
                PhoneNumber1 = serializedCustomer.PhoneNumber1,
                PhoneNumber2 = serializedCustomer.PhoneNumber2,
                Email = serializedCustomer.Mail,
                State = serializedCustomer.State,
                BankAccounts = [
                    new BankAccount
                    {
                        Id = serializedCustomer.BankAccounts[0].Id,
                        Clabe = serializedCustomer.BankAccounts[0].Clabe,
                        Purpose = serializedCustomer.BankAccounts[0].Purpose.Equals(BankAccountType.Receive) ? "receive" : "collect",
                        CustomerRfc = serializedCustomer.Rfc
                    },
                    new BankAccount
                    {
                        Id = serializedCustomer.BankAccounts[1].Id,
                        Clabe = serializedCustomer.BankAccounts[1].Clabe,
                        Purpose = serializedCustomer.BankAccounts[1].Purpose.Equals(BankAccountType.Receive) ? "receive" : "collect",
                        CustomerRfc = serializedCustomer.Rfc
                    }
                ],
                PersonalReferences = [
                    new PersonalReference
                    {
                        Id = serializedCustomer.PersonalReferences[0].Id,
                        Name = serializedCustomer.PersonalReferences[0].Name,
                        PhoneNumber = serializedCustomer.PersonalReferences[0].PhoneNumber,
                        Relationship = serializedCustomer.PersonalReferences[0].Relationship,
                        CustomerRfc = serializedCustomer.Rfc
                    },
                    new PersonalReference
                    {
                        Id = serializedCustomer.PersonalReferences[1].Id,
                        Name = serializedCustomer.PersonalReferences[1].Name,
                        PhoneNumber = serializedCustomer.PersonalReferences[1].PhoneNumber,
                        Relationship = serializedCustomer.PersonalReferences[1].Relationship,
                        CustomerRfc = serializedCustomer.Rfc
                    }
                ]
            };
        }

        public int AddCustomer(Customer customer)
        {
            if (!customer.IsValidForCreation())
            {
                throw new Exception(ErrorMessages.InvalidFields);
            }

            CustomerServiceClient client = new CustomerServiceClient();
            Response response;
            CustomerDC newCustomer = new()
            {
                Rfc = customer.Rfc,
                Name = customer.Name,
                Birthdate = customer.BirthDate.ToString("yyyy-MM-dd"),
                HouseAdress = customer.HouseAddress,
                WorkAddress = customer.WorkAddress,
                PhoneNumber1 = customer.PhoneNumber1,
                PhoneNumber2 = customer.PhoneNumber2,
                Mail = customer.Email,
                State = customer.State,
                BankAccounts = [
                    new BankAccountDC
                    {
                        Clabe = customer.BankAccounts[0].Clabe,
                        Purpose = customer.BankAccounts[0].Purpose.Equals("receive") ? BankAccountType.Receive : BankAccountType.Collect,
                        CustomerRfc = customer.Rfc
                    },
                    new BankAccountDC
                    {
                        Clabe = customer.BankAccounts[1].Clabe,
                        Purpose = customer.BankAccounts[1].Purpose.Equals("receive") ? BankAccountType.Receive : BankAccountType.Collect,
                        CustomerRfc = customer.Rfc
                    }
                ],
                PersonalReferences = [
                    new PersonalReferenceDC
                    {
                        Name = customer.PersonalReferences[0].Name,
                        PhoneNumber = customer.PersonalReferences[0].PhoneNumber,
                        Relationship = customer.PersonalReferences[0].Relationship,
                        CustomerRfc = customer.Rfc
                    },
                    new PersonalReferenceDC
                    {
                        Name = customer.PersonalReferences[1].Name,
                        PhoneNumber = customer.PersonalReferences[1].PhoneNumber,
                        Relationship = customer.PersonalReferences[1].Relationship,
                        CustomerRfc = customer.Rfc
                    }
                ]
            };

            try
            {
                response = client.AddCustomer(newCustomer);
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
                    throw new Exception(ErrorMessages.DuplicatedCustomer);
                default:
                    return 0;
            }
        }
   
        public int UpdateCustomerPersonalInformation(Customer customer)
        {
            if (!customer.IsValidForUpdate())
            {
                throw new Exception(ErrorMessages.InvalidFields);
            }
            CustomerServiceClient client = new CustomerServiceClient();
            Response response;
            CustomerDC updatedCustomer = new()
            {
                Rfc = customer.Rfc,
                Name = customer.Name,
                Birthdate = customer.BirthDate.ToString("yyyy-MM-dd"),
                HouseAdress = customer.HouseAddress,
                WorkAddress = customer.WorkAddress,
                PhoneNumber1 = customer.PhoneNumber1,
                PhoneNumber2 = customer.PhoneNumber2,
                Mail = customer.Email,
                State = customer.State
            };

            try
            {
                response = client.UpdateCustomerPersonalInformation(updatedCustomer);
            }
            catch (CommunicationException error)
            {
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
        
        public int UpdateCustomerBankAccount(BankAccount bankAccount)
        {
            if (!bankAccount.IsValidForUpdate())
            {
                throw new Exception(ErrorMessages.InvalidFields);
            }
            CustomerServiceClient client = new CustomerServiceClient();
            Response response;
            BankAccountDC updatedBankAccount = new()
            {
                Id = bankAccount.Id,
                Clabe = bankAccount.Clabe,
                Purpose = bankAccount.Purpose.Equals("receive") ? BankAccountType.Receive : BankAccountType.Collect
            };

            try
            {
                response = client.UpdateCustomerBankAccount(updatedBankAccount);
            }
            catch (CommunicationException error)
            {
                throw new Exception(ErrorMessages.ServerError);
            }

            switch (response.StatusCode)
            {
                case 1:
                    throw new Exception(ErrorMessages.ServerError);
                case 2:
                    throw new Exception(ErrorMessages.BadRequest);
                default:
                    return 0;
            }
        }

        public int UpdateCustomerPersonalReference(PersonalReference personalReference)
        {
            if (!personalReference.IsValidForUpdate())
            {
                throw new Exception(ErrorMessages.InvalidFields);
            }

            CustomerServiceClient client = new CustomerServiceClient();
            Response response;
            PersonalReferenceDC updatedPersonalReference = new()
            {
                Id = personalReference.Id,
                Name = personalReference.Name,
                PhoneNumber = personalReference.PhoneNumber,
                Relationship = personalReference.Relationship
            };

            try
            {
                response = client.UpdateCustomerPersonalReference(updatedPersonalReference);
            }
            catch (CommunicationException error)
            {
                throw new Exception(ErrorMessages.ServerError);
            }

            switch (response.StatusCode)
            {
                case 1:
                    throw new Exception(ErrorMessages.ServerError);
                case 2:
                    throw new Exception(ErrorMessages.BadRequest);
                default:
                    return 0;
            }
        }
    
        public int UpdateCustomerState(string rfc, bool state)
        {
            CustomerServiceClient cliente = new();
            Response response;
            try
            {
                response = cliente.UpdateCustomerState(rfc, state);
            }
            catch (CommunicationException error)
            {
                throw new Exception(ErrorMessages.ServerError);
            }

            switch (response.StatusCode)
            {
                case 1:
                    throw new Exception(ErrorMessages.ServerError);
                case 2:
                    throw new Exception(ErrorMessages.BadRequest);
                case 5:
                    throw new Exception(ErrorMessages.CannotDeactivateCustomer);
                default:
                    return 0;
            }
        }
    }
}
