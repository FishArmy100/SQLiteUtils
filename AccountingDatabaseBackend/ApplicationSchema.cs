using AccountingDatabaseBackend.DBEntities;
using SQLiteUtils;
using SQLiteUtils.Schema;
using SQLiteUtils.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountingDatabaseBackend
{
    public static class ApplicationSchema
    {
        public const string DATABASE_SCHEMA_NAME = "AccountingDatabase";

        public static DBSchema CreateSchema()
        {
            SchemaBuilder builder = new SchemaBuilder(DATABASE_SCHEMA_NAME);
            builder.AddEntry<Address>(DBTableNames.ADDRESS_TABLE_NAME);
            builder.AddEntry<BudgetReport>(DBTableNames.BUDGET_REPORT_TABLE_NAME);
            builder.AddEntry<Check>(DBTableNames.CHECK_TABLE_NAME);
            builder.AddEntry<Employee>(DBTableNames.EMPLOYEE_TABLE_NAME);
            builder.AddEntry<Expense>(DBTableNames.EXPENSE_TABLE_NAME);
            builder.AddEntry<Receipt>(DBTableNames.RECEIPTS_TABLE_NAME);
            builder.AddEntry<Revenue>(DBTableNames.REVENUE_TABLE_NAME);
            builder.AddEntry<Vendor>(DBTableNames.VENDOR_TABLE_NAME);
            return builder.Build();
        }

        public static void InitTestDB(DBHandle handle)
        {
            DBSchema schema = CreateSchema();
            handle.BuildSchema(schema);

            List<Address> addresses = new List<Address>();
            List<BudgetReport> reports = new List<BudgetReport>();
            List<Check> checks = new List<Check>();
            List<Receipt> receipts = new List<Receipt>();

            List<Vendor> vendors = Vendor.GenerateRandom(5).Select(v =>
            {
                addresses.Add(v.Item2);
                return v.Item1;
            }).ToList();

            List<Employee> employees = Employee.CreateRandom(10).Select(e =>
            {
                addresses.Add(e.Item2);
                return e.Item1;
            }).ToList();

            List<Expense> expenses = Expense.CreateRandom(7, vendors, employees).Select(e =>
            {
                reports.Add(e.Item2);
                checks.Add(e.Item3);
                return e.Item1;
            }).ToList();

            List<Revenue> revenues = Revenue.GenerateRandom(10, employees).Select(r =>
            {
                receipts.Add(r.Item3);
                reports.Add(r.Item2);
                return r.Item1;
            }).ToList();

            SQLSerializer.Serialize(DBTableNames.ADDRESS_TABLE_NAME, handle, addresses);
            SQLSerializer.Serialize(DBTableNames.BUDGET_REPORT_TABLE_NAME, handle, reports);
            SQLSerializer.Serialize(DBTableNames.CHECK_TABLE_NAME, handle, checks);
            SQLSerializer.Serialize(DBTableNames.RECEIPTS_TABLE_NAME, handle, receipts);

            SQLSerializer.Serialize(DBTableNames.VENDOR_TABLE_NAME, handle, vendors);
            SQLSerializer.Serialize(DBTableNames.EMPLOYEE_TABLE_NAME, handle, employees);

            SQLSerializer.Serialize(DBTableNames.EXPENSE_TABLE_NAME, handle, expenses);
            SQLSerializer.Serialize(DBTableNames.REVENUE_TABLE_NAME, handle, revenues);
        }
    }
}
