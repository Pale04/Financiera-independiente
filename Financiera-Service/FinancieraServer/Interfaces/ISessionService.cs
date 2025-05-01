using Data_Access.Entities;
using FinancieraServer.DataContracts;

namespace FinancieraServer.Interfaces
{
    [ServiceContract]
    public interface ISessionService
    {
        [OperationContract]
        ResponseWithContent<Employee> Login(String username, String password);

        [OperationContract]
        ResponseWithContent<Employee> GetAccountInfo(String username);
    }
}
