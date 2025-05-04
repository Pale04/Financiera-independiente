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
                subsidiaries = context.Subsidiary
                    .OrderBy(d => d.id)
                    .Where(d => d.id > lastId)
                    .ToList();
            }
            return subsidiaries;
        }

        public bool AnotherExists(Subsidiary subsidiary)
        {
            using (var context = new independent_financialContext(ConnectionStringGenerator.GetConnectionString(ConnectionRole.Reader)))
            {
              // todo: make addresess to lowercase
                return context.Subsidiary.Any(d => d.Address == subsidiary.Address && d.id != subsidiary.id);
            }
        }

        public int Add(string address)
        {
            int result = 0;
            using (var context = new independent_financialContext(ConnectionStringGenerator.GetConnectionString(ConnectionRole.Administrator)))
            {
                var newSubsidiary = new Subsidiary
                {
                    Address = address,
                    state = true,
                };

                context.Subsidiary.Add(newSubsidiary);
                result = context.SaveChanges();
            }
            return result;
        }

        public int Update(int id, string address)
        {
            int result = 0;
            using (var context = new independent_financialContext(ConnectionStringGenerator.GetConnectionString(ConnectionRole.Administrator)))
            {
                var subsidiary = context.Subsidiary.Find(id);
                if (subsidiary != null)
                {
                    subsidiary.Address = address;
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
                var subsidiary = context.Subsidiary.Find(id);
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
