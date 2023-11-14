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
    public class Check : ISQLUpdateable
    {
        public int check_number;
        public float amount;
        public DateTime date;

        public Check() { }

        public Check(int check_number, float amount, DateTime date)
        {
            this.check_number = check_number;
            this.amount = amount;
            this.date = date;
        }

        public static List<Check> GenerateRandom(int count)
        {
            var checks = new Faker<Check>()
                .StrictMode(true)
                .RuleFor(c => c.check_number, f => f.IndexGlobal)
                .RuleFor(c => c.amount, f => new Random().Next())
                .RuleFor(c => c.date, RandomUtils.RandomDate);

            return checks.Generate(count);
        }

        public static SchemaEntry GetEntry(string name)
        {
            return new SchemaEntry(name, new SchemaFeild[]
            {
                new SchemaFeild.Basic(nameof(check_number), "INT", true),
                new SchemaFeild.Basic(nameof(amount), "FLOAT"),
                new SchemaFeild.Basic(nameof(date), "DATETIME"),

                new SchemaFeild.PrimaryKey(nameof(check_number)),
            });
        }

        public (string, string) GetId()
        {
            return (nameof(check_number), check_number.ToString());
        }

        public void OnDeserialize(ref SQLiteDataReader reader)
        {
            check_number = reader.GetInt32(0);
            amount = reader.GetFloat(1);
            date = reader.GetDateTime(2);
        }

        public string[] OnSerialize()
        {
            return new string[]
            {
                check_number.ToString(),
                amount.ToString(),
                $"{date.Year}-{date.Month}-{date.Day}"
            };
        }
    }
}
