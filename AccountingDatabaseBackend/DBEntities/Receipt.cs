using SQLiteUtils.Schema;
using SQLiteUtils.Serialization;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountingDatabaseBackend.DBEntities
{
    public class Receipt : ISQLUpdateable
    {
        public int receipt_number;
        public float amount;
        public DateTime date;

        public Receipt() {}

        public Receipt(int receipt_number, float amount, DateTime date)
        {
            this.receipt_number = receipt_number;
            this.amount = amount;
            this.date = date;
        }

        public static SchemaEntry GetEntry(string name)
        {
            return new SchemaEntry(name, new SchemaFeild[]
            {
                new SchemaFeild.Basic(nameof(receipt_number), "INT"),
                new SchemaFeild.Basic(nameof(amount), "FLOAT"),
                new SchemaFeild.Basic(nameof(date), "DATETIME"),
                new SchemaFeild.PrimaryKey(nameof(receipt_number)),
            });
        }

        public (string, string) GetId()
        {
            return (nameof(receipt_number), receipt_number.ToString());
        }

        public void OnDeserialize(ref SQLiteDataReader reader)
        {
            receipt_number = reader.GetInt32(0);
            amount = reader.GetFloat(1);
            date = reader.GetDateTime(2);
        }

        public string[] OnSerialize()
        {
            return new string[]
            {
                receipt_number.ToString(),
                $"'{amount}'",
                $"{date.Year}-{date.Month}-{date.Day}"
            };
        }
    }
}
