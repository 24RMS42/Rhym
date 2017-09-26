using System.Collections.Generic;
using Xamarin.Forms;

namespace Rhym
{
    public partial class App : Application
    {
        public static HttpHandler G_HTTP_CLIENT { get; set; }
        public static string Token { get; set; }

        public App()
        {
            InitializeComponent();

            G_HTTP_CLIENT = new HttpHandler();

            IDictionary<string, object> properties = Current.Properties;
            if (properties.ContainsKey(Constants.TOKEN_KEY))
            {
                Token = properties[Constants.TOKEN_KEY].ToString();
            }

            MainPage = new NavigationPage(new Login());
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
