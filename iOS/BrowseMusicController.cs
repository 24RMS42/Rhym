using System;
using CoreGraphics;
using Foundation;
using MediaPlayer;
using UIKit;

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

        }

        void GetLocalMusic()
        {
            MPMediaQuery mq = MPMediaQuery.PlaylistsQuery;
        }
    }
}
