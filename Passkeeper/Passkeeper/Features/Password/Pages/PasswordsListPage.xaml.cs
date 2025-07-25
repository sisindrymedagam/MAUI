using Passkeeper.Helpers;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Passkeeper.Features.Password.Pages;

public partial class PasswordsListPage : ContentPage
{
    public ObservableCollection<PasswordListView> PasswordItems { get; set; }
    //public ObservableCollection<CompanyListView> CompanyItems { get; set; }

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

    public PasswordsListPage()
    {
        InitializeComponent();
        PasswordItems = PasswordsList.GetPasswordListView();
        //CompanyItems = PasswordsList.GetCompanyListView();
        _ = new MauiIcon();
        RefreshCommand = new Command(async () => await RefreshAsync());
        BindingContext = this;
    }

    //Overrides the back button/gesture
    protected override bool OnBackButtonPressed()
    {
        if (FloatingPanel.IsVisible)
        {
            FloatingPanel.Hide();
            return true;
        }
        if (GeneratePasswordPanel.IsVisible)
        {
            GeneratePasswordPanel.Hide();
            return true;
        }
        // Default behavior
        return base.OnBackButtonPressed();
    }

    private void HideTabBar(object sender, EventArgs e)
    {
        TabBarHelper.HideTabBar(this);
    }

    private void ShowTabBar(object sender, EventArgs e)
    {
        TabBarHelper.ShowTabBar(this);
    }

    private void OpenAddAction(object sender, EventArgs e)
    {
        FloatingPanel.Show();
    }

    private async void OnAddTapped(object sender, EventArgs e)
    {
        FloatingPanel.Hide(0);
        await Navigation.PushAsync(new SavePasswordPage());
    }

    private void OnGenerateTapped(object sender, EventArgs e)
    {
        FloatingPanel.Hide(0);
        GeneratePasswordPanel.Show();
    }

    private async void OnSettingsTapped(object sender, EventArgs e)
    {
        FloatingPanel.Hide(0);
        //await Navigation.PushAsync(new SettingsPage());
    }

    private async void OnAboutTapped(object sender, EventArgs e)
    {
        //await Navigation.PushAsync(new AboutPage());
    }

    private async void OnHelpTapped(object sender, EventArgs e)
    {
        //await Navigation.PushAsync(new HelpPage());
    }

    private async void OnFeedbackTapped(object sender, EventArgs e)
    {
        //await Navigation.PushAsync(new FeedbackPage());
    }

    private async Task RefreshAsync()
    {
        IsRefreshing = true;
        await Task.Delay(1500); // Simulate a delay

        await ToastHelper.ShowAsync("List is refreshed.");
        IsRefreshing = false;
    }

}
