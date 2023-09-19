using System.Data.SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLiteUtils
{
	public class DBQueryResult
	{
		public readonly IReadOnlyList<IReadOnlyDictionary<string, DBField>> Objects;

		public DBQueryResult(SQLiteDataReader reader)
		{
			List<IReadOnlyDictionary<string, DBField>> objs = new List<IReadOnlyDictionary<string, DBField>>();
			while (reader.Read())
			{
				Dictionary<string, DBField> obj = new Dictionary<string, DBField>();
				for (int ordinal = 0; ordinal < reader.FieldCount; ordinal++)
				{
					string name = reader.GetName(ordinal);
					var (type, value) = FieldTypeUtils.ReadValue(reader, ordinal).Value;
					DBField field = new DBField(name, type, value);
					obj.Add(name, field);
				}
				objs.Add(obj);
			}

			Objects = objs;
		}
	}
}
