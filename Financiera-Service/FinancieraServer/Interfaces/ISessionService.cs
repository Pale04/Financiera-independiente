using FinancieraServer.DataContracts;

namespace FinancieraServer.Interfaces
{
    [ServiceContract]
    public interface ISessionService
    {
        [OperationContract]
        ResponseWithContent<string> Login(String username, String password);
    }
}
