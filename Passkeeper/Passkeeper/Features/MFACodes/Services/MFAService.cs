using OtpNet;
using SQLite;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Passkeeper.Features.MFACodes.Services;

public class MFAService : INotifyPropertyChanged
{
    private readonly Timer _timer;
    private readonly ObservableCollection<MFACode> _mfaCodes;
    private readonly SQLiteAsyncConnection _database;
    private bool _isInitialized = false;

    public ObservableCollection<MFACode> MFACodes => _mfaCodes;

    public MFAService()
    {
        _database = new SQLiteAsyncConnection(Constants.DatabasePath);
        _mfaCodes = [];
        _timer = new Timer(UpdateCodes, null, 0, 1000); // Update every second
    }

    private async Task InitializeAsync()
    {
        await _database.CreateTableAsync<MFACode>();
        await LoadCodesFromDatabase();
        _isInitialized = true;
    }

    private async Task LoadCodesFromDatabase()
    {
        try
        {
            var codes = await _database.Table<MFACode>().ToListAsync();
            MainThread.BeginInvokeOnMainThread(() =>
            {
                _mfaCodes.Clear();
                foreach (var code in codes)
                {
                    _mfaCodes.Add(code);
                }
            });
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error loading MFA codes: {ex.Message}");
        }
    }

    private void UpdateCodes(object? state)
    {
        if (!_isInitialized) return;

        MainThread.BeginInvokeOnMainThread(() =>
        {
            foreach (MFACode mfaCode in _mfaCodes)
            {
                UpdateCode(mfaCode);
            }
        });
    }

    private void UpdateCode(MFACode mfaCode)
    {
        try
        {
            Totp totp = new(Base32Encoding.ToBytes(mfaCode.Secret));
            string code = totp.ComputeTotp();
            mfaCode.Code = code;

            // Calculate time remaining
            int timeStep = 30; // 30 seconds
            long currentTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            long timeStepSeconds = currentTime / timeStep * timeStep;
            int timeRemaining = (int)(timeStep - (currentTime - timeStepSeconds));

            if (timeRemaining <= 0)
                timeRemaining = timeStep;

            mfaCode.TimeRemaining = timeRemaining;
        }
        catch (Exception)
        {
            // Handle invalid secret
            mfaCode.Code = "------";
            mfaCode.TimeRemaining = 30;
        }
    }

    public async Task<int> SaveMFACodeAsync(MFACode mfaCode)
    {
        if (!_isInitialized)
        {
            await InitializeAsync();
        }
        var result = mfaCode.Id == 0 ? await _database.InsertAsync(mfaCode) : await _database.UpdateAsync(mfaCode);
        if (mfaCode.Id == 0)
        {
            mfaCode.Id = result;
            MainThread.BeginInvokeOnMainThread(() => _mfaCodes.Add(mfaCode));
        }
        return result;
    }

    public async Task<int> DeleteMFACodeAsync(MFACode mfaCode)
    {
        if (!_isInitialized)
        {
            await InitializeAsync();
        }
        var result = await _database.DeleteAsync(mfaCode);
        MainThread.BeginInvokeOnMainThread(() => _mfaCodes.Remove(mfaCode));
        return result;
    }

    public async Task<List<MFACode>> GetMFACodesAsync()
    {
        if (!_isInitialized)
        {
            await InitializeAsync();
        }
        return await _database.Table<MFACode>().ToListAsync();
    }

    public async Task<MFACode?> GetMFACodeAsync(int id)
    {
        if (!_isInitialized)
        {
            await InitializeAsync();
        }
        return await _database.Table<MFACode>().Where(m => m.Id == id).FirstOrDefaultAsync();
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}