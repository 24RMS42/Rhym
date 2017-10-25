using System;
using AVFoundation;
using CoreGraphics;
using Foundation;
using MediaPlayer;
using UIKit;
using System.Linq;

namespace Rhym.iOS
{
    public class BrowseMusicController : UIViewController
    {
        public BrowseMusicController()
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            NavigationController.NavigationBar.UserInteractionEnabled = true;
            NavigationController.SetNavigationBarHidden(false, true);

            this.NavigationItem.SetLeftBarButtonItem(
 
               new UIBarButtonItem("Cancel", UIBarButtonItemStyle.Bordered, (sender, args) =>
               {
                   DismissViewController(true, null);
               }), true);

            View.BackgroundColor = UIColor.White;
            Title = "Browse Music";

            var btn = UIButton.FromType (UIButtonType.System);
            btn.Frame = new CGRect (20, 200, 280, 44);
            btn.SetTitle ("Import Music", UIControlState.Normal);

            btn.TouchUpInside += (sender, e) => {
                GetLocalMusic();
            };

            View.AddSubview (btn);
            CheckPermission();
        }

        void CheckPermission()
        {
            MPMediaLibraryAuthorizationStatus authorizationStatus = MPMediaLibrary.AuthorizationStatus;
            Console.WriteLine("auth status:" + authorizationStatus);
        }

        void GetLocalMusic()
        {
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
                 }
              }
        }
    }
}
