using Data_Access;
using Data_Access.Entities;
using FinancieraServer.DataContracts;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FinancieraServer.ServiceImplementations
{
    public partial class CatalogService
    {
        public ResponseWithContent<List<SubsidiaryDC>> GetSubsidiaries()
        {
            SubsidiaryDB subsidiaryDB = new();
            List<Subsidiary> subsidiariesList = [];

            try
            {
                subsidiariesList = subsidiaryDB.GetAll();
            }
            catch (DbException error)
            {
                _logger.LogWarning("Error while attempting to get RequiredDocumentations {error}", error);
                return new ResponseWithContent<List<SubsidiaryDC>>(1, "Error while attempting to get subsidiaries list");
            }

            List<SubsidiaryDC> subsidiariesDC = new();
            foreach (Subsidiary subsidiary in subsidiariesList)
            {
                subsidiariesDC.Add(new SubsidiaryDC()
                {
                    Id = subsidiary.id,
                    Address = subsidiary.address,
                    State = subsidiary.state
                });
            }

            return new ResponseWithContent<List<SubsidiaryDC>>(0, subsidiariesDC);
        }

        public Response AddSubsidiary(string address)
        {
            if (string.IsNullOrWhiteSpace(address))
            {
                _logger.LogInformation("Attempt of add subsidiary with empty address");
                return new Response(2, "Invalid address");
            }

            SubsidiaryDB subsidiaryDB = new();           

            try
            {
                if (subsidiaryDB.Exists(address))
                {
                    _logger.LogInformation("Attempt of add subsidiary with existing address {address}", address);
                    return new Response(3, "Subsidiary already exists");
                }
                subsidiaryDB.Add(address);
            }
            catch (DbException error)
            {
                _logger.LogWarning("Error while attempting to add subsidiary: {error}", error);
                return new Response(1, "Error while attempting to add subsidiary");
            }

            return new Response(0);
        }

        public Response UpdateSubsidiaryAddress(int id, string address)
        {
            if (string.IsNullOrWhiteSpace(address))
            {
                _logger.LogInformation("Attempt of update address with empty name or file type from");
                return new Response(2, "Invalid address");
            }

            SubsidiaryDB subsidiaryDB = new();
            Subsidiary subsidiary = new Subsidiary()
            {
                id = id,
                address = address,
                state = true
            };

            try
            {
                if (subsidiaryDB.AnotherExists(subsidiary))
                {
                    _logger.LogInformation("Attempt of update subsidiary with existing address {address}", address);
                    return new Response(3, "Subsidiary already exists");
                }

                if (subsidiaryDB.UpdateAddress(id, address) != 0)
                {
                    _logger.LogWarning("Received id: {} is non existent", id);
                    return new Response(1, "Error updating subsidiary");
                }
            }
            catch (DbException error)
            {
                _logger.LogWarning("Error while attempting to update subsidiary address: {error}", error);
                return new Response(1, "Error updating subsidiary");
            }

            return new Response(0);
        }

        public Response UpdateSubsidiaryState(int id, bool activeSubsidiary)
        {
            SubsidiaryDB subsidiaryDB = new();

            try
            {
                if (subsidiaryDB.UpdateState(id, activeSubsidiary) != 0)
                {
                    _logger.LogWarning("Received id: {} is non existent", id);
                    return new Response(1, "Error updating subsidiary");
                }
            }
            catch (DbException error)
            {
                _logger.LogWarning("Error while attempting to update subsidiary state: {error}", error);
                return new Response(1, "Error updating  subsidiary");
            }

            return new Response(0);
        }
    }
}
