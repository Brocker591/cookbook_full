namespace Cookbook.App.Repositories;

public class DatabaseSetting
{
    public string DatabasePath { get; init; }


    public SQLite.SQLiteOpenFlags Flags =
        // open the database in read/write mode
        SQLite.SQLiteOpenFlags.ReadWrite |
        // create the database if it doesn't exist
        SQLite.SQLiteOpenFlags.Create |
        // enable multi-threaded database access
        SQLite.SQLiteOpenFlags.SharedCache;

    public DatabaseSetting(string appDataDirectory)
    {

        DatabasePath = Path.Combine(appDataDirectory, Settings.DatabaseFilename);
    }
}
