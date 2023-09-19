using System.Data.SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Raucse;

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

		public int ExecuteCommand(string commandString)
		{
			m_Connection.Open();

			using SQLiteCommand command = m_Connection.CreateCommand();
			command.CommandText = commandString;
			int changedRows = command.ExecuteNonQuery();
			m_Connection.Close();

			return changedRows;
		}

		public Result<DBQueryResult, SQLiteException> ExecuteReadCommand(string commandString)
		{
			m_Connection.Open();
			using SQLiteCommand command = m_Connection.CreateCommand();
			command.CommandText = commandString;
			try
			{
				using SQLiteDataReader reader = command.ExecuteReader();
				DBQueryResult result = new DBQueryResult(reader);
				return result;
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
