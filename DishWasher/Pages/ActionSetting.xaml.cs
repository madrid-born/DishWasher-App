using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DishWasher.Models;
using Action = DishWasher.Models.Action;

namespace DishWasher.Pages ;

    public partial class ActionSetting : ContentPage
    {
        private Action action { get; set; }
        public ActionSetting(Action action)
        {
            InitializeComponent();
            this.action = action;
        }

        private void DeleteAction(object sender, EventArgs e)
        {
            Groups.DeleteAction(action);
            Application.Current.MainPage = new NavigationPage(new PastData(action.GroupName, Groups.GetActions(action.GroupName)));
        }
    }