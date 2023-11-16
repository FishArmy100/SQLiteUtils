using AccountingDatabaseBackend;
using ConsoleTables;
using Raucse;
using Raucse.Strings;
using SQLiteUtils;
using SQLiteUtils.Schema;
using SQLiteUtils.Serialization;
using SQLTest;

const string DB_NAME = "my_database";

DBHandle handle = new DBHandle(DB_NAME);
ApplicationSchema.InitTestDB(handle);


handle.DebugRead(SQLCommandHelper.ReadAll(DBTableNames.EMPLOYEE_TABLE_NAME)).Match(
	ok =>
	{
		using TextWriter writer = new StringWriter();
		ok.Options.OutputTo = writer;
		ok.Write(Format.Alternative);
		ConsoleHelper.WriteMessage(writer.ToString());
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

