using System;
using System.Collections.Generic;
using MMABooksTools;
using MMABooksProps;
using MMABooksDB;

namespace MMABooksBusiness
{
    public class Customer : BaseBusiness
    {
        public string Name
        {
            get { return ((CustomerProps)mProps).Name; }
            set
            {
                if (value != ((CustomerProps)mProps).Name)
                {
                    if (value.Trim().Length >= 1 && value.Trim().Length <= 100)
                    {
                        mRules.RuleBroken("Name", false);
                        ((CustomerProps)mProps).Name = value;
                        mIsDirty = true;
                    }
                    else
                    {
                        throw new ArgumentOutOfRangeException("Name must be between 1 and 100 characters long.");
                    }
                }
            }
        }

        public string Address
        {
            get { return ((CustomerProps)mProps).Address; }
            set
            {
                if (value != ((CustomerProps)mProps).Address)
                {
                    if (value.Trim().Length >= 1 && value.Trim().Length <= 50)
                    {
                        mRules.RuleBroken("Address", false);
                        ((CustomerProps)mProps).Address = value;
                        mIsDirty = true;
                    }
                    else
                    {
                        throw new ArgumentOutOfRangeException("Address must be between 1 and 50 characters long.");
                    }
                }
            }
        }

        public string City
        {
            get { return ((CustomerProps)mProps).City; }
            set
            {
                if (value != ((CustomerProps)mProps).City)
                {
                    if (value.Trim().Length >= 1 && value.Trim().Length <= 20)
                    {
                        mRules.RuleBroken("City", false);
                        ((CustomerProps)mProps).City = value;
                        mIsDirty = true;
                    }
                    else
                    {
                        throw new ArgumentOutOfRangeException("City must be between 1 and 20 characters long.");
                    }
                }
            }
        }

        public string State
        {
            get { return ((CustomerProps)mProps).State; }
            set
            {
                if (value != ((CustomerProps)mProps).State)
                {
                    if (value.Trim().Length == 2)
                    {
                        mRules.RuleBroken("State", false);
                        ((CustomerProps)mProps).State = value;
                        mIsDirty = true;
                    }
                    else
                    {
                        throw new ArgumentOutOfRangeException("State must be exactly 2 characters long.");
                    }
                }
            }
        }

        public string ZipCode
        {
            get { return ((CustomerProps)mProps).ZipCode; }
            set
            {
                if (value != ((CustomerProps)mProps).ZipCode)
                {
                    if (value.Trim().Length <= 15)
                    {
                        mRules.RuleBroken("ZipCode", false);
                        ((CustomerProps)mProps).ZipCode = value;
                        mIsDirty = true;
                    }
                    else
                    {
                        throw new ArgumentOutOfRangeException("ZipCode must be no more than 15 characters long.");
                    }
                }
            }
        }

        public override object GetList()
        {
            List<Customer> customers = new List<Customer>();
            List<CustomerProps> props = (List<CustomerProps>)mdbReadable.RetrieveAll();
            foreach (CustomerProps prop in props)
            {
                Customer c = new Customer(prop);
                customers.Add(c);
            }
            return customers;
        }

        protected override void SetDefaultProperties()
        {
        }

        protected override void SetRequiredRules()
        {
            mRules.RuleBroken("Name", true);
            mRules.RuleBroken("Address", true);
            mRules.RuleBroken("City", true);
            mRules.RuleBroken("State", true);
            mRules.RuleBroken("ZipCode", true);
        }

        protected override void SetUp()
        {
            mProps = new CustomerProps();
            mOldProps = new CustomerProps();
            mdbReadable = new CustomerDB();
            mdbWriteable = new CustomerDB();
        }

        #region constructors
        public Customer() : base()
        {
        }

        public Customer(string key) : base(key)
        {
        }

        private Customer(CustomerProps props) : base(props)
        {
        }
        #endregion
    }
}
