using Business_logic.Payments;
using DomainClasses;
using Business_logic;
using Business_logic.Catalogs;
using PaymentServiceReference;

namespace Payments.Tests
{
    [TestClass()]
    public class PaymentManagerTests
    {
        private static CreditManager _creditManager;
        private static PaymentManager _paymentManager;
        private static EmployeeClass _employee;
        private static Customer _customer;
        private static CreditCondition _creditCondition;
        private static Credit _credit;
        private static Payment _payment1;
        private static Payment _payment2;
        private static Payment _payment3;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            CreateEmployee();
            CreateCreditCondition();
            CreateCustomer();
            CreateCredit();
            CreatePayments();
        }

        private static void CreateEmployee()
        {
            _employee = new()
            {
                Id = 1,
                User = "testUser",
                Password = "Abcd12345.",
                Name = "Test",
                Mail = "test@email.com",
                Address = "Test Address",
                PhoneNumber = "1234567890",
                Birthday = DateOnly.FromDateTime(DateTime.Now),
                Role = "admin",
                SucursalId = 1
            };
            AccountManager accountManager = new();
            accountManager.CreateAccount(_employee);
            UserSession.Instance.Employee = _employee;
        }

        private static void CreateCreditCondition()
        {
            _creditCondition = new()
            {
                Id = 1,
                State = true,
                InterestRate = 10,
                IVA = 16,
                PaymentsPerMonth = 4,
                RegistrerId = _employee.Id,
            };
            CreditConditionManager creditConditionManager = new();
            creditConditionManager.AddCreditCondition(_creditCondition);
        }

        private static void CreateCustomer()
        {
            CustomerManager customerManager = new();
            _customer = new()
            {
                Rfc = "ABCD220136A01",
                Name = "Juan Perez",
                BirthDate = DateOnly.FromDateTime(DateTime.Now.AddYears(-18)),
                HouseAddress = "Calle 123",
                WorkAddress = "Calle 456",
                PhoneNumber1 = "1234567890",
                PhoneNumber2 = "0987654321",
                Email = "correo@gmail.com",
                State = true
            };
            BankAccount receiveBankAccount = new()
            {
                Id = 1,
                Clabe = "123456789012345678",
                Purpose = "receive",
                CustomerRfc = _customer.Rfc
            };
            BankAccount collectBankAccount = new()
            {
                Id = 2,
                Clabe = "123456789012345678",
                Purpose = "collect",
                CustomerRfc = _customer.Rfc
            };
            PersonalReference firstPersonalReference = new()
            {
                Id = 1,
                Name = "Maria Lopez",
                PhoneNumber = "1234567890",
                Relationship = "Sister",
                CustomerRfc = _customer.Rfc
            };
            PersonalReference secondPersonalReference = new()
            {
                Id = 2,
                Name = "Maria Hernández",
                PhoneNumber = "1234567890",
                Relationship = "Mother",
                CustomerRfc = _customer.Rfc
            };
            _customer.BankAccounts = [receiveBankAccount, collectBankAccount];
            _customer.PersonalReferences = [firstPersonalReference, secondPersonalReference];
            customerManager.AddCustomer(_customer);
        }

        private static void CreateCredit()
        {
            CreditManager creditManager = new();
            _credit = new Credit()
            {
                Id = 1,
                Duration = 12,
                State = "requested",
                Capital = 10000,
                Beneficiary = _customer.Rfc,
                ConditionId = _creditCondition.Id
            };
            creditManager.Add(_credit, []);
            creditManager.DeterminateResquest(_credit, true);
        }

        private static void CreatePayments()
        {
            _paymentManager = new PaymentManager();
            _payment1 = new()
            {
                Amount = 200,
                CreditId = _credit.Id,
                CollectionDate = DateOnly.FromDateTime(DateTime.Now),
                RegistrerId = _employee.Id
            };
            _payment2 = new()
            {
                Amount = 300,
                CreditId = _credit.Id,
                CollectionDate = DateOnly.FromDateTime(DateTime.Now.AddDays(7)),
                RegistrerId = _employee.Id

            };
            _payment3 = new()
            {
                Amount = 400,
                CreditId = _credit.Id,
                CollectionDate = DateOnly.FromDateTime(DateTime.Now.AddDays(14)),
                RegistrerId = _employee.Id

            };
            _paymentManager.AddPolicy(_payment1);
            _paymentManager.AddPolicy(_payment2);
            _paymentManager.AddPolicy(_payment3);

        }

        [TestMethod()]
        public void GetPaymentLayoutSuccessfulTest()
        {
            DateOnly startDate = DateOnly.FromDateTime(DateTime.Now);
            DateOnly endDate = DateOnly.FromDateTime(DateTime.Now.AddMonths(1));
            List<PaymentLayout> paymentLayout = _paymentManager.GetPaymentLayout(startDate, endDate);
            Assert.IsTrue(paymentLayout.Count == 3);
        }
    }
}