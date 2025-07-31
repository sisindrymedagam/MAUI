using Microsoft.Extensions.Logging;
using Passkeeper.Features.Password.Services;
using Passkeeper.Features.Settings.Pages;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Passkeeper.Features.Password.Pages;

public partial class PasswordListPage : ContentPage
{
    private readonly ILogger<PasswordListPage> _logger;
    private readonly PasswordStorageService _passwordService;

    public ObservableCollection<Models.PasswordDto> PasswordItems { get; set; } = [];
    public ICommand RefreshCommand { get; }
    private bool isRefreshing;
    public bool IsRefreshing
    {
        get => isRefreshing;
        set { isRefreshing = value; OnPropertyChanged(); }
    }

    public PasswordListPage(ILogger<PasswordListPage> logger, PasswordStorageService passwordService)
    {
        InitializeComponent();
        _logger = logger;
        _passwordService = passwordService;
        RefreshCommand = new Command(async () => await RefreshAsync());
        BindingContext = this;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadPasswordsAsync();
    }

    private async Task LoadPasswordsAsync()
    {
        PasswordItems.Clear();
        List<Models.PasswordDto> items = await _passwordService.GetListAsync();
        foreach (Models.PasswordDto item in items)
            PasswordItems.Add(item);
    }

    private async Task RefreshAsync()
    {
        IsRefreshing = true;
        await LoadPasswordsAsync();
        await ToastHelper.ShowAsync("List is refreshed.");
        IsRefreshing = false;
    }

    private async void OnAddTapped(object sender, EventArgs e)
    {
        // Use DI to resolve PasswordStorageService for SavePasswordPage
        await Navigation.PushAsync(new SavePasswordPage(_passwordService));
    }

    private void OnGenerateTapped(object sender, EventArgs e)
    {
        FloatingPanel.Hide(0);
        GeneratePasswordPanel.Show();
    }

    private void OnSettingsTapped(object sender, EventArgs e)
    {
        FloatingPanel.Hide(0);
        Navigation.PushAsync(new SettingsPage());
    }

    private void OpenAddAction(object sender, EventArgs e)
    {
        FloatingPanel.Show();
    }

    private void OnAboutTapped(object sender, EventArgs e)
    {
        Navigation.PushAsync(new AboutPage());
    }

    private void OnHelpTapped(object sender, EventArgs e)
    {
        Navigation.PushAsync(new HelpPage());
    }

    private void OnFeedbackTapped(object sender, EventArgs e)
    {
        Navigation.PushAsync(new FeedbackPage());
    }

    private void HideTabBar(object sender, EventArgs e)
    {
        TabBarHelper.HideTabBar(this);
    }

    private void ShowTabBar(object sender, EventArgs e)
    {
        TabBarHelper.ShowTabBar(this);
    }
}
