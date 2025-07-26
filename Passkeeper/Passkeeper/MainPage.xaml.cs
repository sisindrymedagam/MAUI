using Microsoft.Extensions.Logging;

namespace Passkeeper
{
    public partial class MainPage : ContentPage
    {
        int count = 0;
        private readonly ILogger<MainPage> _logger;

        public MainPage(ILogger<MainPage> logger)
        {
            _logger = logger;
            InitializeComponent();
        }

        private void OnCounterClicked(object? sender, EventArgs e)
        {
            count++;

            if (count == 1)
                CounterBtn.Text = $"Clicked {count} time";
            else
                CounterBtn.Text = $"Clicked {count} times";
            
            _logger.LogInformation($"we have clicked {count} times!");

            SemanticScreenReader.Announce(CounterBtn.Text);
        }
    }
}
