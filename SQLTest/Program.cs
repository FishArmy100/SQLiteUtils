using Raucse;
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

var updatedEmployees = new List<TestEmployee>
{
	new TestEmployee("bob", "dylan", 24)
};

string insertCommand = SQLSerializer.Serialize(TABLE_NAME, employees);

handle.ExecuteCommand(insertCommand);

SQLSerializer.DeserializeAll<TestEmployee>(TABLE_NAME, handle).Match(
	ok =>
	{
		foreach (var obj in ok)
		{
			ConsoleHelper.WriteMessage(obj.ToString());
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

