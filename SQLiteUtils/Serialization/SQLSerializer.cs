using Raucse;
using Raucse.Strings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SQLiteUtils.Serialization
{
	public static class SQLSerializer
	{
		private class FieldData
		{
			public readonly string Name;
			public readonly FieldType Type;
			public readonly Option<uint> CustomStringCapacity;
			public readonly object Value;

			private FieldData(string name, FieldType type, Option<uint> customStrCap, object value)
			{
				Name = name;
				Type = type;
				CustomStringCapacity = customStrCap;
				Value = value;
			}

			public static FieldData FromInfo(FieldInfo info, object parent)
			{
				var name = info.Name;
				var type = FieldTypeUtils.FromCSType(info.FieldType).Value;
				var attrib = info.GetCustomAttribute<SQLVarChar>();
				Option<uint> customStrCap = new Option<uint>();
				if (attrib is not null)
					customStrCap = attrib.Size;

				var value = info.GetValue(parent)!;
				return new FieldData(name, type, customStrCap, value);
			}
		}

		public static string GetSerializeCommands<T>(string tableName, IEnumerable<T> data)
		{
			CheckType<T>();

			List<List<FieldData>> dataFeilds = GetTableFields(data);
			string[] tableColumns = GetTableColumns<T>();
			string commands = string.Empty;
			commands += string.Join("\n", dataFeilds.Select(d => CreateCommandString(tableName, d)));

			return commands;
		}

		private static string[] GetTableColumns<T>()
		{
			return typeof(T).GetFields()
					.Where(f => f.GetCustomAttribute<SQLSerializableField>() != null)
					.Select(f => $"{f.Name} {FieldTypeUtils.FromCSType(f.FieldType).Value.ToSQLTypeName()}")
					.ToArray();
		}

		private static List<List<FieldData>> GetTableFields<T>(IEnumerable<T> data)
		{
			return data.Select(obj =>
							obj!.GetType()
							.GetFields()
							.Where(f => f.GetCustomAttribute<SQLSerializableField>() != null)
							.Select(f => FieldData.FromInfo(f, obj)).ToList())
							.ToList();
		}

		private static string CreateCommandString(string tableName, IEnumerable<FieldData> fields)
		{
			string[] valueNames = fields.Select(f => f.Name).ToArray();
			string[] values = fields.Select(f => CreateValueString(f.Value, f.Type)).ToArray();

			string command = $"INSERT INTO {tableName} ({string.Join(", ", valueNames)}) VALUES ({string.Join(", ", values)});";
			return command;
		}

		private static string CreateValueString(object value, FieldType type)
		{
			return type switch
			{
				FieldType.Int => $"'{(int)value}'",
				FieldType.Long => $"'{(long)value}'",
				FieldType.ULong => $"'{(ulong)value}'",
				FieldType.String => $"'{(string)value}'",
				FieldType.Float => $"'{(float)value}'",
				FieldType.Double => $"'{(double)value}'",
				FieldType.Bool => (bool)value ? "1" : "0",
				FieldType.DateTime => throw new NotImplementedException(),
				_ => throw new NotImplementedException(),
			};
		}

		private static void CheckType<T>()
		{
			if (typeof(T).GetCustomAttribute<SQLSerializableObject>() == null)
				throw new ArgumentException("Type '" + typeof(T).Name + "', must have the SQLSerializeableObject attribute.");
		}
	}
}
