// WARNING
//
// This file has been generated automatically by Xamarin Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace DocumentPickerTest.iOS
{
    [Register ("ViewController")]
    partial class ViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel DocName { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel DocPath { get; set; }

        [Action ("OpenMenu_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void OpenMenu_TouchUpInside (UIKit.UIButton sender);

        [Action ("OpenPicker_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void OpenPicker_TouchUpInside (UIKit.UIButton sender);

        [Action ("OpenSelectedDoc_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void OpenSelectedDoc_TouchUpInside (UIKit.UIButton sender);

        void ReleaseDesignerOutlets ()
        {
            if (DocName != null) {
                DocName.Dispose ();
                DocName = null;
            }

            if (DocPath != null) {
                DocPath.Dispose ();
                DocPath = null;
            }
        }
    }
}