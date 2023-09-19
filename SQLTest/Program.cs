using Raucse;
using Raucse.Strings;
using SQLiteUtils;

const string DB_NAME = "my_database";

static string GetInsertCommand(string tableName) =>
	$"CREATE TABLE IF NOT EXISTS {tableName} (name TEXT, id INTAGER);\n" +
	$"INSERT INTO {tableName} (name, id) VALUES ('test guy', 37490);\n" +
	$"SELECT * FROM {tableName}";

DBHandle handle = new DBHandle(DB_NAME);
string command = GetInsertCommand("test_table");
handle.ExecuteReadCommand(command).Match(
	ok =>
	{
		foreach (var obj in ok.Objects)
		{
			foreach (var (name, field) in obj)
			{
				ConsoleHelper.WriteMessage($"column name: {name}; field type: {field.Type}; field value: {field.Value.Value}");
			}
		}
	},
	fail =>
	{
		StringMaker maker = new StringMaker();
		maker.AppendLine("Source: ");
		maker.TabIn(TabModes.Number);
		maker.AppendLines(command.Split('\n'));
		maker.TabOut();
		maker.AppendLine("Error:");
		maker.TabIn(TabModes.Bulleted);
		maker.AppendLines(fail.Message.Split('\n'));
		maker.TabOut();
		ConsoleHelper.WriteError(maker.ToString());
	});

