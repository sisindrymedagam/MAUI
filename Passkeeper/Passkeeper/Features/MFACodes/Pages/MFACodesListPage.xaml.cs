using Passkeeper.Features.MFACodes.Services;
using System.Windows.Input;

namespace Passkeeper.Features.MFACodes.Pages;

public partial class MFACodesListPage : ContentPage
{
    private readonly MFAService _mfaService;

    public MFAService MFAService => _mfaService;
    public ICommand RefreshCommand { get; }

    private bool isRefreshing;
    public bool IsRefreshing
    {
        get => isRefreshing;
        set { isRefreshing = value; OnPropertyChanged(); }
    }

    public MFACodesListPage(MFAService mfaService)
    {
        InitializeComponent();
        _mfaService = mfaService;
        RefreshCommand = new Command(async () => await RefreshAsync());
        BindingContext = this;
    }

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
        if (Parent is Shell shell)
        {
            shell.FlyoutBehavior = FlyoutBehavior.Disabled;
        }
    }

    private void ShowTabBar(object sender, EventArgs e)
    {
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

    private async void OnConfirmAdd(object sender, EventArgs e)
    {
        string? name = NameEntry.Text?.Trim();
        string? secret = SecretEntry.Text?.Trim();

        if (string.IsNullOrWhiteSpace(name))
        {
            await DisplayAlert("Error", "Please enter an account name.", "OK");
            return;
        }

        if (string.IsNullOrWhiteSpace(secret))
        {
            await DisplayAlert("Error", "Please enter a secret key.", "OK");
            return;
        }

        // Create and save the new MFA code
        var newCode = new MFACodeDto
        {
            AccountName = name,
            AccountSecret = secret
        };

        await _mfaService.SaveAsync(newCode);

        // Hide the panel and clear the form
        AddCodePanel.Hide();
        ClearAddForm();

        // Show success message
        await DisplayAlert("Success", "Authenticator code added successfully!", "OK");
    }

    private async void OnDeleteMFACode(object sender, object e)
    {
        if (e is MFACodeDto mfaCode)
        {
            bool confirm = await DisplayAlert("Delete Code", 
                $"Are you sure you want to delete '{mfaCode.AccountName}'?", 
                "Delete", "Cancel");
            
            if (confirm)
            {
                await _mfaService.DeleteAsync(mfaCode);
            }
        }
    }

    private void OnEditMFACode(object sender, object e)
    {
        if (e is MFACodeDto mfaCode)
        {
            // TODO: Implement edit functionality
            // For now, just show the code details
            DisplayAlert("Edit Code", $"Editing: {mfaCode.AccountName}", "OK");
        }
    }

    private void ClearAddForm()
    {
        NameEntry.Text = "";
        SecretEntry.Text = "";
    }

    private async Task RefreshAsync()
    {
        IsRefreshing = true;
        await Task.Delay(1000); // Simulate refresh delay
        IsRefreshing = false;
    }
}