using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Raucse.Extensions;

namespace SQLiteUtils.Serialization
{
	public static class SQLSerializer
	{
		public static string Serialize<T>(string tableName, IEnumerable<T> items) where T : ISQLSerializeable, new()
		{
			string s = string.Empty;
			foreach (T item in items)
			{
				string values = item.OnSerialize().Concat(", ");
				s += $"INSERT INTO {tableName} VALUES ({values});\n";
			}

			return s;
		}
	}
}
