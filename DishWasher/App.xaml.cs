using DishWasher.Pages;

namespace DishWasher ;

    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            MainPage = new NavigationPage(new InitialPage());
        }
    }