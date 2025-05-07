using Data_Access;
using FinancieraServer.DataContracts;
using FinancieraServer.Interfaces;
using Data_Access.Entities;
using System.Data.Common;

namespace FinancieraServer.ServiceImplementations
{
    public class CustomerService : ICustomerService
    {
        private ILogger<CustomerService> _logger;

        public CustomerService(ILogger<CustomerService> logger)
        {
            _logger = logger;
        }

        public Response AddCustomer(CustomerDC customer)
        {
            if(!customer.IsvalidForCreation())
            {
                _logger.LogInformation("Attempt of add customer with invalid values");
                return new Response(2, "Invalid atributes");
            }

            ClientDB clientDB = new();
            try
            {
                if (clientDB.Exists(customer.Rfc))
                {
                    _logger.LogInformation("Attempt of add customer with existing rfc");
                    return new Response(3, "Customer with this rfc already exists");
                }
            }
            catch (Exception error)
            {
                _logger.LogWarning("Error while attempting to add Client {error}", error);
                return new Response(1, "Error while attempting to add Client");
            }
            
            Client newClient = new()
            {
                rfc = customer.Rfc,
                name = customer.Name,
                birthday = DateOnly.Parse(customer.Birthdate),
                houseAddress = customer.HouseAdress,
                workAddress = customer.WorkAddress,
                phoneNumber1 = customer.PhoneNumber1,
                phoneNumber2 = customer.PhoneNumber2,
                mail = customer.Mail
            };
            BankAccount[] bankAccounts = customer.BankAccounts.Select(b => new BankAccount
            {
                clabe = b.Clabe,
                purpose = b.Purpose == BankAccountType.Receive ? "receive" : "collect",
                clientId = customer.Rfc
            }).ToArray();
            PersonalReference[] personalReferences = customer.PersonalReferences.Select(p => new PersonalReference
            {
                name = p.Name,
                phoneNumber = p.PhoneNumber,
                relationship = p.Relationship,
                clientRfc = customer.Rfc
            }).ToArray();

            BankAccountDB bankAccountDB = new();
            PersonalReferenceDB personalReferenceDB = new();
            try
            {
                clientDB.Add(newClient);
                foreach (BankAccount bankAccount in bankAccounts)
                {
                    bankAccountDB.Add(bankAccount);
                }
                foreach (PersonalReference personalReference in personalReferences)
                {
                    personalReferenceDB.Add(personalReference);
                }
            }
            catch (DbException error)
            {
                _logger.LogWarning("Error while attempting to add Client {error}", error);
                return new Response(1, "Error while attempting to add Client");
            }

            return new Response(0);
        }

        public ResponseWithContent<CustomerDC> GetCustomerByRfc(string rfc)
        {
            if (!string.IsNullOrEmpty(rfc))
            {
                _logger.LogInformation("Attempt of get customer with invalid rfc");
                return new ResponseWithContent<CustomerDC>(2, "Invalid rfc");
            }

            ClientDB clientDB = new();
            Client databaseClient = new();

            try
            {
                if (!clientDB.Exists(rfc))
                {
                    return new ResponseWithContent<CustomerDC>(4, "Customer with this rfc does not exist");
                }
                databaseClient = clientDB.GetByRfc(rfc);
            }
            catch (DbException error)
            {
                _logger.LogWarning("Error while attempting to get Client {error}", error);
                return new ResponseWithContent<CustomerDC>(1, "Error while attempting to get Client");
            }

            return new ResponseWithContent<CustomerDC>(0, new CustomerDC(databaseClient));
        }

