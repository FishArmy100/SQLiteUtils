using SQLiteUtils.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SQLiteUtils.Schema
{
	public class SchemaBuilder
	{
		private string m_Name;
		private List<SchemaEntry> m_SchemaEnries = new List<SchemaEntry>();
		public SchemaBuilder(string name) 
		{
			m_Name = name;
		}

		public void AddEntry<T>(string name) where T : ISQLSerializeable
		{
			m_SchemaEnries.Add(T.GetEntry(name));
		}

		public void AddEntry(SchemaEntry entry)
		{
			m_SchemaEnries.Add(entry);
		}

		public void AddEntry(string name, IEnumerable<SchemaFeild> feilds)
		{
			SchemaEntry entry = new SchemaEntry(name, feilds.ToArray());
			m_SchemaEnries.Add(entry);
		}

		public DBSchema Build()
		{
			return new DBSchema(m_Name, m_SchemaEnries.ToArray());
		}
	}
}
