using System;
using AVFoundation;
using Foundation;
using Rhym.iOS;
using Xamarin.Forms;

[assembly: Dependency(typeof(AudioService))]
namespace Rhym.iOS
{
    public class AudioService : IAudio
    {
        int clicks = 0;
        AVPlayer player;

        public AudioService()
        {
        }

        public bool Play_Pause(string url)
        {
            Console.WriteLine("ios audio service calling...: " + clicks);
            if (clicks == 0)
            {
                //this.player1 = new AVAudioPlayer(NSUrl.FromString(url), "mp3", out err);
                this.player = new AVPlayer();
                url = url.Replace(" ", "%20");
                Console.WriteLine("audio url: " + url);
                this.player = AVPlayer.FromUrl(NSUrl.FromString(url));
                this.player.Play();
                clicks++;
            }
            else if (clicks % 2 != 0)
            {
                this.player.Pause();
                clicks++;
            }
            else {
                this.player.Play();
                clicks++;
            }
            return true;
        }

        public bool Stop(bool val)
        {
            if (player != null)
            {
                player.Dispose();
                this.player = new AVPlayer();
            }
            clicks = 0;
            return true;
        }
    }
}
