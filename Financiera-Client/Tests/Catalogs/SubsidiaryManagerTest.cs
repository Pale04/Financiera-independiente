using Business_logic.Catalogs;
using DomainClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.Catalogs
{
    [TestClass]
    public class SubsidiaryManagerTest
    {
        private static SubsidiaryManager _manager = new();

        private static Subsidiary _subsidiary1 = new()
        {
            Address = "Calle del skibidipapu #32, Ciudad Olmeca, Mexico",
            State = true
        };
        private static Subsidiary _subsidiary2 = new()
        {
            Address = "Hola mundo",
            State = false
        };
        private static Subsidiary _subsidiary3 = new()
        {
            Address = "Adios mundo",
            State = false
        };

        [ClassInitialize]
        public void Initialize()
        {
            _manager.Add(_subsidiary1.Address);
            _manager.Add(_subsidiary3.Address);
        }

        [TestMethod]
        public void AddSubsidiarySuccessfulTest()
        {
            Assert.AreEqual(0, _manager.Add(_subsidiary2.Address));
        }

        [TestMethod]
        public void AddSubsidiaryWithEmptyFieldsTest()
        {
            Assert.AreEqual(2, _manager.Add(""));
        }

        [TestMethod]
        public void AddExistingSubsidiaryTest()
        {
            Assert.AreEqual(3, _manager.Add(_subsidiary1.Address));
        }

        [TestMethod]
        public void UpdateAddressSuccessfulTest()
        {
            Assert.AreEqual(0, _manager.UpdateAddress(1, "Calle del skibidipapu #69, Ciudad Olmeca, Estados unidos"));
        }

        [TestMethod]
        public void UpdateWithExistingAddressTest()
        {
            Assert.AreEqual(3, _manager.UpdateAddress(1, "Adios mundo"));
        }

        [TestMethod]
        public void UpdateWithEmptyAddressTest()
        {
            Assert.AreEqual(2, _manager.UpdateAddress(1, ""));
        }

        [TestMethod]
        public void UpdateAddressToNonExistentSubsidiaryTest()
        {
            Assert.AreEqual(1, _manager.UpdateAddress(99999, "hola"));
        }

        [TestMethod]
        public void UpdateStateSuccessfulTest()
        {
            Assert.AreEqual(0, _manager.UpdateState(1, false));
        }

        [TestMethod]
        public void UpdateStateToNonExistentSubsidiaryTest()
        {
            Assert.AreEqual(1, _manager.UpdateState(99999, true));
        }
    }
}
