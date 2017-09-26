﻿using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.Database;
using Android.Provider;
using Android.Views;
using Android.Widget;

namespace Rhym.Droid
{
    public class SongAdapter : CursorAdapter
    {
        List<SongModel> songsList;
        Activity context;

        public SongAdapter (Activity context, ICursor cursor, List<SongModel> theSongs): base(context,cursor)
        {
            this.context = context;
            songsList = theSongs;
        }

        public override void BindView(View view, Context context, ICursor cursor)
        {
            var song_title = view.FindViewById<TextView> (Resource.Id.txt_song_name);
            var song_artist = view.FindViewById<TextView> (Resource.Id.txt_artist_name);
            song_title.Text = songsList [cursor.Position].SongName;
            song_artist.Text = songsList [cursor.Position].ArtistName;

            //If there is an image to display, it will display it. If not, it will use a default image
            if (songsList [cursor.Position].UserId == null) {
                //song_album_art = view.FindViewById<ImageView> (Resource.Id.song_album_art);
                //song_album_art.SetImageResource (Resource.Drawable.ic_action_user); //Image needs to be set
            } else {
                //var songUri = ContentUris.WithAppendedId (MediaStore.Audio.Media.ExternalContentUri, songsList [cursor.Position].Id);
                //var albumArtUri = Android.Net.Uri.WithAppendedPath(songUri,MediaStore.Audio.Albums.EntryContentType);

                //song_album_art.SetImageURI (albumArtUri);
            }
        }

        public override View NewView(Context context, ICursor cursor, ViewGroup parent)
        {
            return this.context.LayoutInflater.Inflate(Resource.Layout.songlist_item, parent, false);
        }
    }
}
