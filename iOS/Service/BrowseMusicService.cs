using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Foundation;
using MediaPlayer;
using Rhym.iOS;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(BrowseMusicService))]
namespace Rhym.iOS
{
    public class BrowseMusicService : IBrowseMusic
    {
        ObservableCollection<SongModel> songsList = new ObservableCollection<SongModel>();

        public BrowseMusicService()
        {
        }

        public bool BrowseMusic()
        {
            UIWindow window = UIApplication.SharedApplication.KeyWindow;
            UIViewController viewController = window.RootViewController;
            if (viewController != null) {
                while (viewController.PresentedViewController != null)
                        viewController = viewController.PresentedViewController;
            }

            var navController = new UINavigationController(new BrowseMusicController());
            viewController.PresentViewController (navController, true, null);
            return true;
        }

        public ObservableCollection<SongModel> GetLocalMusic()
        {
              songsList = new ObservableCollection<SongModel>();
              var mq = new MPMediaQuery();
              mq.GroupingType = MPMediaGrouping.Album;

              var value = NSNumber.FromInt32((int)MPMediaType.Music);
              var predicate = MPMediaPropertyPredicate.PredicateWithValue(value, MPMediaItem.MediaTypeProperty);
              mq.AddFilterPredicate(predicate);
              var items = mq.Items;
              var secs = mq.ItemSections;
              
              if(secs != null)
              {
                 var songsFromSAlbums = 
                    from sSection in 
                    from sec in secs select sec
                    from song in items.Skip((int)sSection.Range.Location).Take((int)sSection.Range.Length) select song;
                 foreach(var song in songsFromSAlbums)
                 {
                    Console.WriteLine(song.Title + ": " + song.AssetURL);
                    songsList.Add (new SongModel{
                        MusicId = (long)song.PersistentID,
                        AlbumId = "",
                        SongName = song.AlbumTitle,
                        ArtistName = song.Artist,
                        Url = song.AssetURL.ToString()
                    });
                 }
              }

              return songsList;
        }
    }
}
