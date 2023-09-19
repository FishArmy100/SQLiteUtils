using System.Data.SQLite;
using Raucse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

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
		Bool,
		DateTime,
	}

	public static class FieldTypeUtils
	{
		public static Option<(FieldType, Option<object>)> ReadValue(SQLiteDataReader reader, int ordinal)
		{
			string typeName = reader.GetDataTypeName(ordinal);
			return FromSQLTypeName(typeName).Match(
				ok =>
				{
					if (reader.IsDBNull(ordinal))
						return (ok, new Option<object>());

					object value = ok switch
					{
						FieldType.Int => reader.GetInt32(ordinal),
						FieldType.Long => reader.GetInt64(ordinal),
						FieldType.ULong => reader.GetFieldValue<ulong>(ordinal),
						FieldType.String => reader.GetString(ordinal),
						FieldType.Float => reader.GetFloat(ordinal),
						FieldType.Double => reader.GetDouble(ordinal),
						FieldType.Bool => reader.GetBoolean(ordinal),
						FieldType.DateTime => reader.GetDateTime(ordinal),
						_ => throw new NotImplementedException(),
					};

					return (ok, value);
				},
				() =>
				{
					return new Option<(FieldType, Option<object>)>();
				});
		}
		public static Option<FieldType> FromCSType(Type type)
		{
			return Type.GetTypeCode(type) switch
			{
				TypeCode.Empty => throw new NotImplementedException(),
				TypeCode.Object => throw new NotImplementedException(),
				TypeCode.DBNull => throw new NotImplementedException(),
				TypeCode.Boolean => FieldType.Bool,
				TypeCode.Char => FieldType.String,
				TypeCode.SByte => FieldType.Int,
				TypeCode.Byte => FieldType.Int,
				TypeCode.Int16 => FieldType.Int,
				TypeCode.UInt16 => FieldType.Int,
				TypeCode.Int32 => FieldType.Int,
				TypeCode.UInt32 => FieldType.Int,
				TypeCode.Int64 => FieldType.Long,
				TypeCode.UInt64 => FieldType.ULong,
				TypeCode.Single => FieldType.Float,
				TypeCode.Double => FieldType.Double,
				TypeCode.Decimal => throw new NotImplementedException(),
				TypeCode.DateTime => FieldType.DateTime,
				TypeCode.String => FieldType.String,
				_ => throw new NotImplementedException(),
			};
		}

		public static Option<FieldType> FromSQLTypeName(string typeName)
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
					 sqlTypeName == "NUMERIC" ||
					 sqlTypeName.StartsWith("DECIMAL"))
			{
				return FieldType.Double;
			}
			else if (sqlTypeName == "FLOAT")
			{
				return FieldType.Float;
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

		public static string ToSQLTypeName(this FieldType fieldType)
		{
			return fieldType switch
			{
				FieldType.Int => "INT",
				FieldType.Long => "BIGINT",
				FieldType.ULong => "UNSIGNED BIG INT",
				FieldType.String => "TEXT",
				FieldType.Float => "FLOAT",
				FieldType.Double => "DOUBLE",
				FieldType.Bool => "BOOLEAN",
				FieldType.DateTime => "DATETIME",
				_ => throw new NotImplementedException(),
			};

		}
	}
}
