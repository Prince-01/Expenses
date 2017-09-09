using System;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Database;
using Android.OS;
using Android.Util;
using ExpensesMobile.Droid.Services;
using ExpensesMobile.Models;
using Uri = Android.Net.Uri;

namespace ExpensesMobile.Droid
{
    [Activity(Label = "@string/app_name", Theme = "@style/MyTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);

            LoadApplication(new App());
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            switch (requestCode)
            {
                case 1:
                    if (resultCode == Result.Ok)
                    {
                        Uri uri = data.Data;
                        string path = getPath(this, uri);
                        var file = System.IO.File.ReadAllBytes(path);
                        FileService._addFilePostAction(new File
                        {
                            Name = getFileName(path),
                            Bytes = file,
                            Size = file.Length
                        });
                    }
                    break;
            }

            base.OnActivityResult(requestCode, resultCode, data);
        }

        public static string getFileName(string path)
        {
            return path.Substring(path.LastIndexOf("/") + 1);
        }

        public static string getPath(Context context, Uri uri)
        {
            if ("content".Equals(uri.Scheme, StringComparison.InvariantCultureIgnoreCase))
            {
                string[] projection = { "_data" };
                ICursor cursor = null;

                try
                {
                    cursor = context.ContentResolver.Query(uri, projection, null, null, null);
                    int column_index = cursor.GetColumnIndexOrThrow("_data");
                    if (cursor.MoveToFirst())
                    {
                        return cursor.GetString(column_index);
                    }
                }
                catch (Exception e)
                {
                    // Eat it
                }
            }
            else if ("file".Equals(uri.Scheme, StringComparison.InvariantCultureIgnoreCase))
            {
                return uri.Path;
            }

            return null;
        }
    }
}