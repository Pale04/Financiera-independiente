using FinancieraServer.DataContracts;
using FinancieraServer.Interfaces;

namespace FinancieraServer.ServiceImplementations
{
    public class SessionService : ISessionService
    {
        readonly List<String> ACTIVE_SESSIONS = new List<String>();
        public ResponseWithContent<string> Login(String username, String password)
        {
            var Session = new ResponseWithContent<string>();

            //TODO: Get the object user to compare
            if (false || 1 == 1)
            {
                if (ACTIVE_SESSIONS.Contains(username))
                {
                    Session.StatusCode = 1;
                    Session.Message = "Ya existe una sesión activa con esta cuenta";
                }
                else
                {
                    Session.StatusCode = 0;
                    //TODO: Send the assigned role for the account
                }
            }
            else
            {
                Session.StatusCode = 1;
                Session.Message = "Usuario o contraseña incorrectos";
            }

            

                return Session;
        }
    }
}
