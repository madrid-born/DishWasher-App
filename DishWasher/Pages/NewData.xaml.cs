using DishWasher.Models;

namespace DishWasher.Pages
{
    public partial class NewData : ContentPage
    {
        private string GroupName { get; set; }
        private List<string> Members { get; set; }

        private List<CheckBox> CheckBoxes = new List<CheckBox>();
        
        public NewData(string groupName)
        {
            InitializeComponent();
            GroupName = groupName;
            Members = Groups.GetGroupMembers(groupName);
            LoadCheckboxes();
        }

        private void LoadCheckboxes()
        {
            foreach (var member in Members)
            {
                var checkBox = new CheckBox
                {
                    IsChecked = true
                };
                Label checkBoxLabel = new Label
                {
                    Text = member
                };

                var checkBoxStackLayout = new StackLayout
                {
                    Orientation = StackOrientation.Horizontal,
                    Children = { checkBox, checkBoxLabel }
                };
        
                checkboxContainer.Children.Add(checkBoxStackLayout);
        
                checkBox.BindingContext = checkBoxLabel;
                checkBox.CheckedChanged += CheckBox_CheckedChanged;
        
                CheckBoxes.Add(checkBox);
            }

            var submitButton = new Button
            {
                Text = "Submit",
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                Margin = new Thickness(0, 20)
            };

            submitButton.Clicked += SubmitButton_Clicked;

            checkboxContainer.Children.Add(submitButton);
        }

        private void CheckBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if (sender is CheckBox checkBox && checkBox.BindingContext is Label label)
            {
                // Handle checkbox change event, if needed
            }
        }

        private async void SubmitButton_Clicked(object sender, EventArgs e)
        {
            var selectedUsers = CheckBoxes
                .Where(checkBox => checkBox.IsChecked)
                .Select(checkBox => checkBox.BindingContext as Label)
                .Select(label => label.Text)
                .ToList();
            var names = CheckBoxes
                .Where(checkBox => checkBox.IsVisible)
                .Select(checkBox => checkBox.BindingContext as Label)
                .Select(label => label.Text)
                .ToList();
            List<double> probabilities = CalculateCleaner(names, selectedUsers);
            string cleaner = RandomWithProbability(probabilities, selectedUsers);
            await DisplayAlert("Nice", $"The cleaner is no one else but {cleaner}\nthe odds were {ListToString(probabilities, selectedUsers)}", "ok");
            Groups.SetTableChange(this.GroupName, cleaner, selectedUsers, true);
            Groups.AddAction(GroupName, cleaner, selectedUsers, DateTime.Now);
            Application.Current.MainPage = new NavigationPage(new PastData(GroupName, Groups.GetActions(GroupName)));
        }

        private string ListToString(List<double> probabilities, List<string> names)
        {
            string result = "";
            for (int i = 0; i < names.Count; i++)
            {
                double percentage = probabilities[i]*100;
                string formattedPercentage = percentage.ToString("0.00") + " %";
                result += $"\n{names[i]} : {formattedPercentage}";
            }
            return result;
        }

        private List<double> CalculateCleaner(List<string> names ,List<string> selected)
        {
            List<List<int>> table = Groups.GetTable(GroupName);
            List<List<int>> newTable = OnlyAvailable(selected ,names, table);
            List<double> probabilities = CalculateProbabilities(newTable);
            return probabilities;
        }
        
        private List<List<int>> OnlyAvailable(List<string> selected ,List<string> names ,List<List<int>> table)
        {
            List<List<int>> newTable = new List<List<int>>();
            List<int> notAvailable = new List<int>();
            foreach (var name in names)
            {
                if (!selected.Contains(name))
                {
                    notAvailable.Add(names.IndexOf(name));
                }
            }
            for (int i = 0; i < names.Count; i++)
            {
                List<int> newRow = new List<int>();
                for (int j = 0; j < names.Count; j++)
                {
                    if (!(notAvailable.Contains(i) || notAvailable.Contains(j)))
                    {
                        newRow.Add(table[i][j]);
                    }
                }
                if (newRow.Count > 0)
                {
                    newTable.Add(newRow);
                }
            }
            return newTable;
        }

        private List<double> CalculateProbabilities(List<List<int>> table)
        {
            List<double> probabilities = new List<double>();
            int lenght = table[0].Count;
            for (int i = 0; i < lenght; i++)
            {
                double result = 0;
                for (int j = 0; j < lenght; j++)
                {
                    if (j != i)
                    {
                        if (table[i][j] == table[j][i])
                        {
                            result += (double) 1 / 2;
                        }
                        else
                        {
                            result += (double)table[j][i] / (table[i][j] + table[j][i]);
                        }
                    }
                }
                result *= (double) 2 / (lenght * (lenght - 1));
                probabilities.Add(result);
            }
            return probabilities;
        }

        private string RandomWithProbability(List<double> probabilities, List<string> names)
        {
            Random random = new Random();
            double randomValue = random.NextDouble();
            int boundary = probabilities.Count - 1;
            int selectedIndex = 0;
            double cumulativeProbability = probabilities[0];
            while (randomValue > cumulativeProbability && selectedIndex < boundary)
            {
                selectedIndex++;
                cumulativeProbability += probabilities[selectedIndex];
            }
            return names[selectedIndex];
        }
    }
}