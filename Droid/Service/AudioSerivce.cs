﻿using System;
using Xamarin.Forms;
using Android.Media;
using Rhym.Droid;

[assembly: Dependency(typeof(AudioSerivce))]
namespace Rhym.Droid
{
    public class AudioSerivce : IAudio
    {
        int clicks = 0;
        MediaPlayer player;

        public AudioSerivce()
        {
        }

        public bool Play_Pause (string url)
        {
            Console.WriteLine("android audio service {0} calling...: " + url, clicks);
            if (string.IsNullOrEmpty(url))
                    return false;

            if (clicks == 0) {

                this.player = new MediaPlayer();
                this.player.SetDataSource(url);
                this.player.SetAudioStreamType(Stream.Music);
                this.player.PrepareAsync();
                this.player.Prepared += (sender, args) =>
                {
                    this.player.Start();
                    Console.WriteLine("android audio service started...");
                };
                clicks++;
            } else if (clicks % 2 != 0) {
                this.player.Pause();
                clicks++;

            } else {
                this.player.Start();
                clicks++;
            }

            return true;
        }

        public bool Stop (bool val)
        {
            if (this.player != null)
            {
                this.player.Stop();
                this.player = new MediaPlayer();
            }
            clicks = 0;
            return true;
        }
    }
}
