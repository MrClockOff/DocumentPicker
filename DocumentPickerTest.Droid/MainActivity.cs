using Android.App;
using Android.Widget;
using Android.OS;
using Android.Content;
using Android.Net;
using Android.Provider;

namespace DocumentPickerTest.Droid
{
    [Activity(Label = "PicTheDoc", MainLauncher = true, Icon = "@mipmap/icon")]
    public class MainActivity : Activity
    {
        private const int PickActionRequestCode = 1234;

        private TextView _docName;
        private TextView _docPath;
        private Intent _resultIntent;

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

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            if (requestCode != PickActionRequestCode || resultCode != Result.Ok)
            {
                return;
            }
            _resultIntent = data;
            var uri = _resultIntent.Data;
            _docName.Text = GetDocName(uri);
            _docPath.Text = uri.Path;
        }

        void PicDoc_Click(object sender, System.EventArgs e)
        {
            var intent = new Intent(Intent.ActionGetContent);
            intent.AddCategory(Intent.CategoryOpenable);
            intent.SetType("*/*");
            var mimeTypes = new []{"image/*", "video/*", "application/pdf"};
            intent.PutExtra(Intent.ExtraMimeTypes, mimeTypes);
            StartActivityForResult(intent, PickActionRequestCode);        
        }

        void OpenDoc_Click(object sender, System.EventArgs e)
        {
            var intent = new Intent(Intent.ActionView);
            intent.SetDataAndType(_resultIntent.Data, _resultIntent.Type);
            StartActivity(intent);
        }

        private string GetDocName(Uri uri)
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
        private void SaveDoc(Uri uri, string targetPath)
        {
            //todo strore file in temp location (e.g. cache)
        }
    }
}

