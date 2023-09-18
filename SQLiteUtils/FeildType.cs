using Microsoft.Data.Sqlite;
using Raucse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLiteUtils
{
	public enum FieldType
	{
		Int,
		Long,
		ULong,
		String,
		Float,
		Double,
		Decimal,
		Bool,
		DateTime,
	}

	public static class FeildTypeUtils
	{
		internal static Option<(FieldType, object)> ReadValue(ref SqliteDataReader reader, int ordinal)
		{
			string typeName = reader.GetDataTypeName(ordinal);
			return FromName(typeName).Match(
				ok =>
				{
					object value = ok switch { };
					return new Option<(FieldType, object)>((ok, value));
				},
				() =>
				{
					return new Option<(FieldType, object)>();
				});
		}

		internal static Option<FieldType> FromName(string typeName)
		{
			string sqlTypeName = typeName.ToUpper();
			if (sqlTypeName == "INT"		||
				sqlTypeName == "INTAGER"	||
				sqlTypeName == "TINYINT"	||
				sqlTypeName == "SMALLINT"	||
				sqlTypeName == "MEDIUMINT"	||
				sqlTypeName == "INT2"		||
				sqlTypeName == "INT8")
			{
				return FieldType.Int;
			}
			else if (sqlTypeName == "BIGINT")
			{
				return FieldType.Long;
			}
			else if (sqlTypeName == "UNSIGNED BIG")
			{
				return FieldType.ULong;
			}
			else if (sqlTypeName.StartsWith("CHAR") ||
					 sqlTypeName.StartsWith("VAR") ||
					 sqlTypeName.StartsWith("NATIVE") ||
					 sqlTypeName.StartsWith("NCHAR") ||
					 sqlTypeName.StartsWith("TEXT") ||
					 sqlTypeName.StartsWith("NVAR") ||
					 sqlTypeName.StartsWith("CLOB"))
			{
				return FieldType.String;
			}
			else if (sqlTypeName == "DOUBLE" ||
					 sqlTypeName == "PRECISION" ||
					 sqlTypeName == "REAL" ||
					 sqlTypeName == "NUMERIC")
			{
				return FieldType.Double;
			}
			else if (sqlTypeName == "FLOAT")
			{
				return FieldType.Float;
			}
			else if (sqlTypeName.StartsWith("DECIMAL"))
			{
				return FieldType.Decimal;
			}
			else if (sqlTypeName == "BOOLEAN")
			{
				return FieldType.Bool;
			}
			else if (sqlTypeName.StartsWith("DATE"))
			{
				return FieldType.DateTime;
			}
			else
			{
				return new Option<FieldType>();
			}
		}
	}
}
