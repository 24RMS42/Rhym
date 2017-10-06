using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Plugin.MediaManager;
using Xamarin.Forms;

namespace Rhym
{
    public partial class HomePage : ContentPage
    {
        private ObservableCollection<SongModel> _songList;
        private ObservableCollection<SongModel> _musicList;
        private int current_playing_index;

        public HomePage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            current_playing_index = -1;
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            await GetSongList();
        }

        async Task GetSongList()
        {
            if (_songList == null)
                _songList = await App.G_HTTP_CLIENT.GetPlayListAsync(Constants.GUID);

            if (_musicList == null)
                _musicList = await App.G_HTTP_CLIENT.GetMusicListAsync();

            var all = new ObservableCollection<SongModel>();
            foreach (var item in _songList)
            {
                all.Add(item);
            }

            foreach (var item in _musicList)
            {
                all.Add(item);
            }

            listView.ItemsSource = all;
        }

        public void OnItemSelected (object sender, ItemTappedEventArgs e) {

            if ((sender as ListView).SelectedItem == null)
                return;
            (sender as ListView).SelectedItem = null;

            var item = e.Item as SongModel;
            var index = (listView.ItemsSource as ObservableCollection<SongModel>).IndexOf(item);

            Console.WriteLine("item index:" + index);

            //await CrossMediaManager.Current.Play("https://www.searchgurbani.com/audio/sggs/1.mp3");
            //await CrossMediaManager.Current.Play("http://www.bensound.org/bensound-music/bensound-tenderness.mp3");

            if (item.IsUrl)
            {
                if (index != current_playing_index && current_playing_index != -1)
                {
                    DependencyService.Get<IAudio>().Stop(true);
                }

                DependencyService.Get<IAudio>().Play_Pause(item.Url);
                current_playing_index = index;
            }
            else
            {
                CrossMediaManager.Current.Play(item.Url);
            }
        }

        void OnEditButtonClicked(object sender, EventArgs args)
        {
            var mySongList = new ObservableCollection<SongModel>();
            foreach (var item in _songList)
            {
                if (!item.IsUrl)
                    mySongList.Add(item);
            }
            Navigation.PushAsync(new PlayListPage(_songList));
        }

        void OnProfileButtonClicked(object sender, EventArgs args)
        {
            Navigation.PushAsync(new Signup());
        }
    }
}
