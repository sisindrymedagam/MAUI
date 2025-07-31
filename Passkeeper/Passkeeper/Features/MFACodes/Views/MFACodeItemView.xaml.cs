namespace Passkeeper.Features.MFACodes.Views;

public partial class MFACodeItemView : ContentView
{
    public MFACodeItemView()
    {
        InitializeComponent();
    }

    private async void OnCopyClicked(object sender, EventArgs e)
    {
        if (BindingContext is MFACodeDto mfaCode)
        {
            await Clipboard.SetTextAsync(mfaCode.Issuer);
            await ToastHelper.ShowAsync("Code copied to clipboard");
        }
    }
}