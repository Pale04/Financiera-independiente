using Business_logic;
using DomainClasses;

namespace Tests
{
    [TestClass()]
    public class CustomerManagerTests
    {
        private static CustomerManager _customerManager;

        private static Customer _customer1;
        private static BankAccount _receiveBankAccount1;
        private static BankAccount _collectBankAccount1;
        private static PersonalReference _firstPersonalReference1;
        private static PersonalReference _secondPersonalReference1;

        private static Customer _customer2;
        private static BankAccount _receiveBankAccount2;
        private static BankAccount _collectBankAccount2;
        private static PersonalReference _firstPersonalReference2;
        private static PersonalReference _secondPersonalReference2;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            _customerManager = new();
            _customer1 = new()
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
            _receiveBankAccount1 = new()
            {
                Id = 1,
                Clabe = "123456789012345678",
                Purpose = "receive",
                CustomerRfc = _customer1.Rfc
            };
            _collectBankAccount1 = new()
            {
                Id = 2,
                Clabe = "123456789012345678",
                Purpose = "collect",
                CustomerRfc = _customer1.Rfc
            };
            _firstPersonalReference1 = new()
            {
                Id = 1,
                Name = "Maria Lopez",
                PhoneNumber = "1234567890",
                Relationship = "Sister",
                CustomerRfc = _customer1.Rfc
            };
            _secondPersonalReference1 = new()
            {
                Id = 2,
                Name = "Maria Hernández",
                PhoneNumber = "1234567890",
                Relationship = "Mother",
                CustomerRfc = _customer1.Rfc
            };
            _customer1.BankAccounts = [_receiveBankAccount1, _collectBankAccount1];
            _customer1.PersonalReferences = [_firstPersonalReference1, _secondPersonalReference1];

            _customer2 = new()
            {
                Rfc = "ABCD220136A02",
                Name = "María Perez",
                BirthDate = DateOnly.FromDateTime(DateTime.Now.AddYears(-18)),
                HouseAddress = "Calle 123",
                WorkAddress = "Calle 456",
                PhoneNumber1 = "1234567890",
                PhoneNumber2 = "0987654321",
                Email = "correo@gmail.com",
                State = true
            };
            _receiveBankAccount2 = new()
            {
                Id = 3,
                Clabe = "123456789012345678",
                Purpose = "receive",
                CustomerRfc = _customer2.Rfc
            };
            _collectBankAccount2 = new()
            {
                Id = 4,
                Clabe = "123456789012345678",
                Purpose = "collect",
                CustomerRfc = _customer2.Rfc
            };
            _firstPersonalReference2 = new()
            {
                Id = 3,
                Name = "Emilian Pérez",
                PhoneNumber = "1234567890",
                Relationship = "Son",
                CustomerRfc = _customer2.Rfc
            };
            _secondPersonalReference2 = new()
            {
                Id = 4,
                Name = "Maria Hernández",
                PhoneNumber = "1234567890",
                Relationship = "Daughter",
                CustomerRfc = _customer2.Rfc
            };
            _customer2.BankAccounts = [_receiveBankAccount2, _collectBankAccount2];
            _customer2.PersonalReferences = [_firstPersonalReference2, _secondPersonalReference2];

            _customerManager.AddCustomer(_customer1);
            _customerManager.AddCustomer(_customer2);
        }

        [TestMethod()]
        public void AddCustomerSuccessfulTest()
        {
            Customer customer = new()
            {
                Rfc = "ZBCD220136A01",
                Name = "Luis Pérez",
                BirthDate = DateOnly.FromDateTime(DateTime.Now.AddYears(-18)),
                HouseAddress = "Calle 123",
                WorkAddress = "Calle 456",
                PhoneNumber1 = "1234567890",
                PhoneNumber2 = "0987654321",
                Email = "correo@gmail.com"
            };
            BankAccount receiveBankAccount = new()
            {
                Clabe = "123456789012345678",
                Purpose = "receive",
                CustomerRfc = customer.Rfc
            };
            BankAccount collectBankAccount = new()
            {
                Clabe = "123456789012345678",
                Purpose = "collect",
                CustomerRfc = customer.Rfc
            };
            PersonalReference firstPersonalReference = new()
            {
                Name = "Mariano López",
                PhoneNumber = "1234567890",
                Relationship = "Brother",
                CustomerRfc = customer.Rfc
            };
            PersonalReference secondPersonalReference = new()
            {
                Name = "Cecilia Hernández",
                PhoneNumber = "1234567890",
                Relationship = "Mother",
                CustomerRfc = customer.Rfc
            };
            customer.BankAccounts = [receiveBankAccount, collectBankAccount];
            customer.PersonalReferences = [firstPersonalReference, secondPersonalReference];

            int result = _customerManager.AddCustomer(customer);
            Assert.AreEqual(0, result, "AddCustomerSuccessfulTest");
        }

