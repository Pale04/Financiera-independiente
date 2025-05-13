using FinancieraServer.DataContracts;

namespace FinancieraServer.Interfaces
{
    [ServiceContract]
    public interface ICustomerService
    {
        [OperationContract]
        ResponseWithContent<List<CustomerDC>> GetCustomersByPagination(int pageSize, string markRfc, bool next);
        [OperationContract]
        ResponseWithContent<CustomerDC> GetCustomerByRfc(string rfc);
        [OperationContract]
        Response AddCustomer(CustomerDC customer);
        [OperationContract]
        Response UpdateCustomerPersonalInformation(CustomerDC customer);
        [OperationContract]
        Response UpdateCustomerBankAccount(BankAccountDC bankAccount);
        [OperationContract]
        Response UpdateCustomerPersonalReference(PersonalReferenceDC personalReference);
        [OperationContract]
        Response UpdateCustomerState(string rfc, bool state);
    }
}
