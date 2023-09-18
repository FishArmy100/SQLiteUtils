using Raucse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLiteUtils
{
	public class DBFeild
	{
		public readonly string Name;
		public readonly Type Type;
		public readonly Option<object> Value;

		public DBFeild(string name, Type type, Option<object> value)
		{
			Name = name;
			Type = type;
			Value = value;
		}
	}
}
