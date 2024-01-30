using System.Text.Json;
using DishWasher.Models;
using Preferences = Xamarin.Essentials.Preferences;

namespace DishWasher.Pages ;

    public partial class NewGroupPage : ContentPage
    {
        //number of players till now
        private int number = 3;
        public NewGroupPage()
        {
            InitializeComponent();
        }
        
        //activated after the add member button got clicked
        private void AddMemberClicked(object sender, EventArgs e)
        {
            // Create a new Entry for the member name
            var newMemberEntry = new Entry
            {
                Placeholder = $"barde {number++}"
            };

            // Add the new Entry to the MembersLayout
            MembersLayout.Children.Add(newMemberEntry);
        }

        private async void SubmitClicked(object sender, EventArgs e)
        {
            //read data from GroupNameEntry and the current database
            List<string> groupsNames = Groups.GetGroupsNames();
            string groupName = GroupNameEntry.Text;
            if (!DuplicateName(groupName, groupsNames)){
                
                //add the name of group to the list of GroupsName in database
                Groups.SetGroupsList(groupName);
                
                //read data from memberEntry and make a list out of them and make another value in database with the key groupName
                List<string> names = new List<string>();
                foreach (var memberEntry in MembersLayout.Children)
                {
                    if (memberEntry is Entry entry)
                    {
                         names.Add(entry.Text);
                    }
                }
                Groups.SetGroupMembers(groupName, names);
                
                await DisplayAlert("SUCCESS", "The group has been successfully created", "OK");
                //head to the group
                Application.Current.MainPage = new NavigationPage(new PastData(groupName, Groups.GetActions(groupName)));
            }
            else
            {
                await DisplayAlert("ERROR", "The name is duplicate", "OK");
            }
        }

        //check for the name of group to not be duplicate
        private  bool DuplicateName(string groupName, List<string> list)
        {
            if (list.Contains(groupName))
                return true;
            return false;
        }
    }