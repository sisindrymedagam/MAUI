using SQLite;

namespace Passkeeper.Features.Password.Services;

public class PasswordStorageService
{
    private readonly SQLiteAsyncConnection _database;

    public PasswordStorageService(string dbPath)
    {
        _database = new SQLiteAsyncConnection(dbPath);
        _database.CreateTableAsync<Models.Password>().Wait();
    }

    public Task<List<Models.Password>> GetPasswordsAsync()
    {
        return _database.Table<Models.Password>().ToListAsync();
    }

    public Task<Models.Password?> GetPasswordAsync(int id)
    {
        return _database.Table<Models.Password>().Where(p => p.Id == id).FirstOrDefaultAsync();
    }

    public Task<int> SavePasswordAsync(Models.Password password)
    {
        return password.Id == 0 ? _database.InsertAsync(password) : _database.UpdateAsync(password);
    }

    public Task<int> DeletePasswordAsync(Models.Password password)
    {
        return _database.DeleteAsync(password);
    }
}