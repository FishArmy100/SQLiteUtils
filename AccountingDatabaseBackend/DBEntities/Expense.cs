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
    public class Expense : ISQLUpdateable
    {
        public int expense_id;
        public int ledger_id;
        public string description;
        public string debit;
        public string credit;

        public int vendor_id;
        public int check_id;
        public int entering_employee_id;

        public Expense()
        {
            description = string.Empty;
            debit = string.Empty;
            credit = string.Empty;
        }

        public Expense(int expense_id, int ledger_id, string description, string debit, string credit, int vendor_id, int check_id, int entering_employee_id)
        {
            this.expense_id = expense_id;
            this.ledger_id = ledger_id;
            this.description = description;
            this.debit = debit;
            this.credit = credit;
            this.vendor_id = vendor_id;
            this.check_id = check_id;
            this.entering_employee_id = entering_employee_id;
        }

        public static SchemaEntry GetEntry(string name)
        {
            return new SchemaEntry(name, new SchemaFeild[]
            {
                new SchemaFeild.Basic(nameof(expense_id), "INT", true),
                new SchemaFeild.Basic(nameof(ledger_id), "INT"),
                new SchemaFeild.Basic(nameof(description), "STRING"),
                new SchemaFeild.Basic(nameof(debit), "VARCHAR(20)"),
                new SchemaFeild.Basic(nameof(credit), "VARCHAR(20)"),
                new SchemaFeild.Basic(nameof(vendor_id), "INT"),
                new SchemaFeild.Basic(nameof(check_id), "INT"),
                new SchemaFeild.Basic(nameof(entering_employee_id), "INT"),

                new SchemaFeild.PrimaryKey(nameof(expense_id)),

                new SchemaFeild.ForeignKey(nameof(vendor_id), nameof(Vendor.vendor_id), DBTableNames.VENDOR_TABLE_NAME),
                new SchemaFeild.ForeignKey(nameof(check_id), nameof(Check.check_number), DBTableNames.CHECK_TABLE_NAME),
                new SchemaFeild.ForeignKey(nameof(entering_employee_id), nameof(Employee.id), DBTableNames.EMPLOYEE_TABLE_NAME)
            });
        }

        public (string, string) GetId()
        {
            return (nameof(expense_id), expense_id.ToString());
        }

        public void OnDeserialize(ref SQLiteDataReader reader)
        {
            expense_id = reader.GetInt32(0);
            ledger_id = reader.GetInt32(1);
            description = reader.GetString(2);
            debit = reader.GetString(3);
            credit = reader.GetString(4);
            vendor_id = reader.GetInt32(5);
            check_id = reader.GetInt32(6);
            entering_employee_id = reader.GetInt32(7);

        }

        public string[] OnSerialize()
        {
            return new string[]
            {
                expense_id.ToString(),
                ledger_id.ToString(),
                $"'{description}'",
                $"'{debit}'",
                $"'{credit}'",
                vendor_id.ToString(),
                check_id.ToString(),
                entering_employee_id.ToString()
            };

        }
    }
}
