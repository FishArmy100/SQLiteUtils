using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLiteUtils.Schema
{
	public abstract class SchemaFeild
	{
		public class Basic : SchemaFeild
		{
			public readonly string Name;
			public readonly string SQLType;
			public readonly bool IsNotNull;

			public Basic(string name, string sqlType, bool isNotNull = false)
			{
				Name = name;
				SQLType = sqlType;
				IsNotNull = isNotNull;
			}
		}

		public class PrimaryKey : SchemaFeild
		{ 
			public readonly string Name;

			public PrimaryKey(string name)
			{
				Name = name;
			}
		}

		public class ForeignKey : SchemaFeild
		{
			public readonly string Name;
			public readonly string ForiegnName;
			public readonly string ForeignTableName;

			public ForeignKey(string name, string foriegnName, string foreignTableName)
			{
				Name = name;
				ForiegnName = foriegnName;
				ForeignTableName = foreignTableName;
			}
		}
	}
}
