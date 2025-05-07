using Data_Access.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data_Access
{
    public class ClientDB
    {
        public List<Client> GetByPaginationNext(int pageSize, string rfc)
        {
            List<Client> clients = new();
            using (var context = new independent_financialContext(ConnectionStringGenerator.GetConnectionString(ConnectionRole.Reader)))
            {
                clients = context.Clients
                    .OrderBy(d => d.rfc)
                    .Where(d => string.Compare(d.rfc, rfc) > 0)
                    .Take(pageSize)
                    .ToList();
            }
            return clients;
        }

        public List<Client> GetByPaginationPrevious(int pageSize, string rfc)
        {
            List<Client> clients = new();
            using (var context = new independent_financialContext(ConnectionStringGenerator.GetConnectionString(ConnectionRole.Reader)))
            {
                clients = context.Clients
                    .OrderBy(d => d.rfc)
                    .Where(d => string.Compare(d.rfc, rfc) < 0)
                    .ToList()
                    .TakeLast(pageSize)
                    .ToList();
            }
            return clients;
        }

        public Client GetByRfc(string rfc)
        {
            Client client = new();
            using (var context = new independent_financialContext(ConnectionStringGenerator.GetConnectionString(ConnectionRole.Reader)))
            {
                var existingClient = context.Clients.Where(c => c.rfc == rfc).Include(c => c.PersonalReferences).Include(c => c.PersonalReferences).FirstOrDefault();
                if (existingClient != null)
                {
                    client = existingClient;
                }
            }
            return client;
        }

        public bool Exists(string rfc)
        {
            using (var context = new independent_financialContext(ConnectionStringGenerator.GetConnectionString(ConnectionRole.Reader)))
            {
                return context.Clients.Any(d => d.rfc == rfc);
            }
        }

        public int Add(Client client)
        {
            int result = 0;
            using (var context = new independent_financialContext(ConnectionStringGenerator.GetConnectionString(ConnectionRole.LoanOfficer)))
            {
                client.state = true;
                context.Clients.Add(client);
                result = context.SaveChanges();
            }
            return result;
        }

        public int UpdatePersonalInformation(Client client)
        {
            int result = 0;
            using (var context = new independent_financialContext(ConnectionStringGenerator.GetConnectionString(ConnectionRole.LoanOfficer)))
            {
                var existingClient = context.Clients.Find(client.rfc);
                if (existingClient != null)
                {
                    existingClient.name = client.name;
                    existingClient.birthday = client.birthday;
                    existingClient.houseAddress = client.houseAddress;
                    existingClient.workAddress = client.workAddress;
                    existingClient.phoneNumber1 = client.phoneNumber1;
                    existingClient.phoneNumber2 = client.phoneNumber2;
                    existingClient.mail = client.mail;
                    result = context.SaveChanges();
                }
            }
            return result;
        }

        // <summary>
        // Updates the state of a client in the database.
        // </summary>
        // <param name="rfc">The RFC of the client.</param>
        // <param name="state">The new state of the client.</param>
        // <returns>0 if the client does not exist, -1 if cannot be deactivated, 1 if the update was success</returns>
        public int UpdateState(string rfc, bool state)
        {
            int result = 0;
            using (var context = new independent_financialContext(ConnectionStringGenerator.GetConnectionString(ConnectionRole.LoanOfficer)))
            {
                var existingClient = context.Clients.Find(rfc);
                if (state && existingClient != null)
                {
                    existingClient.state = state;
                    result = context.SaveChanges();
                }
                else if (existingClient != null)
                {
                    using (var command = context.Database.GetDbConnection().CreateCommand())
                    {
                        command.CommandText = "DeactivateClient";
                        command.CommandType = System.Data.CommandType.StoredProcedure;

                        var rfcParameter = command.CreateParameter();
                        rfcParameter.ParameterName = "@rfc";
                        rfcParameter.Value = rfc;
                        command.Parameters.Add(rfcParameter);

                        var executionResult = command.ExecuteScalar();
                        result = Convert.ToInt32(executionResult);
                    }
                }
            }
            return result;
        }
    }
}
