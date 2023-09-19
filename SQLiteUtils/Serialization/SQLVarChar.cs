using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLiteUtils.Serialization
{
	[AttributeUsage(AttributeTargets.Field)]
	public class SQLVarChar : Attribute
	{
		public readonly uint Size;

		public SQLVarChar(uint size)
		{
			Size = size;
		}
	}
}
