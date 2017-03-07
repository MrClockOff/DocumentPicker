using Android.App;
using Android.Widget;
using Android.OS;
using Android.Content;
using Android.Provider;
using System.IO;
using System.Threading.Tasks;
using Android.Webkit;
using Android.Support.V4.Content;
using Android.Content.PM;
using System.Linq;
using System;

namespace DocumentPickerTest.Droid
{
    [Activity(Label = "PicTheDoc", MainLauncher = true, Icon = "@mipmap/icon")]
    public class MainActivity : Activity
    {
        private const int PickActionRequestCode = 1234;

        private TextView _docName;
        private TextView _docPath;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            // Get our button from the layout resource,
            // and attach an event to it
            _docName = FindViewById<TextView>(Resource.Id.docName);
            _docPath = FindViewById<TextView>(Resource.Id.docPath);

            var picDoc = FindViewById<Button>(Resource.Id.pickDoc);
            picDoc.Click += PicDoc_Click;
            var openDoc = FindViewById<Button>(Resource.Id.openDoc);
            openDoc.Click += OpenDoc_Click;
        }

        protected override async void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            if (requestCode != PickActionRequestCode || resultCode != Result.Ok)
            {
                return;
            }
            _docName.Text = GetDocName(data.Data);
            _docPath.Text = await SaveDoc(data.Data);
        }

        void PicDoc_Click(object sender, EventArgs e)
        {
            var intent = new Intent(Intent.ActionGetContent);
            intent.AddCategory(Intent.CategoryOpenable);
            intent.SetType("*/*");
            StartActivityForResult(intent, PickActionRequestCode);        
        }

        void OpenDoc_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_docPath.Text) || Equals(_docPath.Text, "Document path"))
            {
                return;
            }

            try
            {
                var intent = new Intent(Intent.ActionView);
                var uri = FileProvider.GetUriForFile(this, $"{PackageName}.fileprovider", new Java.IO.File(_docPath.Text));
                var type = MimeTypeMap.Singleton.GetMimeTypeFromExtension(MimeTypeMap.GetFileExtensionFromUrl(_docPath.Text));
                intent.SetDataAndType(uri, type);

                var resolveInfos = PackageManager.QueryIntentActivities(intent, PackageInfoFlags.MatchDefaultOnly);
                if (!resolveInfos.Any())
                {
                    Toast.MakeText(this, "Unable to open the file, no suitable app found", ToastLength.Long).Show();
                    return;
                }

                foreach (var ri in resolveInfos)
                {
                    GrantUriPermission(ri.ActivityInfo.PackageName, uri, ActivityFlags.GrantWriteUriPermission | ActivityFlags.GrantReadUriPermission);
                }

                StartActivity(intent);
            }
            catch(Exception ex)
            {
                Toast.MakeText(this, $"Unable to open the file {ex.Message}", ToastLength.Long).Show();
            }
        }

        private string GetDocName(Android.Net.Uri uri)
        {
            var cursor = ContentResolver.Query(uri, null, null, null, null, null);
            try
            {
                if (cursor == null || !cursor.MoveToFirst())
                {
                    return string.Empty;
                }
                return cursor.GetString(cursor.GetColumnIndex(OpenableColumns.DisplayName));
            }
            finally
            {
                cursor.Close();
            }
        }
        private async Task<string> SaveDoc(Android.Net.Uri uri)
        {
            try
            {
                var outPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), _docName.Text);
                using (var inStream = ContentResolver.OpenInputStream(uri))
                {
                    using (var outStream = File.Create(outPath))
                    {
                        await inStream.CopyToAsync(outStream);
                    }
                }
                return outPath;
            }
            catch
            {
                return null;
            }
        }
    }
}

