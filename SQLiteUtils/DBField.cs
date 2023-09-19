using Raucse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLiteUtils
{
	public class DBField
	{
		public readonly string Name;
		public readonly FieldType Type;
		public readonly Option<object> Value;

		public DBField(string name, FieldType type, Option<object> value)
		{
			Name = name;
			Type = type;
			Value = value;
		}
	}
}
