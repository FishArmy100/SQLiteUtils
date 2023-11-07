using AccountingDatabaseFrontend;
using Terminal.Gui;

Application.Run<CLIApp>();

Console.WriteLine($"Username: {((CLIApp)Application.Top).usernameText.Text}");

// Before the application exits, reset Terminal.Gui for clean shutdown
Application.Shutdown();
