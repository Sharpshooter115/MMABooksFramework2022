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
            var props = (ProductProps)p;
            using (var command = new DBCommand())
            {
                command.CommandText = "usp_ProductCreate";
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("productCode_p", props.ProductCode);
                command.Parameters.AddWithValue("description_p", props.Description);
                command.Parameters.AddWithValue("unitPrice_p", props.UnitPrice);
                command.Parameters.AddWithValue("onHandQuantity_p", props.OnHandQuantity);

                var outputParam = command.Parameters.Add("prodId", DBDbType.Int32);
                outputParam.Direction = ParameterDirection.Output;

                try
                {
                    int rowsAffected = RunNonQueryProcedure(command);
                    if (rowsAffected == 1)
                    {
                        props.ProductID = Convert.ToInt32(outputParam.Value);
                        props.ConcurrencyID = 1;
                        return props;
                    }
                    throw new Exception("Unable to insert record. " + props.GetState());
                }
                catch (Exception ex)
                {
                    throw new Exception("Error creating product: " + ex.Message, ex);
                }
            }
        }

        public bool Delete(IBaseProps p)
        {
            var props = (ProductProps)p;
            using (var command = new DBCommand())
            {
                command.CommandText = "usp_ProductDelete";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("prodId", DBDbType.Int32).Value = props.ProductID;
                command.Parameters.Add("concurrencyId_p", DBDbType.Int32).Value = props.ConcurrencyID;

                try
                {
                    int rowsAffected = RunNonQueryProcedure(command);
                    if (rowsAffected == 1)
                    {
                        return true;
                    }
                    throw new Exception("Record cannot be deleted. It has been edited by another user.");
                }
                catch (Exception ex)
                {
                    throw new Exception("Error deleting product: " + ex.Message, ex);
                }
            }
        }

        public IBaseProps Retrieve(object key)
        {
            var props = new ProductProps();
            using (var command = new DBCommand())
            {
                command.CommandText = "usp_ProductSelect";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("prodId", DBDbType.Int32).Value = key;

                using (var data = RunProcedure(command))
                {
                    if (!data.IsClosed && data.Read())
                    {
                        props.SetState(data);
                        return props;
                    }
                    throw new Exception("Record does not exist in the database.");
                }
            }
        }

        public ProductProps RetrieveByProductCode(string productCode)
        {
            var props = new ProductProps();
            using (var command = new DBCommand())
            {
                command.CommandText = "usp_ProductSelectByCode";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("productCode", DBDbType.VarChar).Value = productCode;

                using (var data = RunProcedure(command))
                {
                    if (!data.IsClosed && data.Read())
                    {
                        props.SetState(data);
                        return props;
                    }
                    throw new Exception("Record does not exist in the database.");
                }
            }
        }

        public object RetrieveAll()
        {
            var list = new List<ProductProps>();
            using (var reader = RunProcedure("usp_ProductSelectAll"))
            {
                while (reader.Read())
                {
                    var props = new ProductProps();
                    props.SetState(reader);
                    list.Add(props);
                }
            }
            return list;
        }

        public bool Update(IBaseProps p)
        {
            var props = (ProductProps)p;
            using (var command = new DBCommand())
            {
                command.CommandText = "usp_ProductUpdate";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("prodId", DBDbType.Int32).Value = props.ProductID;
                command.Parameters.Add("productCode_p", DBDbType.VarChar).Value = props.ProductCode;
                command.Parameters.Add("description_p", DBDbType.VarChar).Value = props.Description;
                command.Parameters.Add("unitPrice_p", DBDbType.Decimal).Value = props.UnitPrice;
                command.Parameters.Add("onHandQuantity_p", DBDbType.Int32).Value = props.OnHandQuantity;
                command.Parameters.Add("concurrencyId_p", DBDbType.Int32).Value = props.ConcurrencyID;

                try
                {
                    int rowsAffected = RunNonQueryProcedure(command);
                    if (rowsAffected == 1)
                    {
                        props.ConcurrencyID++;
                        return true;
                    }
                    throw new Exception("Record cannot be updated. It has been edited by another user.");
                }
                catch (Exception ex)
                {
                    throw new Exception("Error updating product: " + ex.Message, ex);
                }
            }
        }
    }
}