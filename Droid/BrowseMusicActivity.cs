using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Database;
using Android.OS;
using Android.Provider;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Views;
using Android.Widget;

namespace Rhym.Droid
{
    [Activity(Label = "Select Music")]
    public class BrowseMusicActivity : Activity
    {
        readonly string[] Permissions =
        {
            Manifest.Permission.ReadExternalStorage
        };

        const int RequestLocationId = 0;
        List<SongModel> songsList;
        ListView listView;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.BrowseMusic);
            listView = FindViewById<ListView>(Resource.Id.listview);
            Button btnImport = FindViewById<Button>(Resource.Id.btn_add);

            TryGetLocalMusic();

            btnImport.Click += (sender, e) => 
            {
                OnBackPressed();
                App.G_SongList = new ObservableCollection<SongModel>();
                foreach (var item in songsList)
                {
                    if (item.IsSelected)
                    {
                        Console.WriteLine("selected song url:" + item.Url);
                        App.G_SongList.Add(item);
                    }
                }
            };
        }

        void GetLocalMusic()
        {
            var uri = MediaStore.Audio.Media.ExternalContentUri;
            string[] projection = {
                MediaStore.Audio.Media.InterfaceConsts.Id,
                MediaStore.Audio.Media.InterfaceConsts.AlbumId,
                MediaStore.Audio.Media.InterfaceConsts.Title,
                MediaStore.Audio.Media.InterfaceConsts.Artist,
                MediaStore.Audio.Media.InterfaceConsts.Data
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
                        ArtistName = cursor.GetString(cursor.GetColumnIndex(projection[3])),
                        Url = cursor.GetString(cursor.GetColumnIndex(projection[4]))
                    });
                } while (cursor.MoveToNext());
            }

            listView.Adapter = new SongAdapter(this, cursor, songsList);
            Console.WriteLine("songlist:" + songsList.Count);
        }

        void TryGetLocalMusic()
        {
        	if ((int)Build.VERSION.SdkInt < 23)
        	{
                GetLocalMusic();
        		return;
        	}

        	GetPermissionAsync();
        }

        void GetPermissionAsync()
        {
        	//Check to see if any permission in our group is available, if one, then all are
        	const string permission = Manifest.Permission.ReadExternalStorage;
        	if (CheckSelfPermission(permission) == (int)Permission.Granted)
        	{
              	GetLocalMusic();
        		return;
        	}

        	//need to request permission
        	//if (ShouldShowRequestPermissionRationale(permission))
        	//{
        	//	//Explain to the user why we need to read the contacts
        	//	Snackbar.Make(layout, "Location access is required to show coffee shops nearby.", Snackbar.LengthIndefinite)
        	//			.SetAction("OK", v => RequestPermissions(PermissionsLocation, RequestLocationId))
        	//			.Show();
        	//	return;
        	//}

        	//Finally request permissions with the list of permissions and Id
        	RequestPermissions(Permissions, RequestLocationId);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
        	switch (requestCode)
        	{
        		case RequestLocationId:
        			{
        				if (grantResults[0] == Permission.Granted)
        				{
        					//Permission granted
        					//var snack = Snackbar.Make(layout, "Location permission is available, getting lat/long.", Snackbar.LengthShort);
        					//snack.Show();

        					GetLocalMusic();
        				}
        				else
        				{
        					//Permission Denied 🙁
        					//Disabling location functionality
        					//var snack = Snackbar.Make(layout, "Location permission is denied.", Snackbar.LengthShort);
        					//snack.Show();
        				}
        			}
                    break;
            }
        }
    }
}
