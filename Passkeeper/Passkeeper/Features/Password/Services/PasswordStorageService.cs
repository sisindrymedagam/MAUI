using SQLite;

namespace Passkeeper.Features.Password.Services;

public class PasswordStorageService
{
    private readonly SQLiteAsyncConnection _database;
    private bool _isInitialized = false;

    public PasswordStorageService()
    {
        _database = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
    }

    private async Task InitializeAsync()
    {
        if (_isInitialized)
            return;
        await _database.CreateTableAsync<Models.PasswordDto>();
        _isInitialized = true;
    }

    public async Task<List<Models.PasswordDto>> GetListAsync()
    {
        await InitializeAsync();
        return await _database.Table<Models.PasswordDto>().ToListAsync();
    }

    public async Task<Models.PasswordDto?> GetAsync(int id)
    {
        await InitializeAsync();
        return await _database.Table<Models.PasswordDto>().Where(p => p.Id == id).FirstOrDefaultAsync();
    }

    public async Task<int> SaveAsync(Models.PasswordDto item)
    {
        await InitializeAsync();
        return await (item.Id == 0 ? _database.InsertAsync(item) : _database.UpdateAsync(item));
    }

    public async Task<int> DeleteAsync(Models.PasswordDto item)
    {
        await InitializeAsync();
        return await _database.DeleteAsync(item);
    }
}