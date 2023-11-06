using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Raucse.Extensions;

namespace SQLiteUtils
{
	public static class SQLCommandHelper
	{
		public static string DropIfExists(string tableName) => $"DROP TABLE IF EXISTS {tableName};";
		public static string CreateIfNotExists(string tableName, IEnumerable<(string, string)> feilds)
		{
			string sqlFeilds = feilds.Select(f => $"{f.Item2} {f.Item1}").Concat(", ");
			return $"CREATE TABLE IF NOT EXISTS {tableName} ({sqlFeilds});";
		}

		public static string DropAndCreate(string tableName, IEnumerable<(string, string)> feilds)
		{
			string drop = DropIfExists(tableName);
			string create = CreateIfNotExists(tableName, feilds);
			return $"{drop}\n{create}";
		}

		public static string ReadAll(string tableName) => $"SELECT * FROM {tableName}";

		public static string Clear(string tableName) => $"DELETE FROM {tableName}";
	}
}
