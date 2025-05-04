using FinancieraServer.DataContracts;

namespace FinancieraServer.Interfaces
{
    [ServiceContract]
    public interface IAccountService
    {
        [OperationContract]
        Response createAccount();

        [OperationContract]
        void SendEmail(string mail, string code);

        [OperationContract]
        Response ChangePassword(string user, String password);

        [OperationContract]
        string GenerateVerificationCode(string user);

        [OperationContract]
        bool CheckVerificationCode(string clientCode, string user);

        
    }
}