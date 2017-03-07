using Android.App;
using Android.Content;
using Android.Support.V4.Content;

namespace DocumentPickerTest.Droid
{
    /// <summary>
    /// This is Android FileProvider which securely provides file content to 
    /// third party apps. It has to be registered in AndroidManifest.xml. Xamarin 
    /// way to register ContentProvider is using ContentProviderAttribute.
    /// </summary>
    [ContentProvider(new [] {"@PACKAGE_NAME@.fileprovider"}, Exported = false, GrantUriPermissions = true)]
    [MetaData("android.support.FILE_PROVIDER_PATHS", Resource = "@xml/pickthedoc_fileprovider_paths")]
    public class FileContentProvider : FileProvider
    {
        
    }
}