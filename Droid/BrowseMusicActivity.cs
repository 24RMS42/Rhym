using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.Database;
using Android.OS;
using Android.Provider;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Rhym.Droid
{
    [Activity(Label = "Select Music")]
    public class BrowseMusicActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.BrowseMusic);
            ListView listView = FindViewById<ListView>(Resource.Id.listview);

            List<SongModel> songsList;

            var uri = MediaStore.Audio.Media.ExternalContentUri;
            string[] projection = {
                MediaStore.Audio.Media.InterfaceConsts.Id,
                MediaStore.Audio.Media.InterfaceConsts.AlbumId,
                MediaStore.Audio.Media.InterfaceConsts.Title,
                MediaStore.Audio.Media.InterfaceConsts.Artist,
            };
            var loader = new CursorLoader (this, uri, projection, null, null, null);
            var cursor = (ICursor)loader.LoadInBackground ();
            songsList = new List<SongModel>();
            if (cursor.MoveToFirst ()) {
                do {
                    songsList.Add (new SongModel{
                        MusicId = cursor.GetLong(cursor.GetColumnIndex(projection[0])),
                        AlbumId = cursor.GetString(cursor.GetColumnIndex(projection[1])),
                        SongName = cursor.GetString(cursor.GetColumnIndex(projection[2])),
                        ArtistName = cursor.GetString(cursor.GetColumnIndex(projection[3]))
                    });
                } while (cursor.MoveToNext());
            }

            listView.Adapter = new SongAdapter(this, cursor, songsList);
            Console.WriteLine("songlist:" + songsList.Count);
        }
    }
}
