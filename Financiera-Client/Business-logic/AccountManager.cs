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
                return 2;
            }

            SessionServiceClient sessionClient = new();
            AccountServiceClient accountClient = new();

            try
            {
                ResponseWithContentOfEmployeeefOWEHwa response = sessionClient.GetAccountInfo(account.user);
                if (response != null)
                {
                    EmployeeClass employeeInfo = new()
                    {
                        user = response.Data.user,
                        mail = response.Data.mail
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
