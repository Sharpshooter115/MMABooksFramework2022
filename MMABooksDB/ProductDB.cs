using System;
using System.Collections.Generic;
using System.Data;
using MMABooksTools;
using MMABooksProps;
using System.Data.Common;
using DBDataReader = MySql.Data.MySqlClient.MySqlDataReader;
using DBConnection = MySql.Data.MySqlClient.MySqlConnection;
using DBCommand = MySql.Data.MySqlClient.MySqlCommand;
using DBDbType = MySql.Data.MySqlClient.MySqlDbType;


namespace MMABooksDB
{
    public class ProductDB : BaseSQLDB, IReadDB, IWriteDB
    {
        public ProductDB() : base() { }
        public ProductDB(DBConnection cn) : base(cn) { }

        public IBaseProps Create(IBaseProps p)

        {
            int rowsAffected = 0;
            ProductProps props = (ProductProps)p;

            DBCommand command = new DBCommand();
            command.CommandText = "usp_ProductCreate";
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("productCode", props.ProductCode);
            command.Parameters.AddWithValue("description", props.Description);
            command.Parameters.AddWithValue("unitPrice", props.UnitPrice);
            command.Parameters.AddWithValue("onHandQuantity", props.OnHandQuantity);

            try
            {

                rowsAffected = RunNonQueryProcedure(command);
                if (rowsAffected == 1)
                {

                    props.ConcurrencyID = 1;
                    return props;

                }

                else
                    throw new Exception("Unable to insert record. " + props.GetState());
            }

            catch (Exception e)
            {
                throw;
            }
            finally
            {
                if (mConnection.State == ConnectionState.Open)
                    mConnection.Close();

            }
        }

        public bool Delete(IBaseProps p)
        {
            ProductProps props = (ProductProps)p;
            int rowsAffected = 0;

            DBCommand command = new DBCommand();
            command.CommandText = "usp_ProductDelete";
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("productId", DBDbType.Int32);
            command.Parameters.Add("conCurrId", DBDbType.Int32);
            command.Parameters["productId"].Value = props.ProductID;
            command.Parameters["conCurrId"].Value = props.ConcurrencyID;

            try
            {
                rowsAffected = RunNonQueryProcedure(command);
                if (rowsAffected == 1)
                {
                    return true;
                }
                else
                {
                    throw new Exception("Record cannot be deleted. It has been edited by another user.");
                }
            }
            catch (Exception e)
            {
                throw;
            }
            finally
            {
                if (mConnection.State == ConnectionState.Open)
                    mConnection.Close();
            }
        }
        public IBaseProps Retrieve(object key)
        {
            DBDataReader data = null;
            ProductProps props = new ProductProps();
            DBCommand command = new DBCommand();

            command.CommandText = "usp_ProductSelect";
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("productId", DBDbType.Int32);
            command.Parameters["productId"].Value = key;

            try
            {
                data = RunProcedure(command);
                if (!data.IsClosed)
                {
                    if (data.Read())
                    {
                        props.SetState(data);
                    }
                    else
                        throw new Exception("Record does not exist in the database.");
                }
                return props;
            }
            catch (Exception e)
            {
                throw;
            }
            finally
            {
                if (data != null)
                {
                    if (!data.IsClosed)
                        data.Close();
                }
            }
        }
        public object RetrieveAll()
        {
            List<ProductProps> list = new List<ProductProps>();
            DBDataReader reader = null;
            ProductProps props;

            try
            {
                reader = RunProcedure("usp_ProductSelectAll");
                if (!reader.IsClosed)
                {
                    while (reader.Read())
                    {
                        props = new ProductProps();
                        props.SetState(reader);
                        list.Add(props);
                    }
                }
                return list;
            }
            catch (Exception e)
            {
                throw;
            }
            finally
            {
                if (!reader.IsClosed)
                {
                    reader.Close();
                }
            }
        }
        public bool Update(IBaseProps p)
        {
            int rowsAffected = 0;
            ProductProps props = (ProductProps)p;

            DBCommand command = new DBCommand();
            command.CommandText = "usp_ProductUpdate";
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("productId", DBDbType.Int32);
            command.Parameters.Add("productCode", DBDbType.VarChar);
            command.Parameters.Add("description", DBDbType.VarChar);
            command.Parameters.Add("unitPrice", DBDbType.Decimal);
            command.Parameters.Add("onHandQuantity", DBDbType.Int32);
            command.Parameters.Add("conCurrId", DBDbType.Int32);
            command.Parameters["productId"].Value = props.ProductID;
            command.Parameters["productCode"].Value = props.ProductCode;
            command.Parameters["description"].Value = props.Description;
            command.Parameters["unitPrice"].Value = props.UnitPrice;
            command.Parameters["onHandQuantity"].Value = props.OnHandQuantity;
            command.Parameters["conCurrId"].Value = props.ConcurrencyID;

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
                    throw new Exception("Record cannot be updated. It has been edited by another user.");
                }
            }
            catch (Exception e)
            {
                throw;
            }
            finally
            {
                if (mConnection.State == ConnectionState.Open)
                    mConnection.Close();
            }
        }
    }
}
