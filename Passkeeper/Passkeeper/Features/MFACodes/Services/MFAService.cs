using SQLite;

namespace Passkeeper.Features.MFACodes.Services;

public class MFAService
{
    private readonly SQLiteAsyncConnection _database;
    private bool _isInitialized = false;

    public MFAService()
    {
        _database = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
    }

    private async Task InitializeAsync()
    {
        if (_isInitialized)
            return;
        await _database.CreateTableAsync<MFACodeDto>();
        _isInitialized = true;
    }

    public async Task<List<MFACodeDto>> GetListAsync()
    {
        await InitializeAsync();
        return await _database.Table<MFACodeDto>().ToListAsync();
    }

    public async Task<MFACodeDto?> GetAsync(int id)
    {
        await InitializeAsync();
        return await _database.Table<MFACodeDto>().Where(p => p.Id == id).FirstOrDefaultAsync();
    }

    public async Task<int> SaveAsync(MFACodeDto item)
    {
        await InitializeAsync();
        return await (item.Id == 0 ? _database.InsertAsync(item) : _database.UpdateAsync(item));
    }

    public async Task<int> DeleteAsync(MFACodeDto item)
    {
        await InitializeAsync();
        return await _database.DeleteAsync(item);
    }
}