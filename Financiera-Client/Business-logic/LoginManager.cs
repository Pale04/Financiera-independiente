using SessionServiceReference;
using DomainClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Business_logic
{
    public class LoginManager
    {
        public int Login(EmployeeClass employee)
        {
            if (employee.isValidForLogin())
            {
                throw new Exception(ErrorMessages.InvalidFields);
            }

            SessionServiceClient client = new();
            ResponseWithContentOfEmployeeefOWEHwa response;

            try
            {
                response = client.Login(employee.user, employee.password);
            }
            catch (CommunicationException error)
            {
                throw new Exception(ErrorMessages.ServerError);
            }

            switch (response.StatusCode)
            {
                case 1:
                    throw new Exception(ErrorMessages.InvalidFields);
                case 3:
                    throw new Exception(ErrorMessages.ActiveSession);
                default:
                    return 0;
            }
        }

        public EmployeeClass getSessionInfo(string user)
        {
            SessionServiceClient client = new();
            ResponseWithContentOfEmployeeefOWEHwa response;

            try
            {
                response = client.GetAccountInfo(user);
            }
            catch(CommunicationException error)
            {
                throw new Exception(ErrorMessages.ServerError);
            }

            switch (response.StatusCode)
            {
                case 1:
                    throw new Exception(ErrorMessages.InvalidFields);
                default:
                    EmployeeClass employeeInfo = new()
                    {
                        user = response.Data.user,
                        role = response.Data.role,
                        mail = response.Data.mail,
                        id = response.Data.id,
                    };

                    return employeeInfo;
            }
        }
    }
}
