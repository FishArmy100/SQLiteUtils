using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SQLiteUtils
{
	public class DBHandle
	{
		public readonly string Name;
		private readonly SqliteConnection m_Connection;

		public DBHandle(string name)
		{
			Name = name;
			m_Connection = GetConnection(name);
		}

		public DBHandle(string name, SqliteConnection connection)
		{
			Name = name;
			m_Connection = connection;
		}

		private static SqliteConnection GetConnection(string databaseName)
		{
			string location = Assembly.GetExecutingAssembly().Location;
			string? folder = Path.GetDirectoryName(location);
			string uri = $"{folder}\\{databaseName}.db";
			return new SqliteConnection($"URI=file:{uri}");
		}
	}
}
