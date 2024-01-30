using System;
using System.Collections.Generic;
using Microsoft.Maui.Controls;

namespace DishWasher.Pages
{
    public partial class ResultPage : ContentPage
    {
        public ResultPage(List<string> selectedUsers)
        {
            InitializeComponent();

            foreach (var user in selectedUsers)
            {
                var textField = new Entry
                {
                    Placeholder = user
                };
                textFieldContainer.Children.Add(textField);
            }
        }
    }
}