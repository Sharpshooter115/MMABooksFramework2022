using System;
using System.Collections.Generic;
using System.Data;
using MMABooksTools;
using MMABooksProps;
using DBDataReader = MySql.Data.MySqlClient.MySqlDataReader;
using DBConnection = MySql.Data.MySqlClient.MySqlConnection;
using DBCommand = MySql.Data.MySqlClient.MySqlCommand;
using DBDbType = MySql.Data.MySqlClient.MySqlDbType;

namespace MMABooksDB
{
    public class CustomerDB : BaseSQLDB, IReadDB, IWriteDB
    {
        public CustomerDB() : base() { }
        public CustomerDB(DBConnection cn) : base(cn) { }

        public IBaseProps Create(IBaseProps p)
        {
            int rowsAffected = 0;
            CustomerProps props = (CustomerProps)p;

            using (DBCommand command = new DBCommand("usp_CustomerCreate", mConnection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("name_p", props.Name);
                command.Parameters.AddWithValue("address_p", props.Address);
                command.Parameters.AddWithValue("city_p", props.City);
                command.Parameters.AddWithValue("state_p", props.State);
                command.Parameters.AddWithValue("zipcode_p", props.ZipCode);
                command.Parameters.Add("custId", DBDbType.Int32).Direction = ParameterDirection.Output;

                try
                {
                    rowsAffected = RunNonQueryProcedure(command);
                    if (rowsAffected == 1)
                    {
                        props.CustomerID = (int)command.Parameters["custId"].Value;
                        props.ConcurrencyID = 1;
                        return props;
                    }
                    else
                    {
                        throw new Exception("Unable to insert record.");
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        public IBaseProps Retrieve(object key)
        {
            CustomerProps props = new CustomerProps();

            using (DBCommand command = new DBCommand("usp_CustomerRetrieve", mConnection)) // Update procedure name here
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("custId", key);

                using (DBDataReader reader = RunProcedure(command))
                {
                    if (reader.Read())
                    {
                        props.SetState(reader);
                    }
                    else
                    {
                        throw new Exception("Customer not found.");
                    }
                }
            }
            return props;
        }

        public object RetrieveAll()
        {
            List<CustomerProps> list = new List<CustomerProps>();

            using (DBCommand command = new DBCommand("usp_CustomerSelectAll", mConnection))
            {
                command.CommandType = CommandType.StoredProcedure;

                using (DBDataReader reader = RunProcedure(command))
                {
                    while (reader.Read())
                    {
                        CustomerProps props = new CustomerProps();
                        props.SetState(reader);
                        list.Add(props);
                    }
                }
            }
            return list;
        }

        public bool Update(IBaseProps p)
        {
            CustomerProps props = (CustomerProps)p;
            int rowsAffected = 0;

            using (DBCommand command = new DBCommand("usp_CustomerUpdate", mConnection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("custId", props.CustomerID);
                command.Parameters.AddWithValue("name_p", props.Name);
                command.Parameters.AddWithValue("address_p", props.Address);
                command.Parameters.AddWithValue("city_p", props.City);
                command.Parameters.AddWithValue("state_p", props.State);
                command.Parameters.AddWithValue("zipcode_p", props.ZipCode);
                command.Parameters.AddWithValue("conCurrId", props.ConcurrencyID);

                try
                {
                    rowsAffected = RunNonQueryProcedure(command);
                    if (rowsAffected == 1)
                    {
                        props.ConcurrencyID++;
                        return true;
                    }
                    else
                    {
                        throw new Exception("Record cannot be updated. It may have been edited by another user.");
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        public bool Delete(IBaseProps p)
        {
            CustomerProps props = (CustomerProps)p;
            int rowsAffected = 0;

            using (DBCommand command = new DBCommand("usp_CustomerDelete", mConnection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("custId", props.CustomerID);
                command.Parameters.AddWithValue("conCurrId", props.ConcurrencyID);

                try
                {
                    rowsAffected = RunNonQueryProcedure(command);
                    return rowsAffected == 1;
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
    }
}
