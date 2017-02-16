using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using CrossCamera.Core;
using Foundation;
using MobileCoreServices;
using UIKit;

namespace CrossCamera.iOS
{
	internal class iOSCameraDelegate : UIImagePickerControllerDelegate
	{
		readonly string _fullPath;

		public iOSCameraDelegate(string fullPath)
		{
			_fullPath = fullPath;
		}

		public override void Canceled(UIImagePickerController picker)
		{
			picker.DismissViewController(true, () =>
			{
				var taskSource = iOSCamera.currentTaskSource;
				iOSCamera.currentTaskSource = null;
				taskSource.SetResult(null);
			});
		}

		public override void FinishedPickingMedia(UIImagePickerController picker, NSDictionary info)
		{
			var taskSource = iOSCamera.currentTaskSource;

			VideoFile result = null;

			var type = info[new NSString("UIImagePickerControllerMediaType")] as NSString;
			if (type != null && type == UTType.Movie)
			{
				var url = info[new NSString("UIImagePickerControllerMediaURL")] as NSUrl;
				if (url != null)
				{
					File.Move(url.Path, _fullPath);
					result = InternalVideoFile.Open(_fullPath);
				}
			}

			picker.DismissViewController(true, () =>
			{
				iOSCamera.currentTaskSource = null;
				taskSource.SetResult(result);
			});
		}
	}
}
