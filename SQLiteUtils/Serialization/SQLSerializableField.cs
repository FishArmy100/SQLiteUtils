using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLiteUtils.Serialization
{
	[AttributeUsage(AttributeTargets.Field)]
	public class SQLSerializableField : Attribute
	{
		public readonly string? CustomName = null;

		public SQLSerializableField() { }
	}
}
