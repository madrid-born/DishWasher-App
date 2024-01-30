using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DishWasher.Models;

namespace DishWasher.Pages ;

    public partial class Setting : ContentPage
    {
        private string GroupName { get; set; }
        public Setting(string groupName)
        {
            InitializeComponent();
            GroupName = groupName;
        }

        private void DeleteGroup(object sender, EventArgs e)
        {
            //if (sender is Button button && button.CommandParameter is string groupName)
            Groups.DeleteGroup(GroupName);
            Application.Current.MainPage = new NavigationPage(new InitialPage());

        }
    }