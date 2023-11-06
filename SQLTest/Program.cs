﻿using Raucse;
using Raucse.Strings;
using SQLiteUtils;
using SQLiteUtils.Schema;
using SQLiteUtils.Serialization;
using SQLTest;

const string DB_NAME = "my_database";
const string TABLE_NAME = "test_table";

DBHandle handle = new DBHandle(DB_NAME);

SchemaBuilder builder = new SchemaBuilder("my_schema");
builder.AddEntry<TestEmployee>(TABLE_NAME);
DBSchema schema = builder.Build();

handle.BuildSchema(schema);

var employees = new List<TestEmployee>
{
	new TestEmployee("bob", "dylen", 24),
	new TestEmployee("issac", "rob", 32),
	new TestEmployee("robbert", "grotten", 53)
};

string insertCommand = SQLSerializer.Serialize(TABLE_NAME, employees);

handle.ExecuteCommand(insertCommand);

handle.ExecuteReadCommand(SQLCommandHelper.ReadAll(TABLE_NAME), reader =>
	{
		string objString = "";
		for(int ordinal = 0; ordinal < reader.FieldCount; ordinal++)
		{
			objString += $"{reader.GetName(ordinal)}: `{reader.GetValue(ordinal)}`; ";
		}

		return objString;
	}).Match(
	ok =>
	{
		foreach (var obj in ok)
		{
			ConsoleHelper.WriteMessage(obj);
		}
	},
	fail =>
	{
		StringMaker maker = new StringMaker();
		maker.AppendLine("Error:");
		maker.TabIn(TabModes.Bulleted);
		maker.AppendLines(fail.Message.Split('\n'));
		maker.TabOut();
		ConsoleHelper.WriteError(maker.ToString());
	});