        public ResponseWithContent<List<CustomerDC>> GetCustomersByPagination(int pageSize, string markRfc, bool next)
        {
            if (pageSize <= 0 || string.IsNullOrWhiteSpace(markRfc))
            {
                _logger.LogInformation("Attempt of get customers with {pageSize} page size and {markId} mark id", pageSize, markRfc);
                return new ResponseWithContent<List<CustomerDC>>(2, "Invalid page size or mark ID");
            }

            ClientDB clientDB = new();
            List<Client> databaseClients = new();

            try
            {
                databaseClients = next ? clientDB.GetByPaginationNext(pageSize, markRfc) : clientDB.GetByPaginationPrevious(pageSize, markRfc);
            }
            catch (DbException error)
            {
                _logger.LogWarning("Error while attempting to get Clients {error}", error);
                return new ResponseWithContent<List<CustomerDC>>(1, "Error while attempting to get Clients");
            }

            List<CustomerDC> customers = new();
            foreach (Client client in databaseClients)
            {
                customers.Add(new CustomerDC(client));
            }
            
            return new ResponseWithContent<List<CustomerDC>>(0, customers);
        }

        public Response UpdateCustomerBankAccount(BankAccountDC bankAccount)
        {
            if (bankAccount.Id < 1 || string.IsNullOrWhiteSpace(bankAccount.Clabe))
            {
                _logger.LogInformation("Attempt of update customer bank account with invalid values");
                return new Response(2, "Invalid atributes");
            }

            BankAccountDB bankAccountDB = new();
            BankAccount updatedBankAccount = new()
            {
                id = bankAccount.Id,
                clabe = bankAccount.Clabe,
                purpose = bankAccount.Purpose == BankAccountType.Receive ? "receive" : "collect"
            };

            try
            {
                bankAccountDB.Update(updatedBankAccount);
            }
            catch (DbException error)
            {
                _logger.LogWarning("Error while attempting to update BankAccount {error}", error);
                return new Response(1, "Error while attempting to update BankAccount");
            }

            return new Response(0);
        }

        public Response UpdateCustomerPersonalInformation(CustomerDC customer)
        {
            if (!customer.IsValidForUpdate())
            {
                _logger.LogInformation("Attempt of update customer with invalid values");
                return new Response(2, "Invalid atributes");
            }

            ClientDB clientDB = new();
            Client updatedClient = new()
            {
                rfc = customer.Rfc,
                name = customer.Name,
                birthday = DateOnly.Parse(customer.Birthdate),
                houseAddress = customer.HouseAdress,
                workAddress = customer.WorkAddress,
                phoneNumber1 = customer.PhoneNumber1,
                phoneNumber2 = customer.PhoneNumber2,
                mail = customer.Mail
            };

            try
            {
                clientDB.UpdatePersonalInformation(updatedClient);
            }
            catch (DbException error)
            {
                _logger.LogWarning("Error while attempting to update Client {error}", error);
                return new Response(1, "Error while attempting to update Client");
            }

            return new Response(0);
        }

        public Response UpdateCustomerPersonalReference(PersonalReferenceDC personalReference)
        {
            if (!personalReference.IsValidForUpdate())
            {
                _logger.LogInformation("Attempt of update customer personal reference with invalid values");
                return new Response(2, "Invalid atributes");
            }

            PersonalReferenceDB personalReferenceDB = new();
            PersonalReference updatedPersonalReference = new()
            {
                id = personalReference.Id,
                name = personalReference.Name,
                phoneNumber = personalReference.PhoneNumber,
                relationship = personalReference.Relationship
            };

            try
            {
                personalReferenceDB.Update(updatedPersonalReference);
            }
            catch (DbException error)
            {
                _logger.LogWarning("Error while attempting to update PersonalReference {error}", error);
                return new Response(1, "Error while attempting to update PersonalReference");
            }

            return new Response(0);
        }

        public Response UpdateCustomerState(string rfc, bool state)
        {
            if (string.IsNullOrEmpty(rfc))
            {
                _logger.LogInformation("Attempt of update customer state with invalid rfc");
                return new Response(2, "Invalid rfc");
            }

            ClientDB clientDB = new();
            int result;
            try
            {
                result = clientDB.UpdateState(rfc, state);
            }
            catch (DbException error)
            {
                _logger.LogWarning("Error while attempting to update Client {error}", error);
                return new Response(1, "Error while attempting to update Client");
            }

            if (result == -1)
            {
                return new Response(5, "Cannot deactivate client, because has an active credit");
            }

            return new Response(0);
        }
    }
}
