using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Rhym
{
    public partial class Login : ContentPage
    {
        private bool _switchClicked;

        public Login()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            IDictionary<string, object> properties = Application.Current.Properties;
            if(properties.ContainsKey(Constants.REMEMBERME_KEY))
            {
                bool rememberme = (bool)properties[Constants.REMEMBERME_KEY];
                if (rememberme)
                {
                    RememberMeSwitch.IsToggled = true;
                    email.Text = properties[Constants.USEREMAIL_KEY].ToString();
                    password.Text = properties[Constants.USERPWD_KEY].ToString();

                    await LoginAsync(properties[Constants.USEREMAIL_KEY].ToString(), properties[Constants.USERPWD_KEY].ToString());
                }
            }
        }

        async void OnLoginButtonClicked(object sender, EventArgs args)
        {
            await LoginAsync(email.Text, password.Text);
        }

        void OnSignupButtonClicked(object sender, EventArgs args)
        {
            Navigation.PushAsync(new Signup());
        }

        async Task LoginAsync(string useremail, string userpassword)
        {
            if (CheckValidate())
            {
                if (_switchClicked) SaveLoginDetail();

                var result = await App.G_HTTP_CLIENT.LoginAsync(useremail, userpassword);

                if (result)
                {
                    await Navigation.PushAsync(new HomePage());
                }
            }
        }

        private bool CheckValidate()
        {
            if (string.IsNullOrEmpty(email.Text) || string.IsNullOrEmpty(password.Text))
            {
                DisplayAlert("Warning!", "Email and password is required!", "OK");
                return false;
            }
            else if (!Validater.EmailValidator(email.Text))
            {
                DisplayAlert("Warning!", "Please input valid email", "OK");
                return false;
            }
            else
                return true;
        }

        void ClickSwitch(object sender, ToggledEventArgs args)
        {
            if (args.Value)
                _switchClicked = true;
            else
                _switchClicked = false;
        }

        void SaveLoginDetail()
        {
            IDictionary<string, object> properties = Application.Current.Properties;
            properties[Constants.REMEMBERME_KEY] = true;
            properties[Constants.USEREMAIL_KEY] = email.Text.Trim();
            properties[Constants.USERPWD_KEY] = password.Text.Trim();
        }
    }
}
