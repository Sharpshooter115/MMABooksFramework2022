using NUnit.Framework;
using MMABooksProps;
using MMABooksDB;
using System.Data;
using System.Collections.Generic;
using System;
using MMABooksBusiness;

namespace MMABooksTests
{
    [TestFixture]
    public class ProductTests
    {
        [SetUp]
        public void TestResetDatabase()
        {
            var db = new ProductDB();
            var command = new MySql.Data.MySqlClient.MySqlCommand
            {
                CommandText = "usp_testingResetProductData",
                CommandType = CommandType.StoredProcedure
            };
            db.RunNonQueryProcedure(command);
        }

        [Test]
        public void TestRetrieveFromDataStoreConstructor()
        {
            var product = new Product(1);
            Assert.AreEqual(1, product.ProductID);
            Assert.AreEqual("A4CS", product.ProductCode);
            Assert.AreEqual("Murach's ASP.NET 4 Web Programming with C# 2010", product.Description);
            Assert.AreEqual(56.50m, product.UnitPrice);
            Assert.AreEqual(4637, product.OnHandQuantity);
            Assert.IsFalse(product.IsNew);
            Assert.IsTrue(product.IsValid);
        }

        [Test]
        public void TestCreateProduct()
        {
            var product = new Product
            {
                ProductCode = "NEWCODE",
                Description = "Test Product",
                UnitPrice = 56.50m,
                OnHandQuantity = 10
            };

            product.Save();

            var newProduct = new Product(product.ProductID);
            Assert.AreEqual("NEWCODE", newProduct.ProductCode);
            Assert.AreEqual("Test Product", newProduct.Description);
            Assert.AreEqual(56.50m, newProduct.UnitPrice);
            Assert.AreEqual(10, newProduct.OnHandQuantity);
        }

        [Test]
        public void TestUpdateProduct()
        {
            var product = new Product(1);
            product.Description = "Updated Description";
            product.Save();

            var updatedProduct = new Product(1);
            Assert.AreEqual("Updated Description", updatedProduct.Description);
        }

        [Test]
        public void TestDeleteProduct()
        {
            var product = new Product(1);
            product.Delete();

            Assert.Throws<Exception>(() => new Product(1));
        }

        [Test]
        public void TestNoRequiredPropertiesNotSet()
        {
            var product = new Product();
            Assert.Throws<Exception>(() => product.Save());
        }

        [Test]
        public void TestInvalidPropertySet()
        {
            var product = new Product();
            Assert.Throws<ArgumentOutOfRangeException>(() => product.ProductCode = "INVALID_CODE_TOO_LONG");
        }

        [Test]
        public void TestConcurrencyIssue()
        {
            var product1 = new Product(1);
            var product2 = new Product(1);

            product1.Description = "Updated first";
            product1.Save();

            product2.Description = "Updated second";
            Assert.Throws<Exception>(() => product2.Save());
        }
    }
}