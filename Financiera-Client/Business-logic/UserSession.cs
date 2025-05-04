using DomainClasses;

namespace Business_logic
{
    public class UserSession
    {
        private static UserSession _instance;
        public EmployeeClass Employee { get; set; }

        private UserSession() { }

        public static UserSession Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new UserSession();
                }
                return _instance;
            }
        }

    }
}
