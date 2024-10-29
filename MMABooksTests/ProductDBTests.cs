using NUnit.Framework;
using MMABooksProps;
using MMABooksDB;
using DBCommand = MySql.Data.MySqlClient.MySqlCommand;
using System.Data;
using System.Collections.Generic;
using System;
using MySql.Data.MySqlClient;
using MMABooksBusiness;

namespace MMABooksTests
{
    [TestFixture]
    public class ProductTests
    {
        [SetUp]
        public void TestResetDatabase()
        {
            ProductDB db = new ProductDB();
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
            Product p = new Product(3722);
            Assert.AreEqual(3722, p.ProductID);
            Assert.AreEqual("JSP2", p.ProductCode);
            Assert.AreEqual("Murach's JAVA Servlets and JSP (2nd Edition)", p.Description);
            Assert.AreEqual(52.50m, p.UnitPrice);
            Assert.AreEqual(4999, p.OnHandQuantity);
            Assert.IsFalse(p.IsNew);
            Assert.IsTrue(p.IsValid);
        }

        [Test]
        public void TestSaveNewProduct()
        {
            Product p = new Product
            {
                ProductCode = "NEWCODE",
                Description = "New Product",
                UnitPrice = 20.00m,
                OnHandQuantity = 10
            };
            p.Save();

            Product p2 = new Product("NEWCODE");
            Assert.AreEqual("NEWCODE", p2.ProductCode);
            Assert.AreEqual("New Product", p2.Description);
        }

        [Test]
        public void TestUpdateExistingProduct()
        {
            Product p = new Product(3714);
            p.Description = "Updated Description";
            p.Save();

            Product p2 = new Product(3714);
            Assert.AreEqual("Updated Description", p2.Description);
        }

        [Test]
        public void TestDeleteProduct()
        {
            Product p = new Product(3721);
            p.Delete();
            Assert.Throws<Exception>(() => new Product(3721));
        }

        [Test]
        public void TestGetList()
        {
            Product p = new Product();
            List<Product> products = (List<Product>)p.GetList();
            Assert.IsTrue(products.Count > 0);
            Assert.AreEqual("A4CS", products[0].ProductCode);
        }

        [Test]
        public void TestNoRequiredPropertiesNotSet()
        {
            Product p = new Product();
            Assert.Throws<Exception>(() => p.Save());
        }

        [Test]
        public void TestSomeRequiredPropertiesNotSet()
        {
            Product p = new Product();
            p.ProductCode = "??";
            Assert.Throws<Exception>(() => p.Save());
        }

        [Test]
        public void TestInvalidPropertySet()
        {
            Product p = new Product();
            Assert.Throws<ArgumentOutOfRangeException>(() => p.ProductCode = "INVALID_CODE_TOO_LONG");
        }

        [Test]
        public void TestConcurrencyIssue()
        {
            Product p1 = new Product(3728);
            Product p2 = new Product(3728);

            p1.Description = "Updated first";
            p1.Save();

            p2.Description = "Updated second";
            Assert.Throws<Exception>(() => p2.Save());
        }
    }
}