﻿using Business_logic.Payments;
using DomainClasses;
using Business_logic;
using Business_logic.Catalogs;

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
                Id = 1,
                Amount = 200,
                CreditId = _credit.Id,
                CollectionDate = DateOnly.FromDateTime(DateTime.Now),
                RegistrerId = _employee.Id
            };
            _payment2 = new()
            {
                Id = 2,
                Amount = 300,
                CreditId = _credit.Id,
                CollectionDate = DateOnly.FromDateTime(DateTime.Now.AddDays(7)),
                RegistrerId = _employee.Id
            };
            _payment3 = new()
            {
                Id = 3,
                Amount = 400,
                CreditId = _credit.Id,
                CollectionDate = DateOnly.FromDateTime(DateTime.Now.AddDays(14)),
                RegistrerId = _employee.Id
            };
            _paymentManager.AddPayment(_payment1);
            _paymentManager.AddPayment(_payment2);
            _paymentManager.AddPayment(_payment3);

        }

        [TestMethod()]
        public void GetPaymentLayoutSuccessfulTest()
        {
            DateOnly startDate = DateOnly.FromDateTime(DateTime.Now);
            DateOnly endDate = DateOnly.FromDateTime(DateTime.Now.AddMonths(1));
            List<PaymentLayout> paymentLayout = _paymentManager.GetPaymentLayout(startDate, endDate);
            Assert.IsTrue(paymentLayout.Count == 3);
        }

        [TestMethod()]
        public void GetPaymentsFromCsvSuccesfulTest()
        {
            List<Payment> result = _paymentManager.GetPaymentsFromCsv(Directory.GetCurrentDirectory() + "\\Payments\\TestPaymentLayouts\\TestLayout_1.csv");
            Assert.IsTrue(result.Count == 3);
        }

        [TestMethod()]
        public void GetPaymentsFromCsvIncorrectFormatTest()
        {
            Assert.ThrowsException<Exception>(() =>
            {
                _paymentManager.GetPaymentsFromCsv(Directory.GetCurrentDirectory() + "\\Payments\\TestPaymentLayouts\\TestLayout_2.csv");
            });
        }

        [TestMethod()]
        public void UpdatePaymentSatateSuccessfulTest()
        {
            _payment1.State = PaymentStatus.Collected;
            int result = _paymentManager.UpdatePaymentsState(_payment1);
            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public void AddPaymentSuccessfulTest()
        {
            Payment newPayment = new()
            {
                Amount = 500,
                CreditId = _credit.Id,
                CollectionDate = DateOnly.FromDateTime(DateTime.Today.AddDays(1)),
                RegistrerId = _employee.Id
            };

            int result = _paymentManager.AddPayment(newPayment);

            Assert.AreEqual(0, result, "The payment should be added successfully and return status code 0.");
        }

        [TestMethod]
        public void AddPaymentInvalidFieldsTest()
        {
            Payment invalidPayment = new()
            {
                Amount = -100,
                CreditId = _credit.Id,
                CollectionDate = DateOnly.FromDateTime(DateTime.Today.AddDays(1)),
                RegistrerId = _employee.Id
            };

            Assert.ThrowsException<Exception>(() =>
            {
                _paymentManager.AddPayment(invalidPayment);
            }, "An exception should be thrown when trying to add a payment with invalid fields.");
        }

        [TestMethod]
        public void AddPaymentEmptyTest()
        {
            Payment emptyPayment = new();

            Assert.ThrowsException<Exception>(() =>
            {
                _paymentManager.AddPayment(emptyPayment);
            }, "An exception should be thrown when trying to add an empty payment.");
        }

        [TestMethod]
        public void GetPaymentsFromDateRangeSuccessfulTest()
        {
            DateTime startDate = DateTime.Today.AddDays(-1);
            DateTime endDate = DateTime.Today.AddDays(15);

            List<Payment>? result = _paymentManager.GetPaymentsFromDateRange(startDate, endDate);

            Assert.IsNotNull(result, "The result should not be null.");
            Assert.IsTrue(result.Count >= 3, "At least three payments should be returned in the valid date range.");
        }

        [TestMethod]
        public void GetPaymentsFromDateRangeInvalidDateTest()
        {
            DateTime startDate = DateTime.Today.AddDays(30);
            DateTime endDate = DateTime.Today.AddDays(31); // Date range with no expected payments

            List<Payment>? result = _paymentManager.GetPaymentsFromDateRange(startDate, endDate);

            Assert.IsNotNull(result, "The result should not be null even for an empty range.");
            Assert.AreEqual(0, result.Count, "No payments should be returned for a range with no data.");
        }
    }
}