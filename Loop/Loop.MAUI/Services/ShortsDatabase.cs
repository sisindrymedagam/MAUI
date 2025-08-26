using Loop.MAUI.Models;
using SQLite;

namespace Loop.MAUI.Services;
public class ShortsDatabase
{
    private SQLiteAsyncConnection database;

    private async Task Init()
    {
        if (database is not null)
            return;

        database = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
        CreateTableResult result = await database.CreateTableAsync<ShortsListDto>();
    }

    // Save or update shorts
    public async Task SaveShortsAsync(IEnumerable<ShortsListDto> shorts, bool force = false)
    {
        await Init();

        if (force)
            await database.DeleteAllAsync<ShortsListDto>();

        foreach (ShortsListDto s in shorts)
        {
            await database.InsertOrReplaceAsync(s);
        }
    }

    // Delete shorts by id
    public async Task DeleteShortsAsync(IEnumerable<int> ids)
    {
        await Init();
        foreach (int id in ids)
        {
            await database.DeleteAsync<ShortsListDto>(id);
        }
    }

    // Get all shorts
    public async Task<List<ShortsListDto>> GetShortsAsync()
    {
        await Init();
        return await database.Table<ShortsListDto>().OrderByDescending(s => s.Id).ToListAsync();
    }

    // Get all shorts
    public async Task<int> GetShortsCountAsync()
    {
        await Init();
        return await database.Table<ShortsListDto>().CountAsync();
    }

    // Delete all shorts
    public async Task DeleteAllShortsAsync()
    {
        await database.DeleteAllAsync<ShortsListDto>();
    }
}