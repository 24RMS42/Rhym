using System;
using System.Collections.ObjectModel;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Xamarin.Forms;

namespace Rhym
{
    public partial class GetLocalSong : ContentPage
    {
        private ObservableCollection<SongModel> _songList;

        public GetLocalSong()
        {
            InitializeComponent();
            CheckPermission();

            if (_songList == null)
                _songList = DependencyService.Get<IBrowseMusic>().GetLocalMusic();
            
            Console.WriteLine("song count:" + _songList.Count);
            if (_songList != null)
                listView.ItemsSource = _songList;
            else
                ImportButton.IsEnabled = false;
        }

        async void CheckPermission()
        {
            try
            {
                var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.MediaLibrary);
                if (status != PermissionStatus.Granted)
                {
                    if(await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.MediaLibrary))
                    {
                        await DisplayAlert("Need permission", "Rhym needs media permission", "OK");
                    }

                    var results = await CrossPermissions.Current.RequestPermissionsAsync(Permission.MediaLibrary);

                    if(results.ContainsKey(Permission.MediaLibrary))
                        status = results[Permission.MediaLibrary];
                }

                if (status == PermissionStatus.Granted)
                {
                    
                }
                else if(status != PermissionStatus.Unknown)
                {
                    await DisplayAlert("Permission denied", "Please enable media library permission", "OK");
                    return;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("check permission exception:" + ex);
            }
        }

        public void OnItemSelected (object sender, ItemTappedEventArgs e) {

            if ((sender as ListView).SelectedItem == null)
                return;
            (sender as ListView).SelectedItem = null;

            var item = e.Item as SongModel;
            if (item.IsSelected)
                item.IsSelected = false;
            else
            {
                item.IsSelected = true;
            }
        }

        void OnImportSongClicked(object sender, EventArgs args)
        {
            if (_songList == null)
                return;
            
            foreach (var item in _songList)
            {
                if (item.IsSelected)
                {
                    Console.WriteLine("selected song url:" + item.Url);
                    App.G_SongList.Add(item);
                }
            }

            Navigation.PopAsync();
        }
    }
}
