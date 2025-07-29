using SQLite;

namespace Passkeeper.Features.Password.Services;

public class PasswordStorageService
{
    private readonly SQLiteAsyncConnection _database;
    private bool _isInitialized = false;

    public PasswordStorageService(string dbPath)
    {
        _database = new SQLiteAsyncConnection(dbPath);
        // Initialize database asynchronously
        _ = InitializeAsync();
    }

    private async Task InitializeAsync()
    {
        await _database.CreateTableAsync<Models.Password>();
        _isInitialized = true;
    }

    public async Task<List<Models.Password>> GetPasswordsAsync()
    {
        if (!_isInitialized)
        {
            await InitializeAsync();
        }
        return await _database.Table<Models.Password>().ToListAsync();
    }

    public async Task<Models.Password?> GetPasswordAsync(int id)
    {
        if (!_isInitialized)
        {
            await InitializeAsync();
        }
        return await _database.Table<Models.Password>().Where(p => p.Id == id).FirstOrDefaultAsync();
    }

    public async Task<int> SavePasswordAsync(Models.Password password)
    {
        if (!_isInitialized)
        {
            await InitializeAsync();
        }
        return await (password.Id == 0 ? _database.InsertAsync(password) : _database.UpdateAsync(password));
    }

    public async Task<int> DeletePasswordAsync(Models.Password password)
    {
        if (!_isInitialized)
        {
            await InitializeAsync();
        }
        return await _database.DeleteAsync(password);
    }
}