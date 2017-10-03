using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Plugin.Media;
using Xamarin.Forms;

namespace Rhym
{
    public partial class Signup : ContentPage
    {
        byte[] imageData;

        public Signup()
        {
            InitializeComponent();

            IDictionary<string, object> properties = Application.Current.Properties;
            if (properties.ContainsKey(Constants.FIRSTNAME_KEY))
            {
                firstName.Text = properties[Constants.FIRSTNAME_KEY].ToString();
            }

            if (properties.ContainsKey(Constants.LASTNAME_KEY))
            {
                lastName.Text = properties[Constants.LASTNAME_KEY].ToString();
            }

            if (properties.ContainsKey(Constants.USEREMAIL_KEY))
            {
                email.Text = properties[Constants.USEREMAIL_KEY].ToString();
            }

            if (properties.ContainsKey(Constants.TOKEN_KEY))
            {
                var token = properties[Constants.TOKEN_KEY].ToString();
                if (string.IsNullOrEmpty(token))
                {
                    PhotoImage.IsVisible = false;
                    signupButton.Text = "Sign Up";
                }
                else
                {
                    PhotoImage.IsVisible = true;
                    signupButton.Text = "Update";

                    if (properties.ContainsKey(Constants.AVATAR_URL_KEY))
                    {
                        var avatar_url = properties[Constants.AVATAR_URL_KEY].ToString();
                        PhotoImage.Source = new UriImageSource { CachingEnabled = true, Uri = new Uri(avatar_url) };
                    }
                }
            }
        }

        public async void OnPhotoTapped(object sender, EventArgs e)
        {
            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                await DisplayAlert("Photos Not Supported", ":( Permission not granted to photos.", "OK");
                return;
            }
            var file = await CrossMedia.Current.PickPhotoAsync();

            //if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            //{
            //    await DisplayAlert("No Camera", ":( No camera avaialble.", "OK");
            //    return;
            //}

            //var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions{});

            if (file == null)
                return;

            Content.FindByName<Image>("PhotoImage").Source = ImageSource.FromStream(() =>
            {
                var stream = file.GetStream();

                var memoryStream = new MemoryStream();
                file.GetStream().CopyTo(memoryStream);
                imageData = memoryStream.ToArray();

                App.G_HTTP_CLIENT.UploadImage(imageData);

                file.Dispose();

                return stream;
            });
        }

        async void OnSignupButtonClicked(object sender, EventArgs args)
        {
            await SignupAsync(email.Text, password.Text, firstName.Text, lastName.Text);
        }

        async Task SignupAsync(string useremail, string userpassword, string firstname, string lastname)
        {
            if (CheckValidate())
            {
                var result = await App.G_HTTP_CLIENT.SignupAsync(firstname, lastname, useremail, userpassword);

                if (result)
                {
                    IDictionary<string, object> properties = Application.Current.Properties;
                    properties[Constants.FIRSTNAME_KEY] = firstname;
                    properties[Constants.LASTNAME_KEY] = lastname;
                    properties[Constants.USEREMAIL_KEY] = useremail;
                    await Navigation.PushAsync(new SignupConfirm(useremail));
                }
            }
        }

        private bool CheckValidate()
        {
            if (string.IsNullOrEmpty(firstName.Text) || string.IsNullOrEmpty(lastName.Text) || string.IsNullOrEmpty(email.Text) || string.IsNullOrEmpty(password.Text))
            {
                DisplayAlert("Warning!", "Please fill out all fields", "OK");
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
    }
}
