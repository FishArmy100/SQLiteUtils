using SQLiteUtils.Schema;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLiteUtils.Serialization
{
	public interface ISQLSerializeable
	{
		static abstract SchemaEntry GetEntry(string name);
		string[] OnSerialize();
		void OnDeserialize(ref SQLiteDataReader reader);
	}
}
