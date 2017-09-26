using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Rhym
{
    public partial class SignupConfirm : ContentPage
    {
        private string _email;
        public SignupConfirm(string email)
        {
            InitializeComponent();
            _email = email;
        }

        async void OnActivateButtonClicked(object sender, EventArgs args)
        {
            await ActivateAsync(activationCode.Text);
        }

        async Task ActivateAsync(string activationcode)
        {
            if (CheckValidate())
            {
                var result = await App.G_HTTP_CLIENT.ActivateAsync(_email, activationcode);

                if (result)
                {
                    await Navigation.PushAsync(new HomePage());
                }
            }
        }

        private bool CheckValidate()
        {
            if (string.IsNullOrEmpty(activationCode.Text))
            {
                DisplayAlert("Warning!", "Activation code is required!", "OK");
                return false;
            }
            else
                return true;
        }
    }
}
