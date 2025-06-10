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

            if (account.IsValidForLogin())
            {
                return 2;
            }

            SessionServiceClient sessionClient = new();
            AccountServiceClient accountClient = new();

            try
            {
                ResponseWithContentOfEmployeeefOWEHwa response = sessionClient.GetAccountInfo(account.User);
                if (response != null)
                {
                    EmployeeClass employeeInfo = new()
                    {
                        User = response.Data.user,
                        Mail = response.Data.mail
                    };

                    string code = accountClient.GenerateVerificationCode(response.Data.user);
                    accountClient.SendEmail(response.Data.mail, code);
                }
                else
                {
                    return 2;
                }
            }
            catch (CommunicationException error)
            {
                return 1;
            }

            return 0;
        }

        public int ChangePassword(EmployeeClass employee, string password, string confirmPassword)
        {
            if (Equals(password, confirmPassword))
            {
                if (IsPasswordValid(password))
                {
                    AccountServiceClient accountClient = new();
                    AccountServiceReference.Response response;
                    try{
                        response = accountClient.ChangePassword(employee.User, password);
                        return response.StatusCode;

                    }
                    catch(CommunicationException error)
                    {
                        return 1;
                    }
                }
                else
                {
                    return 2;
                }
            }
            else
            {
                return 2;
            }
        }

        public bool IsPasswordValid(string password)
        {
           
            if (string.IsNullOrWhiteSpace(password))
                return false;

            Regex forbiddenChars = new Regex(@"['#\-%]");
            if (forbiddenChars.IsMatch(password))
                return false;

            return true;
        }

        public bool isCodeValid(string user, string code)
        {
            AccountServiceClient client = new();
            bool valid = false;
            try
            {
                valid = client.CheckVerificationCode(code, user);
                if (!valid)
                {
                    return valid;
                }
                else
                {
                    valid = true;
                    return valid;
                }
            }
            catch (CommunicationException error)
            {
                return valid;
            }
        }

        public int CreateAccount(EmployeeClass employee)
        {
            AccountServiceClient accountClient = new();
            AccountServiceReference.Response response;

            EmployeeDC newEmployee = new()
            {
                user = employee.User,
                password = employee.Password,
                name = employee.Name,
                mail = employee.Mail,
                address = employee.Address,
                phone = employee.PhoneNumber,
                birthday = employee.Birthday.ToString(),
                role = employee.Role,
                subsidiaryId = employee.SucursalId

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
