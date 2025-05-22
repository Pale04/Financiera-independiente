    using Data_Access.Entities;
using Data_Access;
using FinancieraServer.DataContracts;
using FinancieraServer.Interfaces;
using System.Data.Common;

namespace FinancieraServer.ServiceImplementations
{
    public  class SessionService : ISessionService
    {
        private ILogger<SessionService> _logger;
        readonly List<string> ACTIVE_SESSIONS = new List<string>();

        public SessionService(ILogger<SessionService> logger)
        {
            _logger = logger;
        }

        public ResponseWithContent<Employee> Login(String username, String password)
        {
            var UserLogin = new AccountDC()
            {
                username = username,
                password = password
            };

            EmployeeDB employeeDB = new EmployeeDB();
            Employee employeeLogin = new()
            {
                user = username,
                password = password
            };


            try
            {
                if (employeeDB.Exists(employeeLogin))
                {
                    if (employeeDB.IsCorrectPassword(employeeLogin))
                    {
                        if (ACTIVE_SESSIONS.Contains(username))
                        {
                            return new ResponseWithContent<Employee>(3, "There is a active session already");
                        }
                        else
                        {
                            
                            _logger.LogInformation($"Session initiated for {employeeLogin.user} at {DateTime.Now}");
                            ACTIVE_SESSIONS.Add(employeeLogin.user);
                            return new ResponseWithContent<Employee> (0, employeeLogin);

                            
                        }
                    }
                    else
                    {
                        return new ResponseWithContent<Employee>(2, "Incorrect username or password");
                    }
                }
                else
                {
                    return new ResponseWithContent<Employee>(4, "User do not exists");
                }
            }
            catch (DbException error)
            {
                _logger.LogWarning($"An Error Ocurred trying to get Employee Information: {error.Message}");
                return new ResponseWithContent<Employee>(1, "An error ocurred, please try again later");

            }

           
        }

        public ResponseWithContent<Employee> GetAccountInfo(string username)
        {
            EmployeeDB employeeLogin = new EmployeeDB();
            Employee employeeData = new()
            {
                user = username
            };



            try
            {
                if (employeeLogin.Exists(employeeData))
                {
                    employeeData = employeeLogin.GetEmployeeData(employeeData);
                    return new ResponseWithContent<Employee>(0, employeeData);
                }
                else
                {
                    return new ResponseWithContent<Employee>(1, "Incorrect username or password");
                }
            }
            catch (DbException error)
            {
                _logger.LogWarning($"An Error Ocurred trying to get Employee Information: {error.Message}");
                return new ResponseWithContent<Employee>(1, "An error ocurred, please try again later");
            }
        }

        Response ISessionService.Logout(string username)
        {
            
            if (ACTIVE_SESSIONS.Contains(username))
            {
                ACTIVE_SESSIONS.Remove(username);
                return new Response(0);
            }
            else
            {
                return new Response(1);
            }

        }
    }

    
}
