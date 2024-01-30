using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using DishWasher.Models;
using Action = DishWasher.Models.Action;

namespace DishWasher.Pages ;

    public partial class PastData : ContentPage
    {
        private string Group { get; set; }
        private List<List<int>> Table { get; set; }
        
        private string TableString { get; set; }
        public PastData(string groupName, List<Action> actions)
        {
            InitializeComponent();
            Group = groupName;
            Table = Groups.GetTable(groupName);
            LoadTable();
            //TableString = TableToString(Table);
            PastDataList.ItemsSource = actions;
        }

        private void LoadTable()
        {
            List<StackLayout> stacks = new List<StackLayout>();
            foreach (var intList in Table)
            {
                List<Label> labels = new List<Label>();
                foreach (var integer in intList)
                {
                    Label integerField = new Label
                    {
                        Text = integer.ToString(),
                    };
                    labels.Add(integerField);
                }
                
                var listStackLayout = new StackLayout
                {
                    Orientation = StackOrientation.Horizontal,
                    Spacing = 10,
                    Children = {}
                };
                
                foreach (var label in labels)
                {
                    listStackLayout.Children.Add(label);
                }
                stacks.Add(listStackLayout);
            }
            var tableStackLayout = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                Spacing = 10,
                Children = {}
            };
                
            foreach (var stack in stacks)
            {
                tableStackLayout.Children.Add(stack);
            }
            PastTable.Children.Add(tableStackLayout);
        }

        private string TableToString(List<List<int>> table)
        {
            string result = "";
            foreach (var intList in table)
            {
                foreach (var integer in intList)
                {
                    result += $"{integer}\t";
                }
                result += "\n";
            }
            return result;
        }
        
        private async void Button_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new NewData(Group));
        }

        private async void Setting_OnClicked(object sender, EventArgs e)
        {
            var button = (Button)sender;
            if (button?.BindingContext is Action action)
                await Navigation.PushAsync(new ActionSetting(action));
        }
    }