using System;
using Foundation;
using MobileCoreServices;

using UIKit;

namespace DocumentPickerTest.iOS
{
    public partial class ViewController : UIViewController
    {
        private readonly string[] _docTypes;

        protected ViewController(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
            _docTypes = new string[]
            {
                UTType.UTF8PlainText,
                UTType.PDF,
                UTType.PNG,
                UTType.JPEG,
                UTType.Text,
                UTType.Image,
                UTType.RTF
            };
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            // Perform any additional setup after loading the view, typically from a nib.
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }

        partial void OpenPicker_TouchUpInside(UIButton sender)
        {
            var docPicker = new UIDocumentPickerViewController(_docTypes, UIDocumentPickerMode.Import);
            docPicker.DidPickDocument += DocPicker_DidPickDocument;
            PresentViewController(docPicker, true, null);
        }

        partial void OpenMenu_TouchUpInside(UIButton sender)
        {
            var docMenu = new UIDocumentMenuViewController(_docTypes, UIDocumentPickerMode.Import);
            docMenu.DidPickDocumentPicker += DocMenu_DidPickDocumentPicker;
            docMenu.ModalPresentationStyle = UIModalPresentationStyle.Popover;
            PresentViewController(docMenu, true, null);
        }

        void DocMenu_DidPickDocumentPicker(object sender, UIDocumentMenuDocumentPickedEventArgs e)
        {
            e.DocumentPicker.DidPickDocument += DocPicker_DidPickDocument;
            PresentViewController(e.DocumentPicker, true, null);
        }

        void DocPicker_DidPickDocument(object sender, UIDocumentPickedEventArgs e)
        {
            e.Url.StartAccessingSecurityScopedResource();
            DocName.Text = e.Url.LastPathComponent;
            DocPath.Text = e.Url.AbsoluteString;
            e.Url.StopAccessingSecurityScopedResource();
        }

        partial void OpenSelectedDoc_TouchUpInside(UIButton sender)
        {
            var url = NSUrl.FromString(DocPath.Text);
            var docPreview = UIDocumentInteractionController.FromUrl(url);
            docPreview.Name = DocName.Text;
            docPreview.ViewControllerForPreview = (controller) => this;
            docPreview.PresentPreview(true);
        }
    }
}
