using Data_Access;
using Data_Access.Entities;
using FinancieraServer.DataContracts;
using FinancieraServer.Interfaces;
using System.Data.Common;
using System.Net;
using System.Net.Mail;
using System.Runtime.CompilerServices;
using System.Text;

namespace FinancieraServer.ServiceImplementations
{
    public class AccountService : IAccountService
    {
        private ILogger<AccountService> _logger;
        readonly Dictionary<string, string> verificationCodes = new Dictionary<string, string>();

        public AccountService(ILogger<AccountService> logger)
        {
            _logger = logger;
        }

        public Response createAccount(EmployeeDC employee)
        {
            if (!employee.isValid())
            {
                return new Response(2, "Invalid data");
            }
            
            EmployeeDB employeeDB = new EmployeeDB();

            Employee newEmployee = new Employee()
            {
                user = employee.user,
                password = employee.password,
                role = employee.role,
                name = employee.name,
                mail = employee.mail,
                phoneNumber = employee.phone,
                address = employee.address,
                birthday = DateOnly.ParseExact(employee.birthday, "dd/MM/yyyy"),
                sucursalId = employee.subsidiaryId
            };

            try
            {
                if (employeeDB.Exists(newEmployee))
                {
                    return new Response(3, "An account with the same user or email already exists");
                }
                else if (employeeDB.Add(newEmployee) != 0)
                {
                    return new Response(1, "An error ocurred, please try again later");
                }
            }
            catch (DbException error)
            {
                _logger.LogWarning("An error ocurred while trying to create an account: ", error);
                return new Response(1, "An error ocurred, please try again later");
            }

            _logger.LogInformation($"Account for user {employee.user} created at {DateTime.Now}");
            return new Response(0, "Account created successfully");
        }

        Response IAccountService.ChangePassword(string user, string password)
        {
            EmployeeDB employeeDB = new EmployeeDB();
            Employee employeeData = new()
            {
                user = user,
                password = password
            };

            try
            {
                int dbCode = employeeDB.ChangePassword(employeeData.user, employeeData.password);
                if(dbCode == 1)
                {
                    return new Response(2, "Invalid Credentials");
                }
                else
                {
                    _logger.LogInformation($"Password changed for the account {employeeData.user} at {DateTime.Now}");
                    return new Response(0, "Password changed correctly");
                }
            }
            catch(DbException error)
            {
                _logger.LogWarning($"An Error Ocurred trying to change the password: {error.Message}");
                return new Response(1, "An error ocurred, please try again later");
            }
        }

        bool IAccountService.CheckVerificationCode(string clientCode, string user)
        {
            bool isCodeCorrect = false;
            if (verificationCodes.TryGetValue(user, out string realCode))
            {
                isCodeCorrect = realCode.Equals(clientCode, StringComparison.OrdinalIgnoreCase);
            }
            return isCodeCorrect;
        }

         string IAccountService.GenerateVerificationCode(string user)
        {
            if (verificationCodes.ContainsKey(user))
            {
                verificationCodes.Remove(user);
            }
            const string CHARS = "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            var random = new Random();
            var stringChars = new char[6];
            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = CHARS[random.Next(CHARS.Length)];
            }

            var verificationCode = new String(stringChars);
            verificationCodes.Add(user, verificationCode);
            return verificationCode;
        }

        int IAccountService.SendEmail(string mail, string code)
        {
            try
            {
                string smtpPassword = Environment.GetEnvironmentVariable("SMPT_PASSWORD");

                if (string.IsNullOrWhiteSpace(smtpPassword))
                {
                    return 2;
                }

                StringBuilder mailBody = new StringBuilder();
                mailBody.AppendFormat("<h1> Has solicitado restablecer tu contraseña</h1>");
                mailBody.AppendFormat("<br />");
                mailBody.AppendFormat("<h2> Ingresa el código de verificación para cambia tu contraseña</h2>");
                mailBody.AppendFormat("<br />");
                mailBody.AppendFormat($"<p> tu código de verificación es {code} </p>");

                MailMessage correo = new MailMessage
                {
                    From = new MailAddress("fnncrspprt@gmail.com", "FinancieraSupport"),
                    Subject = "Restablecer contraseña",
                    Body = mailBody.ToString(),
                    IsBodyHtml = true
                };
                correo.To.Add(mail);

                SmtpClient smtp = new SmtpClient("smpt.gmail.com")
                {
                    Port = 587,
                    Credentials = new System.Net.NetworkCredential("fnncrspprt@gmail.com", smtpPassword),
                    EnableSsl = true
                };

                smtp.Send(correo);
                _logger.LogInformation($"Verification code email sent successfully");

                return 0;
            }
            catch (Exception error)
            {
                _logger.LogError($"An error ocurred trying to send the email {error.Message}");
                return 2;
            }
        }
    }
}
