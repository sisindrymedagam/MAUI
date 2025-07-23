using OtpNet;
using Passkeeper.Features.MFACodes.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;

namespace Passkeeper.Features.MFACodes.Services;

public class MFAService : INotifyPropertyChanged
{
    private readonly Timer _timer;
    private readonly ObservableCollection<MFACode> _mfaCodes;
    
    public ObservableCollection<MFACode> MFACodes => _mfaCodes;
    
    public MFAService()
    {
        _mfaCodes = new ObservableCollection<MFACode>();
        _timer = new Timer(UpdateCodes, null, 0, 1000); // Update every second
        LoadSampleData();
        
        // Debug: Check if sample data was loaded
        System.Diagnostics.Debug.WriteLine($"MFAService initialized with {_mfaCodes.Count} codes");
    }
    
    private void UpdateCodes(object? state)
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            foreach (var mfaCode in _mfaCodes)
            {
                UpdateCode(mfaCode);
            }
        });
    }
    
    private void UpdateCode(MFACode mfaCode)
    {
        try
        {
            var totp = new Totp(Base32Encoding.ToBytes(mfaCode.Secret));
            var code = totp.ComputeTotp();
            mfaCode.Code = code;
            
            // Calculate time remaining
            var timeStep = 30; // 30 seconds
            var currentTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            var timeStepSeconds = (long)(currentTime / timeStep) * timeStep;
            var timeRemaining = (int)(timeStep - (currentTime - timeStepSeconds));
            
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
    
    private void LoadSampleData()
    {
        // Sample data for demonstration
        var sampleCodes = new List<MFACode>
        {
            new MFACode { Id = 1, Name = "Google", Issuer = "Google", Secret = "JBSWY3DPEHPK3PXP" },
            new MFACode { Id = 2, Name = "Microsoft", Issuer = "Microsoft", Secret = "JBSWY3DPEHPK3PXP" },
            new MFACode { Id = 3, Name = "GitHub", Issuer = "GitHub", Secret = "JBSWY3DPEHPK3PXP" },
            new MFACode { Id = 4, Name = "Dropbox", Issuer = "Dropbox", Secret = "JBSWY3DPEHPK3PXP" },
            new MFACode { Id = 5, Name = "Steam", Issuer = "Valve", Secret = "JBSWY3DPEHPK3PXP" }
        };
        
        foreach (var code in sampleCodes)
        {
            _mfaCodes.Add(code);
            System.Diagnostics.Debug.WriteLine($"Added sample code: {code.DisplayName}");
        }
        
        System.Diagnostics.Debug.WriteLine($"Total codes loaded: {_mfaCodes.Count}");
    }
    
    public void AddCode(string name, string issuer, string secret)
    {
        var newCode = new MFACode
        {
            Id = _mfaCodes.Count + 1,
            Name = name,
            Issuer = issuer,
            Secret = secret
        };
        
        _mfaCodes.Add(newCode);
        System.Diagnostics.Debug.WriteLine($"Added new code: {newCode.DisplayName}. Total: {_mfaCodes.Count}");
    }
    
    public void RemoveCode(MFACode code)
    {
        _mfaCodes.Remove(code);
    }
    
    public event PropertyChangedEventHandler? PropertyChanged;
    
    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
} 