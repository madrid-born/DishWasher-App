using Microsoft.Maui.Controls;
using Xamarin.Essentials;
using Preferences = Xamarin.Essentials.Preferences;

namespace DishWasher
{
    public partial class MainPage : ContentPage
    {
        int count;

        public MainPage()
        {
            InitializeComponent();

            // Retrieve the count from Preferences during initialization
            count = Preferences.Get("ButtonClickCount", 0);
            UpdateCounterText();
        }

        private void OnCounterClicked(object sender, EventArgs e)
        {
            count++;
            UpdateCounterText();

            // Save the updated count to Preferences
            Preferences.Set("ButtonClickCount", count);

            // Announce the new count using the SemanticScreenReader
            SemanticScreenReader.Announce(CounterBtn.Text);
        }

        private void UpdateCounterText()
        {
            if (count == 1)
                CounterBtn.Text = $"Clicked {count} time";
            else
                CounterBtn.Text = $"Clicked {count} times";
        }
    }
}