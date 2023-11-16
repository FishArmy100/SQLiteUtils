using System.Data.SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Raucse;
using SQLiteUtils.Schema;
using Raucse.Extensions;
using ConsoleTables;

namespace SQLiteUtils
{
	public class DBHandle
	{
		public readonly string Name;
		private readonly SQLiteConnection m_Connection;

		public DBHandle(string name)
		{
			Name = name;
			m_Connection = GetConnection(name);
		}

		public DBHandle(string name, SQLiteConnection connection)
		{
			Name = name;
			m_Connection = connection;
		}

		public void BuildSchema(DBSchema schema)
		{
			string commands = "";
			foreach(SchemaEntry entry in schema.Entries)
			{
				string deleteCommand = SQLCommandHelper.DropIfExists(entry.Name);
				string createCommand = SQLCommandHelper.GenerateSchemaEntry(entry);
				commands += $"{deleteCommand}\n{createCommand}\n";
			}
			
			this.ExecuteCommand(commands);
		}

		public int ExecuteCommand(string commandString)
		{
			m_Connection.Open();

			using SQLiteCommand command = m_Connection.CreateCommand();
			command.CommandText = commandString;
			int changedRows = command.ExecuteNonQuery();
			m_Connection.Close();

			return changedRows;
		}

		public Result<ConsoleTable, SQLiteException> DebugRead(string commandString)
		{
			m_Connection.Open();
			using SQLiteCommand command = m_Connection.CreateCommand();
			command.CommandText = commandString;
			try
			{
				using SQLiteDataReader reader = command.ExecuteReader();


				List<string> columns = new List<string>();
				for (int i = 0; i < reader.FieldCount; i++)
				{
					columns.Add(reader.GetName(i).ToUpper());
				}


				ConsoleTable table = new ConsoleTable(columns.ToArray());
				while (reader.Read())
				{
					List<string> row = new List<string>();
					for (int i = 0; i < reader.FieldCount; i++)
					{
						string value = reader.GetValue(i) switch
						{
							string s => $"\"{s}\"",
							object o => o.ToString()!,
							null => "null"
						};

						row.Add(value);
					}

					table.AddRow(row.Select(v => v as object).ToArray());
				}

				return table;
			}
			catch (SQLiteException e)
			{
				return e;
			}
		}

		public Result<List<T>, SQLiteException> ExecuteReadCommand<T>(string commandString, Func<SQLiteDataReader, T> onRead)
		{
			m_Connection.Open();
			using SQLiteCommand command = m_Connection.CreateCommand();
			command.CommandText = commandString;
			try
			{
				using SQLiteDataReader reader = command.ExecuteReader();
				List<T> objs = new List<T>();
				while (reader.Read())
				{
					objs.Add(onRead(reader));
				}

				return objs;
			}
			catch (SQLiteException e)
			{
				return e;
			}
			finally
			{
				m_Connection.Close();
			}
		}

		private static SQLiteConnection GetConnection(string databaseName)
		{
			string location = Assembly.GetExecutingAssembly().Location;
			string? folder = Path.GetDirectoryName(location);
			string uri = $"{folder}\\{databaseName}.db";
			return new SQLiteConnection($"URI=file:{uri}");
		}
	}
}
