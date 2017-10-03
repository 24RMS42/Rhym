using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Rhym
{
    public partial class PlayListPage : ContentPage
    {
        private ObservableCollection<SongModel> _songList;

        public PlayListPage(ObservableCollection<SongModel> songList)
        {
            InitializeComponent();
            _songList = songList;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            AddSongToPlayList();
        }

        async Task AddSongToPlayList()
        {
            if (App.G_SongList.Count > 0)
            {
                foreach (var item in App.G_SongList)
                {
                    _songList.Add(item);
                }

                await App.G_HTTP_CLIENT.CreatePlayListAsync(Constants.GUID, _songList);
            }

            listView.ItemsSource = _songList;
            App.G_SongList = new ObservableCollection<SongModel>();
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
                //var index = (listView.ItemsSource as ObservableCollection<SongModel>).IndexOf(item);
            }
        }

        void OnAddSongClicked(object sender, EventArgs args)
        {
           DependencyService.Get<IBrowseMusic>().BrowseMusic();
        }

        async void OnRemoveSongClicked(object sender, EventArgs args)
        {
            var isChecked = false;
            for (int i = _songList.Count - 1; i >= 0; i--)
            {
                var item = _songList[i];
                if (item.IsSelected)
                {
                    _songList.Remove(item);
                    isChecked = true;
                }
            }

            if (isChecked)
                await App.G_HTTP_CLIENT.CreatePlayListAsync(Constants.GUID, _songList);
        }
    }
}
