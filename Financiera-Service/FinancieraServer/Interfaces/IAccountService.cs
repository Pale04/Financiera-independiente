using FinancieraServer.DataContracts;

namespace FinancieraServer.Interfaces
{
    [ServiceContract]
    public interface IAccountService
    {
        [OperationContract]
        Response createAccount();
    }
}
