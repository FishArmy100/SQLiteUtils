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
    public class BudgetReport : ISQLUpdateable
	{
		public int ledger_account;
		public DateTime date_from;
		public DateTime date_to;
		public float balance_of_ledger_account;
		public float bugeted_amount;

		public BudgetReport()
		{
		}

		public BudgetReport(int ledger_account, DateTime date_from, DateTime date_to, float balance_of_ledger_account, float bugeted_amount)
		{
			this.ledger_account = ledger_account;
			this.date_from = date_from;
			this.date_to = date_to;
			this.balance_of_ledger_account = balance_of_ledger_account;
			this.bugeted_amount = bugeted_amount;
		}

		public static List<BudgetReport> CreateRandom(int count)
		{
			var reports = new Faker<BudgetReport>()
				.StrictMode(true)
				.RuleFor(b => b.ledger_account, f => f.UniqueIndex)
				.RuleFor(b => b.date_from, RandomUtils.RandomDate)
				.RuleFor(b => b.date_to, RandomUtils.RandomDate)
				.RuleFor(b => b.balance_of_ledger_account, f => new Random().Next())
				.RuleFor(b => b.bugeted_amount, f => new Random().Next());

			return reports.Generate(count);

		}

		public static SchemaEntry GetEntry(string name)
		{
			return new SchemaEntry(name, new SchemaFeild[]
			{
				new SchemaFeild.Basic(nameof(ledger_account), "INT", true),
				new SchemaFeild.Basic(nameof(date_from), "DATETIME"),
				new SchemaFeild.Basic(nameof(date_to), "DATETIME"),
				new SchemaFeild.Basic(nameof(balance_of_ledger_account), "FLOAT"),
				new SchemaFeild.Basic(nameof(bugeted_amount), "FLOAT"),

				new SchemaFeild.PrimaryKey(nameof(ledger_account))
			});
		}

		public (string, string) GetId()
		{
			return (nameof(ledger_account), ledger_account.ToString());
		}

		public void OnDeserialize(ref SQLiteDataReader reader)
		{
			ledger_account = reader.GetInt32(0);
			date_from = reader.GetDateTime(1);
			date_to = reader.GetDateTime(2);
			balance_of_ledger_account = reader.GetFloat(3);
			bugeted_amount = reader.GetFloat(4);
		}

		public string[] OnSerialize()
		{
			return new string[]
			{
				ledger_account.ToString(),
				$"{date_from.Year}-{date_from.Month}-{date_from.Day}",
				$"{date_to.Year}-{date_to.Month}-{date_to.Day}",
				balance_of_ledger_account.ToString(),
				bugeted_amount.ToString()
			};
		}
	}
}
