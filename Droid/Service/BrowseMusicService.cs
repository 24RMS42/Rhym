using System;
using Android.Content;
using Rhym.Droid;
using Xamarin.Forms;

[assembly: Dependency(typeof(BrowseMusicService))]
namespace Rhym.Droid
{
    public class BrowseMusicService : IBrowseMusic
    {
        public BrowseMusicService()
        {
        }

        public bool BrowseMusic()
        {
            Forms.Context.StartActivity(typeof(BrowseMusicActivity));
            return true;
        }
    }
}
