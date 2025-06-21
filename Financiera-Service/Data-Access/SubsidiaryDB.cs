using Data_Access.Entities;

namespace Data_Access
{
    public class SubsidiaryDB
    {
        public List<Subsidiary> GetAll()
        {
            List<Subsidiary> subsidiaries = [];
            using (var context = new independent_financialContext(ConnectionStringGenerator.GetConnectionString(ConnectionRole.Reader)))
            {
                subsidiaries = context.Subsidiaries
                    .OrderBy(d => d.id)
                    .ToList();
            }
            return subsidiaries;
        }

        public bool AnotherExists(Subsidiary subsidiary)
        {
            using (var context = new independent_financialContext(ConnectionStringGenerator.GetConnectionString(ConnectionRole.Reader)))
            {
                return context.Subsidiaries.Any(d => d.address.ToLower() == subsidiary.address.ToLower() && d.id != subsidiary.id);
            }
        }

        public bool Exists(string address)
        {
            using (var context = new independent_financialContext(ConnectionStringGenerator.GetConnectionString(ConnectionRole.Reader)))
            {
                return context.Subsidiaries.Any(d => d.address.ToLower() == address.ToLower());
            }
        }

        public int Add(string address)
        {
            int result = 0;
            using (var context = new independent_financialContext(ConnectionStringGenerator.GetConnectionString(ConnectionRole.Administrator)))
            {
                var newSubsidiary = new Subsidiary
                {
                    address = address,
                    state = true,
                };

                context.Subsidiaries.Add(newSubsidiary);
                result = context.SaveChanges();
            }
            return result;
        }

        public int UpdateAddress(int id, string address)
        {
            int result = 0;
            using (var context = new independent_financialContext(ConnectionStringGenerator.GetConnectionString(ConnectionRole.Administrator)))
            {
                var subsidiary = context.Subsidiaries.Find(id);
                if (subsidiary != null)
                {
                    subsidiary.address = address;
                    result = context.SaveChanges();
                }
            }
            return result;
        }

        public int UpdateState(int id, bool isActive)
        {
            int result = 0;
            using (var context = new independent_financialContext(ConnectionStringGenerator.GetConnectionString(ConnectionRole.Administrator)))
            {
                var subsidiary = context.Subsidiaries.Find(id);
                if (subsidiary != null)
                {
                    subsidiary.state = isActive;
                    result = context.SaveChanges();
                }
            }
            return result;
        }
    }
}
