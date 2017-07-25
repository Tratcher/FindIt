﻿using FindIt.Views;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace FindIt
{
	public partial class App : Application
    {
        public static ILocator Locator { get; private set; }

        public static void Init(ILocator locator)
        {
            Locator = locator;
        }

        public App()
		{
			InitializeComponent();

			SetMainPage();
		}

		public static void SetMainPage()
		{
            Current.MainPage = new TabbedPage
            {
                Children =
                {
                    new NavigationPage(new ItemsPage())
                    {
                        Title = "Browse",
                        Icon = Device.OnPlatform<string>("tab_feed.png",null,null)
                    },
                    new NavigationPage(new AboutPage())
                    {
                        Title = "About",
                        Icon = Device.OnPlatform<string>("tab_about.png",null,null)
                    },
                }
            };
        }
	}
}
