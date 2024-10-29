using System;
using MMABooksTools;
using DBDataReader = MySql.Data.MySqlClient.MySqlDataReader;
using System.Text.Json;

namespace MMABooksProps
{
    [Serializable()]
    public class ProductProps : IBaseProps
    {
        #region Auto-implemented Properties

        public int ProductID { get; set; } = 0;
        public string ProductCode { get; set; } = "";
        public string Description { get; set; } = "";
        public decimal UnitPrice { get; set; } = 0.0m;
        public int OnHandQuantity { get; set; } = 0;

        public int ConcurrencyID { get; set; } = 0;
        #endregion

        public object Clone()
        {
            return new ProductProps();
            {
                ProductID = this.ProductID ,
                ProductCode = this.ProductCode ,
                Description = this.Description ,
                UnitPrice = this.UnitPrice ,
                OnHandQuantity = this.OnHandQuantity ,
                ConcurrencyID = this.ConcurrencyID


            };

        }
        public string GetState()
        {
            return JsonSerializer.Serialize(this);
        }
        public void SetState(string jsonString)
        {
            var props = JsonSerializer.Deserialize<ProductProps>(jsonString);
            if (props != null)
            {
                this.ProductID = props.ProductID;
                this.ProductCode = props.ProductCode;
                this.Description = props.Description;
                this.UnitPrice = props.UnitPrice;
                this.OnHandQuantity = props.OnHandQuantity;
                this.ConcurrencyID = props.ConcurrencyID;
            }
        }
        public void SetState(DBDataReader dr)
        {
            this.ProductID = (int)dr["ProductID"];
            this.ProductCode = dr["ProductCode"].ToString().Trim();
            this.Description = dr["Description"].ToString().Trim();
            this.UnitPrice = (decimal)dr["UnitPrice"];
            this.OnHandQuantity = (int)dr["OnHandQuantity"];
            this.ConcurrencyID = (int)dr["ConcurrencyID"];
        }
    }
}
