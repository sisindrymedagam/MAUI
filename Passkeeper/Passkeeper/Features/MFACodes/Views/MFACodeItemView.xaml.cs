using Passkeeper.Features.MFACodes.Models;

namespace Passkeeper.Features.MFACodes.Views;

public partial class MFACodeItemView : ContentView
{
    public MFACodeItemView()
    {
        InitializeComponent();
    }

    private async void OnCopyClicked(object sender, EventArgs e)
    {
        if (BindingContext is MFACode mfaCode)
        {
            await Clipboard.SetTextAsync(mfaCode.Code);
            await ToastHelper.ShowAsync("Code copied to clipboard");
        }
    }
}