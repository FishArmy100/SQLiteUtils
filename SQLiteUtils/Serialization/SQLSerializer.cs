using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Raucse.Extensions;
using Raucse;

namespace SQLiteUtils.Serialization
{
	public static class SQLSerializer
	{
		public static string Serialize<T>(string tableName, IEnumerable<T> items) where T : ISQLSerializeable
		{
			string s = string.Empty;
			foreach (T item in items)
			{
				string values = item.OnSerialize().Concat(", ");
				s += $"INSERT INTO {tableName} VALUES ({values});\n";
			}

			return s;
		}

		public static Option<SQLiteException> Serialize<T>(string tableName, DBHandle handle, IEnumerable<T> items) where T : ISQLSerializeable
		{
			string command = Serialize(tableName, items);
			try
			{
				handle.ExecuteCommand(command);
				return new Option<SQLiteException>();
			}
			catch(SQLiteException e)
			{
				return new Option<SQLiteException>(e);
			}
		}

		public static Option<SQLiteException> Update<T>(string tableName, DBHandle handle, IEnumerable<T> items)
			where T : ISQLUpdateable
		{
			string commands = "";
			foreach(T item in items)
			{
				var (name, value) = item.GetId();
				string delete = $"DELETE FROM {tableName} WHERE {name} = {value};";
				string serialize = Serialize(tableName, items);
				commands += $"{delete}\n{serialize}";
			}

			try
			{
				handle.ExecuteCommand(commands);
				return new Option<SQLiteException>();
			}
			catch(SQLiteException e)
			{
				return new Option<SQLiteException>(e);
			}
		}

		public static Result<List<T>, SQLiteException> Deserialize<T>(string selector, DBHandle handle) 
			where T : ISQLSerializeable, new()
		{
			return handle.ExecuteReadCommand(selector, reader =>
			{
				T item = new T();
				item.OnDeserialize(ref reader);
				return item;
			});
		}

		public static Result<List<T>, SQLiteException> DeserializeAll<T>(string tableName, DBHandle handle)
			where T : ISQLSerializeable, new()
		{
			return Deserialize<T>(SQLCommandHelper.ReadAll(tableName), handle);
		}
	}
}
