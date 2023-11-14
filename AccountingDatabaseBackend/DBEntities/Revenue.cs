using Bogus;
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
        public int revenue_transaction_id;
        public int ledger_account;
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
            this.revenue_transaction_id = transaction_id;
            this.ledger_account = ledger_id;
            this.entering_employee_id = employee_id;
            this.receipt_number = receipt_number;
        }

        public static List<(Revenue, BudgetReport, Receipt)> GenerateRandom(int count, List<Employee> employees)
        {
            var ledgers = BudgetReport.CreateRandom(count);
            var receipts = Receipt.GenerateRandom(count);

            var revenues = new Faker<Revenue>()
                .StrictMode(true)
                .RuleFor(r => r.credit, f => f.Finance.CreditCardCvv())
                .RuleFor(r => r.debit, f => f.Finance.CreditCardCvv())
                .RuleFor(r => r.date, RandomUtils.RandomDate)
                .RuleFor(r => r.description, f => f.Lorem.Paragraph())
                .RuleFor(r => r.revenue_transaction_id, f => f.IndexGlobal)
                .RuleFor(r => r.ledger_account, f => ledgers[f.IndexFaker].ledger_account)
                .RuleFor(r => r.entering_employee_id, f => employees.RandomFrom(f).id)
                .RuleFor(r => r.receipt_number, f => receipts[f.IndexFaker].receipt_number);

            return revenues.Generate(count).Zip(ledgers, receipts).ToList();
        }

        public static SchemaEntry GetEntry(string name)
        {
            return new SchemaEntry(name, new SchemaFeild[]
            {
                new SchemaFeild.Basic(nameof(credit), "VARCHAR(20)"),
                new SchemaFeild.Basic(nameof(debit), "VARCHAR(20)"),
                new SchemaFeild.Basic(nameof(date), "DATETIME"),
                new SchemaFeild.Basic(nameof(description), "STRING"),

                new SchemaFeild.Basic(nameof(revenue_transaction_id), "INT"),
                new SchemaFeild.Basic(nameof(ledger_account), "INT"),
                new SchemaFeild.Basic(nameof(entering_employee_id), "INT"),
                new SchemaFeild.Basic(nameof(receipt_number), "INT"),

                new SchemaFeild.PrimaryKey(nameof(revenue_transaction_id)),
                
                new SchemaFeild.ForeignKey(nameof(entering_employee_id), DBTableNames.EMPLOYEE_TABLE_NAME, nameof(Employee.id)),
                new SchemaFeild.ForeignKey(nameof(receipt_number), DBTableNames.RECEIPTS_TABLE_NAME, nameof(Receipt.receipt_number)),
                new SchemaFeild.ForeignKey(nameof(ledger_account), nameof(BudgetReport.ledger_account), DBTableNames.BUDGET_REPORT_TABLE_NAME),
            });
        }

        public (string, string) GetId()
        {
            return (nameof(revenue_transaction_id), revenue_transaction_id.ToString());
        }

        public void OnDeserialize(ref SQLiteDataReader reader)
        {
            credit = reader.GetString(0);
            debit = reader.GetString(1);
            date = reader.GetDateTime(2);
            description = reader.GetString(3);
            revenue_transaction_id = reader.GetInt32(4);
            ledger_account = reader.GetInt32(5);
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
                revenue_transaction_id.ToString(),
                ledger_account.ToString(),
                entering_employee_id.ToString(),
                receipt_number.ToString(),
            };
        }
    }
}