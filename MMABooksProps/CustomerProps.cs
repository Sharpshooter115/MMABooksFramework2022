using System;
using MMABooksTools;
using DBDataReader = MySql.Data.MySqlClient.MySqlDataReader;
using System.Text.Json;

namespace MMABooksProps
{
    [Serializable()]
    public class CustomerProps : IBaseProps
    {
        #region Auto-implemented Properties
        public int CustomerID { get; set; }
        public string Name { get; set; } = "";
        public string Address { get; set; } = "";
        public string City { get; set; } = "";
        public string State { get; set; } = "";
        public string ZipCode { get; set; } = "";

        public int ConcurrencyID { get; set; } = 0;
        #endregion

        public object Clone()
        {
            return new CustomerProps
            {
                CustomerID = this.CustomerID,
                Name = this.Name,
                Address = this.Address,
                City = this.City,
                State = this.State,
                ZipCode = this.ZipCode,
                ConcurrencyID = this.ConcurrencyID
            };
        }

        public string GetState()
        {
            return JsonSerializer.Serialize(this);
        }

        public void SetState(string jsonString)
        {
            var props = JsonSerializer.Deserialize<CustomerProps>(jsonString);
            if (props != null)
            {
                this.CustomerID = props.CustomerID;
                this.Name = props.Name;
                this.Address = props.Address;
                this.City = props.City;
                this.State = props.State;
                this.ZipCode = props.ZipCode;
                this.ConcurrencyID = props.ConcurrencyID;
            }
        }

        public void SetState(DBDataReader dr)
        {
            this.CustomerID = (int)dr["CustomerID"];
            this.Name = dr["Name"].ToString().Trim();
            this.Address = dr["Address"].ToString().Trim();
            this.City = dr["City"].ToString().Trim();
            this.State = dr["State"].ToString().Trim();
            this.ZipCode = dr["ZipCode"].ToString().Trim();
            this.ConcurrencyID = (int)dr["ConcurrencyID"];
        }
    }
}