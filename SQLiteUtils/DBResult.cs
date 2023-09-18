using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLiteUtils
{
	public class DBResult
	{
		public readonly IReadOnlyList<IReadOnlyDictionary<string, DBFeild>> Objects;

		public DBResult(SqliteDataReader reader)
		{
			while (reader.Read())
			{
				Dictionary<string, DBFeild> obj = new Dictionary<string, DBFeild>();
				for (int ordinal = 0; ordinal < reader.FieldCount; ordinal++)
				{
					string name = reader.GetName(ordinal);
					
				}
			}
		}

		private static Type GetTypeFromSqlType(string sqlType)
		{
			string typeText = sqlType.ToLower();

			bool wasInt
		}
	}
}
