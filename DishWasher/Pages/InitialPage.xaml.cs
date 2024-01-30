using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

    namespace DishWasher.Pages ;

        public partial class InitialPage : ContentPage
        {
            public InitialPage()
            {
                InitializeComponent();
            }
        
            private async void NewGroup_Clicked(object sender, EventArgs e)
            {
                // Navigate to the Login page
            
                await Navigation.PushAsync(new NewGroupPage());
            }
        
            private async void Groups_Clicked(object sender, EventArgs e)
            {
                // Navigate to the Login page
                await Navigation.PushAsync(new GroupsPage());
            }
        }