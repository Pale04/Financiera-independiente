using AccountServiceReference;
using DomainClasses;
using SessionServiceReference;
using System.ServiceModel;
using System.Text.RegularExpressions;

namespace Business_logic
{
    public class AccountManager
    {
        public int SendEmail(EmployeeClass account)
        {

            if (account.isValidForLogin())
            {
                throw new Exception(ErrorMessages.InvalidFields);
            }

            SessionServiceClient sessionClient = new();
            AccountServiceClient accountClient = new();

            try
            {
                if (sessionClient.GetAccountInfo(account.user) != null)
                {
                    EmployeeClass employeeInfo = new()
                    {
                        user = account.user,
                        mail = account.mail
                    };

                    string code = accountClient.GenerateVerificationCode(account.user);
                    accountClient.SendEmail(account.mail, code);
                }
                else
                {

                }
            }
            catch (CommunicationException error)
            {
                throw new Exception(ErrorMessages.ServerError);
            }

            return 0;
        }

        public int ChangePassword(EmployeeClass employee, string password, string confirmPassword)
        {
            if (Equals(password, confirmPassword))
            {
                if (isPasswordValid(password))
                {
                    AccountServiceClient accountClient = new();
                    AccountServiceReference.Response response;
                    try{
                        response = accountClient.ChangePassword(employee.user, password);
                        return response.StatusCode;

                    }
                    catch(CommunicationException error)
                    {
                        throw new Exception(ErrorMessages.ServerError);
                    }
                }
                else
                {
                    throw new Exception(ErrorMessages.InvalidFields);
                }
            }
            else
            {
                throw new Exception(ErrorMessages.InvalidFields);
            }
        }

        public bool isPasswordValid(string password)
        {
            Regex regex = new Regex("[[^'#-%]$");
            if (String.IsNullOrWhiteSpace(password))
            {
                if (regex.IsMatch(password))
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return false;
            }
        }

        public int CreateAccount(EmployeeClass employee)
        {
            AccountServiceClient accountClient = new();
            AccountServiceReference.Response response;

            EmployeeDC newEmployee = new()
            {
                user = employee.user,
                password = employee.password,
                name = employee.name,
                mail = employee.mail,
                address = employee.address,
                phone = employee.phoneNumber,
                birthday = employee.birthday.ToString(),
                role = employee.role,
                subsidiaryId = employee.sucursalId

            };

            try
            {
                response = accountClient.createAccount(newEmployee);
            }
            catch (CommunicationException error)
            {
                throw new Exception(ErrorMessages.ServerError);
            }

            return response.StatusCode;
        }
    }
}
