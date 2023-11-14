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
    public class Expense : ISQLUpdateable
    {
        public int expense_transaction_id;
        public int ledger_account;
        public string description;
        public string debit;
        public string credit;

        public int vendor_id;
        public int check_number;
        public int entering_employee_id;

        public Expense()
        {
            description = string.Empty;
            debit = string.Empty;
            credit = string.Empty;
        }

        public Expense(int expense_id, int ledger_id, string description, string debit, string credit, int vendor_id, int check_id, int entering_employee_id)
        {
            this.expense_transaction_id = expense_id;
            this.ledger_account = ledger_id;
            this.description = description;
            this.debit = debit;
            this.credit = credit;
            this.vendor_id = vendor_id;
            this.check_number = check_id;
            this.entering_employee_id = entering_employee_id;
        }

        public static List<(Expense, BudgetReport, Check)> CreateRandom(int count, List<Vendor> vendors, List<Employee> employees)
        {
            var checks = Check.GenerateRandom(count);
            var budgets = BudgetReport.CreateRandom(count);

            var expenses = new Faker<Expense>()
                .StrictMode(true)
                .RuleFor(e => e.expense_transaction_id, f => f.IndexGlobal)
                .RuleFor(e => e.ledger_account, f => budgets[f.IndexFaker].ledger_account)
                .RuleFor(e => e.description, f => f.Lorem.Paragraph())
                .RuleFor(e => e.debit, f => f.Finance.CreditCardCvv())
                .RuleFor(e => e.credit, f => f.Finance.CreditCardCvv())
                .RuleFor(e => e.vendor_id, f => vendors.RandomFrom(f).vendor_id)
                .RuleFor(e => e.check_number, f => checks[f.IndexFaker].check_number)
                .RuleFor(e => e.entering_employee_id, f => employees.RandomFrom(f).id);

            return expenses.Generate(count).Zip(budgets, checks).ToList();
        }

        public static SchemaEntry GetEntry(string name)
        {
            return new SchemaEntry(name, new SchemaFeild[]
            {
                new SchemaFeild.Basic(nameof(expense_transaction_id), "INT", true),
                new SchemaFeild.Basic(nameof(ledger_account), "INT"),
                new SchemaFeild.Basic(nameof(description), "STRING"),
                new SchemaFeild.Basic(nameof(debit), "VARCHAR(20)"),
                new SchemaFeild.Basic(nameof(credit), "VARCHAR(20)"),
                new SchemaFeild.Basic(nameof(vendor_id), "INT"),
                new SchemaFeild.Basic(nameof(check_number), "INT"),
                new SchemaFeild.Basic(nameof(entering_employee_id), "INT"),

                new SchemaFeild.PrimaryKey(nameof(expense_transaction_id)),

                new SchemaFeild.ForeignKey(nameof(ledger_account), nameof(BudgetReport.ledger_account), DBTableNames.BUDGET_REPORT_TABLE_NAME),
                new SchemaFeild.ForeignKey(nameof(vendor_id), nameof(Vendor.vendor_id), DBTableNames.VENDOR_TABLE_NAME),
                new SchemaFeild.ForeignKey(nameof(check_number), nameof(Check.check_number), DBTableNames.CHECK_TABLE_NAME),
                new SchemaFeild.ForeignKey(nameof(entering_employee_id), nameof(Employee.id), DBTableNames.EMPLOYEE_TABLE_NAME)
            });
        }

        public (string, string) GetId()
        {
            return (nameof(expense_transaction_id), expense_transaction_id.ToString());
        }

        public void OnDeserialize(ref SQLiteDataReader reader)
        {
            expense_transaction_id = reader.GetInt32(0);
            ledger_account = reader.GetInt32(1);
            description = reader.GetString(2);
            debit = reader.GetString(3);
            credit = reader.GetString(4);
            vendor_id = reader.GetInt32(5);
            check_number = reader.GetInt32(6);
            entering_employee_id = reader.GetInt32(7);

        }

        public string[] OnSerialize()
        {
            return new string[]
            {
                expense_transaction_id.ToString(),
                ledger_account.ToString(),
                $"'{description}'",
                $"'{debit}'",
                $"'{credit}'",
                vendor_id.ToString(),
                check_number.ToString(),
                entering_employee_id.ToString()
            };

        }
    }
}
