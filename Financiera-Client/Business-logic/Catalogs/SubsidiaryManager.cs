using CatalogServiceReference;
using DomainClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Business_logic.Catalogs
{
    public class SubsidiaryManager
    {

        public List<DomainClasses.Subsidiary> GetAll(){
            CatalogServiceClient client = new();
            ResponseWithContentOfArrayOfSubsidiaryDC1nk_PiFui response;

            try
            {
                response = client.GetSubsidiaries();
            }
            catch (CommunicationException error)
            {
                // log error
                throw new Exception(ErrorMessages.ServerError);
            }

            List<DomainClasses.Subsidiary> subsidiaries = new List<DomainClasses.Subsidiary>();

            foreach (SubsidiaryDC subsidiary in response.Data) 
            {
                subsidiaries.Add(new DomainClasses.Subsidiary()
                {
                    Id = subsidiary.Id,
                    Address = subsidiary.Address,
                    State = subsidiary.State
                });
            }

            return subsidiaries;
        }

        public int Add(string address)
        {
            CatalogServiceClient client = new();
            Response response;

            try
            {
                response = client.AddSubsidiary(address);
            }
            catch (CommunicationException error)
            {
                // log response.Message
                throw new Exception(ErrorMessages.ServerError);
            }

            return response.StatusCode;
        }

        public int updateAddress(int id, string address)
        {
            CatalogServiceClient client = new();
            Response response;

            try
            {
                response = client.UpdateSubsidiaryAddress(id, address);
            }
            catch (CommunicationException error)
            {
                // log response.Message
                throw new Exception(ErrorMessages.ServerError);
            }

            return response.StatusCode;
        }

        public int updateState(int id, bool state)
        {
            CatalogServiceClient client = new();
            Response response;

            try
            {
                response = client.UpdateSubsidiaryState(id, state);
            }
            catch (CommunicationException error)
            {
                // log response.Message
                throw new Exception(ErrorMessages.ServerError);
            }

            return response.StatusCode;
        }
    }
}
