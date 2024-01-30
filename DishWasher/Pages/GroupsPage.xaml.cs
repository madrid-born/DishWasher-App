using System.Text.Json;
using DishWasher.Models;

namespace DishWasher.Pages ;

    public partial class GroupsPage : ContentPage
    {
        public GroupsPage()
        {
            InitializeComponent();
        }
        
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            // this will load the groups you have to the main page
            GroupsNames.ItemsSource = Groups.GetGroupsNames();
        }

        private async void GroupTapped(object sender, TappedEventArgs e)
        {
            // action handler for taping on one group
            var tappedGroup = (sender as Grid)?.GestureRecognizers
                .OfType<TapGestureRecognizer>()
                .FirstOrDefault()
                ?.CommandParameter;
            if (tappedGroup is String groupName)
            {
                Application.Current.MainPage = new NavigationPage(new PastData(groupName, Groups.GetActions(groupName)));
            }
        }

        private async void Button_OnClicked(object sender, EventArgs e)
        {
            var button = (Button)sender;
            if (button?.BindingContext is string groupName)
            await Navigation.PushAsync(new Setting(groupName));
        }
    }