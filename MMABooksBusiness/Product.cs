using System;
using MMABooksTools;
using MMABooksProps;
using MMABooksDB;
using System.Collections.Generic;

namespace MMABooksBusiness
{
    public class Product : BaseBusiness
    {
        public int ProductID => ((ProductProps)mProps).ProductID;

        public string ProductCode
        {
            get => ((ProductProps)mProps).ProductCode;
            set
            {
                if (value.Trim().Length < 1 || value.Trim().Length > 10)
                {
                    throw new ArgumentOutOfRangeException("ProductCode must be between 1 and 10 characters long.");
                }

                if (value != ((ProductProps)mProps).ProductCode)
                {
                    mRules.RuleBroken("ProductCode", false);
                    ((ProductProps)mProps).ProductCode = value;
                    mIsDirty = true;
                }
            }
        }

        public string Description
        {
            get => ((ProductProps)mProps).Description;
            set
            {
                if (value.Trim().Length < 1 || value.Trim().Length > 50)
                {
                    throw new ArgumentOutOfRangeException("Description must be between 1 and 50 characters long.");
                }

                if (value != ((ProductProps)mProps).Description)
                {
                    mRules.RuleBroken("Description", false);
                    ((ProductProps)mProps).Description = value;
                    mIsDirty = true;
                }
            }
        }

        public decimal UnitPrice
        {
            get => ((ProductProps)mProps).UnitPrice;
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException("UnitPrice must be non-negative.");
                }

                mRules.RuleBroken("UnitPrice", false);
                ((ProductProps)mProps).UnitPrice = value;
                mIsDirty = true;
            }
        }

        public int OnHandQuantity
        {
            get => ((ProductProps)mProps).OnHandQuantity;
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException("OnHandQuantity must be non-negative.");
                }

                mRules.RuleBroken("OnHandQuantity", false);
                ((ProductProps)mProps).OnHandQuantity = value;
                mIsDirty = true;
            }
        }

        public override object GetList()
        {
            var products = new List<Product>();
            var props = (List<ProductProps>)mdbReadable.RetrieveAll();

            foreach (var prop in props)
            {
                var product = new Product(prop);
                products.Add(product);
            }

            return products;
        }

        protected override void SetDefaultProperties()
        {
        }

        protected override void SetRequiredRules()
        {
            mRules.RuleBroken("ProductCode", true);
            mRules.RuleBroken("Description", true);
            mRules.RuleBroken("UnitPrice", true);
            mRules.RuleBroken("OnHandQuantity", true);
        }

        protected override void SetUp()
        {
            mProps = new ProductProps();
            mOldProps = new ProductProps();
            mdbReadable = new ProductDB();
            mdbWriteable = new ProductDB();
        }

        public Product() : base()
        {
        }

        public Product(int key) : base(key.ToString())
        {
            var productDB = new ProductDB();
            var productProps = (ProductProps)productDB.Retrieve(key);
            mProps = productProps ?? throw new Exception("Product not found.");
        }

        public Product(string productCode)
        {
            var productDB = new ProductDB();
            var productProps = (ProductProps)productDB.RetrieveByProductCode(productCode);
            mProps = productProps ?? throw new Exception("Product not found with the given code.");
        }

        private Product(ProductProps props) : base(props)
        {
        }

        public void Save()
        {
            if (string.IsNullOrWhiteSpace(ProductCode))
            {
                throw new Exception("ProductCode is required.");
            }
            if (string.IsNullOrWhiteSpace(Description))
            {
                throw new Exception("Description is required.");
            }
            if (UnitPrice <= 0)
            {
                throw new Exception("UnitPrice must be greater than zero.");
            }
            if (OnHandQuantity < 0)
            {
                throw new Exception("OnHandQuantity cannot be negative.");
            }

            if (mIsDirty)
            {
                if (ProductID == 0)
                {
                    mdbWriteable.Create(mProps);
                }
                else
                {
                    mdbWriteable.Update(mProps);
                }
                mIsDirty = false;
            }
        }

        public void Delete()
        {
            mdbWriteable.Delete(mProps);
        }
    }
}