        [TestMethod()]
        public void AddCustomerWithEmptyFieldsTest()
        {
            Customer customer = new();
            BankAccount receiveBankAccount = new();
            BankAccount collectBankAccount = new();
            PersonalReference firstPersonalReference = new();
            PersonalReference secondPersonalReference = new();
            customer.BankAccounts = [receiveBankAccount, collectBankAccount];
            customer.PersonalReferences = [firstPersonalReference, secondPersonalReference];

            Assert.ThrowsException<Exception>(() => _customerManager.AddCustomer(customer), "AddCustomerSuccessfulTest");
        }

        [TestMethod()]
        public void AddCustomerDuplicatedTest()
        {
            Customer customer = new()
            {
                Rfc = _customer1.Rfc,
                Name = "Luis Pérez",
                BirthDate = DateOnly.FromDateTime(DateTime.Now.AddYears(-18)),
                HouseAddress = "Calle 123",
                WorkAddress = "Calle 456",
                PhoneNumber1 = "1234567890",
                PhoneNumber2 = "0987654321",
                Email = "correo@gmail.com"
            };
            BankAccount receiveBankAccount = new()
            {
                Clabe = "123456789012345678",
                Purpose = "receive",
                CustomerRfc = customer.Rfc
            };
            BankAccount collectBankAccount = new()
            {
                Clabe = "123456789012345678",
                Purpose = "collect",
                CustomerRfc = customer.Rfc
            };
            PersonalReference firstPersonalReference = new()
            {
                Name = "Mariano López",
                PhoneNumber = "1234567890",
                Relationship = "Brother",
                CustomerRfc = customer.Rfc
            };
            PersonalReference secondPersonalReference = new()
            {
                Name = "Cecilia Hernández",
                PhoneNumber = "1234567890",
                Relationship = "Mother",
                CustomerRfc = customer.Rfc
            };
            customer.BankAccounts = [receiveBankAccount, collectBankAccount];
            customer.PersonalReferences = [firstPersonalReference, secondPersonalReference];

            Assert.ThrowsException<Exception>(() => _customerManager.AddCustomer(customer), "AddCustomerSuccessfulTest");
        }

        [TestMethod()]
        public void UpdateCustomerPersonalInformationSuccessfulTest()
        {
            _customer1.Name = "Francisco Hernández";
            _customer1.BirthDate = DateOnly.FromDateTime(DateTime.Now.AddYears(-19));
            _customer1.HouseAddress = "Calle 1234";
            _customer1.WorkAddress = "Calle 4567";
            _customer1.PhoneNumber1 = "1234567891";
            _customer1.PhoneNumber2 = "0987654320";
            _customer1.Email = "correoNuevo@gmail.com";

            int result = _customerManager.UpdateCustomerPersonalInformation(_customer1);
            Assert.AreEqual(0, result, "UpdateCustomerPersonalInformationSuccessfulTest");
        }

        [TestMethod()]
        public void UpdateCustomerPersonalInformationEmptyFieldsTest()
        {
            Customer customer = new();
            Assert.ThrowsException<Exception>(() => _customerManager.UpdateCustomerPersonalInformation(customer), "UpdateCustomerPersonalInformationSuccessfulTest");
        }

        [TestMethod()]
        public void UpdateCustomerBankAccountSuccessfulTest()
        {
            _receiveBankAccount1.Clabe = "123456789012345679";

            int result = _customerManager.UpdateCustomerBankAccount(_receiveBankAccount1);
            Assert.AreEqual(0, result, "UpdateCustomerBankAccountSuccessfulTest");
        }

        [TestMethod()]
        public void UpdateCustomerBankAccountEmptyFieldsTest()
        {
            BankAccount bankAccount = new();
            Assert.ThrowsException<Exception>(() => _customerManager.UpdateCustomerBankAccount(bankAccount), "UpdateCustomerBankAccountSuccessfulTest");
        }

        [TestMethod()]
        public void UpdateCustomerPersonalReferenceSuccessfulTest()
        {
            _firstPersonalReference1.Name = "Francisco Sarabia";
            _firstPersonalReference1.PhoneNumber = "2281020301";
            _firstPersonalReference1.Relationship = "Grandfather";

            int result = _customerManager.UpdateCustomerPersonalReference(_firstPersonalReference1);
            Assert.AreEqual(0, result, "UpdateCustomerPersonalReferenceSuccessfulTest");
        }

        [TestMethod()]
        public void UpdateCustomerPersonalReferenceEmptyFieldsTest()
        {
            PersonalReference personalReference = new();
            Assert.ThrowsException<Exception>(() => _customerManager.UpdateCustomerPersonalReference(personalReference), "UpdateCustomerPersonalReferenceSuccessfulTest");
        }

        [TestMethod()]
        public void UpdateCustomerStateSuccessfulTest()
        {
            int result = _customerManager.UpdateCustomerState(_customer1.Rfc, !_customer1.State);
            Assert.AreEqual(0, result, "UpdateCustomerStateSuccessfulTest");
        }

        [TestMethod()]
        public void UpdateCustomerStateWithActiveCreditTest()
        {
            //TODO
        }
    }
}