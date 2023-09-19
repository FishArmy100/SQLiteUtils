// See https://aka.ms/new-console-template for more information

using Raucse;
using SQLiteUtils;

const string DB_NAME = "my_database";
string GetInsertCommand(string tableName) =>
	$@"CREATE TABLE IF NOT EXISTS {tableName};
	   INSERT INTO {tableName} (name TEXT, id INTAGER) VALUES ('test guy', 37490);
	   SELECT * FROM {tableName};";

DBHandle handle = new DBHandle(DB_NAME);
DBQueryResult result = handle.ExecuteReadCommand(GetInsertCommand("test_table"));
foreach(var obj in result.Objects)
{
	foreach(var (name, field) in obj)
	{
		ConsoleHelper.WriteMessage($"column name: {name}; field type: {field.Type}; field value: {field.Value}");
	}
}
