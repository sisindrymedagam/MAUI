namespace Looply.MAUI;

public static class Constants
{
    public static readonly DateTime MinDateTime = new(2025, 1, 1);

    public const string DatabaseFilename = "loopydb.db3";

    public const SQLite.SQLiteOpenFlags Flags =
        // open the database in read/write mode
        SQLite.SQLiteOpenFlags.ReadWrite |
        // create the database if it doesn't exist
        SQLite.SQLiteOpenFlags.Create |
        // enable multi-threaded database access
        SQLite.SQLiteOpenFlags.SharedCache;

    public static string DatabasePath => Path.Combine(FileSystem.AppDataDirectory, DatabaseFilename);

    public const string TokenName = "AuthToken";
    public const string TokenExpirationName = "AuthTokenExpiration";

    public const string UserEmailName = "UserEmail";
    public const string LastSyncUtcName = "LastSyncUtc";
}
