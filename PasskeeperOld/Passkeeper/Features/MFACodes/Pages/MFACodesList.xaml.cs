using Passkeeper.Features.MFACodes.Services;
using Passkeeper.Features.MFACodes.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Passkeeper.Features.MFACodes.Pages;

public partial class MFACodesList : ContentPage
{
    private readonly MFAService _mfaService;
    
    public MFAService MFAService => _mfaService;
    
    // Direct property for easier binding
    public ObservableCollection<MFACode> MFACodes => _mfaService.MFACodes;
    
    public ICommand? RefreshCommand { get; }
    
    bool isRefreshing;
    public bool IsRefreshing
    {
        get { return isRefreshing; }
        set
        {
            isRefreshing = value;
            OnPropertyChanged();
        }
    }
    
    public MFACodesList()
    {
        InitializeComponent();
        _mfaService = new MFAService();
        RefreshCommand = new Command(async () => await RefreshAsync());
        BindingContext = this;
        
        // Debug: Check if codes are loaded
        System.Diagnostics.Debug.WriteLine($"MFACodes count: {_mfaService.MFACodes.Count}");
    }
    
    // Override the back button/gesture
    protected override bool OnBackButtonPressed()
    {
        if (AddCodePanel.IsVisible)
        {
            AddCodePanel.Hide();
            return true;
        }
        return base.OnBackButtonPressed();
    }
    
    private void HideTabBar(object sender, EventArgs e)
    {
        // Hide tab bar when bottom sheet is shown
        if (Parent is Shell shell)
        {
            shell.FlyoutBehavior = FlyoutBehavior.Disabled;
        }
    }
    
    private void ShowTabBar(object sender, EventArgs e)
    {
        // Show tab bar when bottom sheet is hidden
        if (Parent is Shell shell)
        {
            shell.FlyoutBehavior = FlyoutBehavior.Disabled;
        }
    }
    
    private void OnAddCodeClicked(object sender, EventArgs e)
    {
        AddCodePanel.Show();
    }
    
    private void OnCancelAdd(object sender, EventArgs e)
    {
        AddCodePanel.Hide();
        ClearAddForm();
    }
    
    private void OnConfirmAdd(object sender, EventArgs e)
    {
        var name = NameEntry.Text?.Trim();
        var issuer = IssuerEntry.Text?.Trim();
        var secret = SecretEntry.Text?.Trim();
        
        if (string.IsNullOrWhiteSpace(name))
        {
            DisplayAlert("Error", "Please enter an account name.", "OK");
            return;
        }
        
        if (string.IsNullOrWhiteSpace(secret))
        {
            DisplayAlert("Error", "Please enter a secret key.", "OK");
            return;
        }
        
        // Add the new code
        _mfaService.AddCode(name, issuer ?? "", secret);
        
        // Debug: Check if code was added
        System.Diagnostics.Debug.WriteLine($"Added code. Total count: {_mfaService.MFACodes.Count}");
        
        // Hide the panel and clear the form
        AddCodePanel.Hide();
        ClearAddForm();
        
        // Show success message
        DisplayAlert("Success", "Authenticator code added successfully!", "OK");
    }
    
    private void ClearAddForm()
    {
        NameEntry.Text = "";
        IssuerEntry.Text = "";
        SecretEntry.Text = "";
    }
    
    private async Task RefreshAsync()
    {
        IsRefreshing = true;
        
        // Simulate refresh delay
        await Task.Delay(1000);
        
        IsRefreshing = false;
    }
}