using AccountServiceReference;
using DomainClasses;
using SessionServiceReference;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

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
            if (isSamePassword(password, confirmPassword))
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

        public bool isSamePassword(string password, string confirmPassword)
        { 
            return String.Equals(password, confirmPassword);
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
                    throw new Exception(ErrorMessages.WrongCode);
                }
                else
                {
                    valid = true;
                    return valid;
                }
            }
            catch (CommunicationException error)
            {
                throw new Exception(ErrorMessages.ServerError);
            }
        }
    }
}
