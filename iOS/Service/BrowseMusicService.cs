using System;
using Rhym.iOS;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(BrowseMusicService))]
namespace Rhym.iOS
{
    public class BrowseMusicService : IBrowseMusic
    {
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
    }
}
