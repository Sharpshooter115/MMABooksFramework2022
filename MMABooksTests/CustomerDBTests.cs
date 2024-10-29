using System;
using NUnit.Framework;
using MMABooksDB;
using MMABooksProps;

namespace MMABooksTests
{
    [TestFixture]
    public class CustomerDBTests
    {
        private CustomerDB _customerDB;
        private int _testCustomerId;

        [SetUp]
        public void Setup()
        {
            _customerDB = new CustomerDB();
            var customerProps = new CustomerProps
            {
                Name = "Molunguri, A",
                Address = "1108 Johanna Bay Drive",
                City = "Birmingham",
                State = "AL",
                ZipCode = "35216-6909"
            };
            var createdCustomer = _customerDB.Create(customerProps) as CustomerProps;
            _testCustomerId = createdCustomer.CustomerID;
        }

        [Test]
        public void Create_Customer_Returns_CustomerProps()
        {
            var customerProps = new CustomerProps
            {
                Name = "John Doe",
                Address = "123 Elm St",
                City = "Springfield",
                State = "IL",
                ZipCode = "62701"
            };

            var result = _customerDB.Create(customerProps) as CustomerProps;

            Assert.IsNotNull(result);
            Assert.AreEqual("John Doe", result.Name);
            Assert.IsTrue(result.CustomerID > 0);
        }

        [Test]
        public void Retrieve_ExistingCustomer_Returns_CustomerProps()
        {
            var result = _customerDB.Retrieve(_testCustomerId) as CustomerProps;

            Assert.IsNotNull(result);
            Assert.AreEqual(_testCustomerId, result.CustomerID);
            Assert.AreEqual("Molunguri, A", result.Name);
        }

        [Test]
        public void Update_ExistingCustomer_UpdatesCustomerProps()
        {
            var customerProps = new CustomerProps
            {
                CustomerID = _testCustomerId,
                Name = "Updated Name",
                Address = "Updated Address",
                City = "Updated City",
                State = "AL",
                ZipCode = "35216-6909",
                ConcurrencyID = 1
            };

            var updateResult = _customerDB.Update(customerProps);

            Assert.IsTrue(updateResult);

            var updatedCustomer = _customerDB.Retrieve(_testCustomerId) as CustomerProps;
            Assert.AreEqual("Updated Name", updatedCustomer.Name);
        }

        [Test]
        public void Delete_ExistingCustomer_ReturnsTrue()
        {
            var customerProps = new CustomerProps
            {
                CustomerID = _testCustomerId,
                ConcurrencyID = 1
            };

            var result = _customerDB.Delete(customerProps);

            Assert.IsTrue(result);
        }

        [Test]
        public void RetrieveAll_Returns_ListOfCustomers()
        {
            var result = _customerDB.RetrieveAll() as List<CustomerProps>;

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count > 0);
        }

        [TearDown]
        public void Cleanup()
        {
            var customerProps = new CustomerProps { CustomerID = _testCustomerId, ConcurrencyID = 1 };
            _customerDB.Delete(customerProps);
        }
    }
}
