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
    public class Revenue : ISQLUpdateable
    {
        public string credit;
        public string debit;
        public DateTime date;
        public string description;
        // Primary key
        public int transaction_id;
        public int ledger_id;
        public int entering_employee_id;
        public int receipt_number;

        public Revenue()
        {
            credit = "";
            debit = "";
            description = "";
        }

        public Revenue(string credit, string debit, DateTime date, string description, int transaction_id, int ledger_id, int employee_id, int receipt_number)
        {
            this.credit = credit;
            this.debit = debit;
            this.date = date;
            this.description = description;
            this.transaction_id = transaction_id;
            this.ledger_id = ledger_id;
            this.entering_employee_id = employee_id;
            this.receipt_number = receipt_number;
        }

        public static SchemaEntry GetEntry(string name)
        {
            return new SchemaEntry(name, new SchemaFeild[]
            {
                new SchemaFeild.Basic(nameof(credit), "VARCHAR(20)"),
                new SchemaFeild.Basic(nameof(debit), "VARCHAR(20)"),
                new SchemaFeild.Basic(nameof(date), "DATETIME"),
                new SchemaFeild.Basic(nameof(description), "STRING"),

                new SchemaFeild.Basic(nameof(transaction_id), "INT"),
                new SchemaFeild.Basic(nameof(ledger_id), "INT"),
                new SchemaFeild.Basic(nameof(entering_employee_id), "INT"),
                new SchemaFeild.Basic(nameof(receipt_number), "INT"),

                new SchemaFeild.PrimaryKey(nameof(transaction_id)),
                new SchemaFeild.ForeignKey(nameof(entering_employee_id), DBTableNames.EMPLOYEE_TABLE_NAME, nameof(Employee.id)),
                new SchemaFeild.ForeignKey(nameof(receipt_number), DBTableNames.RECEIPTS_TABLE_NAME, nameof(Receipt.receipt_number))
            });
        }

        public (string, string) GetId()
        {
            return (nameof(transaction_id), transaction_id.ToString());
        }

        public void OnDeserialize(ref SQLiteDataReader reader)
        {
            credit = reader.GetString(0);
            debit = reader.GetString(1);
            date = reader.GetDateTime(2);
            description = reader.GetString(3);
            transaction_id = reader.GetInt32(4);
            ledger_id = reader.GetInt32(5);
            entering_employee_id = reader.GetInt32(6);
            receipt_number = reader.GetInt32(7);
        }

        public string[] OnSerialize()
        {
            return new string[]
            {
                $"'{credit}'",
                $"'{debit}'",
                $"{date.Year}-{date.Month}-{date.Day}",
                $"'{description}'",
                transaction_id.ToString(),
                ledger_id.ToString(),
                entering_employee_id.ToString(),
                receipt_number.ToString(),
            };
        }
    }
}