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
    }
}
