namespace Passkeeper;

public static class SqliteConnectionFactory
{
    public static ISQLiteAsyncConnection CreateConnection()
    {
        return new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
    }

    public static async Task CreateTables(this SQLiteAsyncConnection connection)
    {
        await connection.CreateTableAsync<MFACodeDto>();
        await connection.CreateTableAsync<PasswordDto>();
    }
}
