using Bogus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountingDatabaseBackend
{
    public static class RandomUtils
    {
        public static DateTime RandomDate(Faker f)
        {
            return f.Date.BetweenDateOnly(DateOnly.MinValue, DateOnly.MaxValue).ToDateTime(TimeOnly.MinValue);
        }

        public static T RandomFrom<T>(this List<T> self, Faker f)
        {
            return self[f.Random.Int(0, self.Count)];
        }
    }
}
