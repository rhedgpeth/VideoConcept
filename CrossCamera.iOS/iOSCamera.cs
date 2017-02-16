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
	internal class iOSCamera : Core.Camera
	{
		internal static TaskCompletionSource<VideoFile> currentTaskSource = null;

		readonly string _defaultVideoSaveDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

		void CheckVideoExtension(string path)
		{
			if (Path.GetExtension(path).ToLowerInvariant() != ".mov")
			{
				throw new Exception("Invalid video extension.");
			}
		}

		// force extension to be .mov if it was a .mp4
		string CreateNewPath(string path)
		{
			var newPath = "";
			if (Path.GetExtension(path).ToLowerInvariant() == ".mp4")
			{
				newPath = Path.ChangeExtension(path, "mov");
			}
			else
			{
				newPath = path;
			}
			return newPath;
		}

		public override string DefaultVideoSaveDirectory
		{
			get
			{
				return _defaultVideoSaveDirectory;
			}
		}

		public override void DeleteVideoFile(VideoFile videoFile)
		{
			var path = videoFile.Path;
			videoFile.Dispose();
			InternalVideoFile.Delete(path);
		}

		public override VideoFile OpenVideoFile(string fullPath)
		{
			CheckVideoExtension(fullPath);

			return InternalVideoFile.Open(fullPath);
		}

		public override Task<VideoFile> TakeVideoAsync(string path)
		{
			var taskSource = new TaskCompletionSource<Core.VideoFile>();

			if (Interlocked.CompareExchange(ref currentTaskSource, taskSource, null) != null)
			{
				throw new Exception("Camera is already in use.");
			}

			var newPath = CreateNewPath(path);

			var fullPath = Path.Combine(_defaultVideoSaveDirectory, newPath);

			CheckVideoExtension(fullPath);

			if (File.Exists(fullPath))
			{
				throw new Exception("Video already exists.");
			}

			var picker = new UIImagePickerController();
			picker.ImagePickerControllerDelegate = new iOSCameraDelegate(fullPath);
			picker.MediaTypes = new string[] { UTType.Movie };
			picker.SourceType = UIImagePickerControllerSourceType.Camera;
			picker.CameraCaptureMode = UIImagePickerControllerCameraCaptureMode.Video;

			var vc = UIApplication.SharedApplication.KeyWindow.RootViewController;

			while (vc.PresentedViewController != null)
			{
				vc = vc.PresentedViewController;
			}

			vc.PresentViewController(picker, true, () => { });

			return currentTaskSource.Task;
		}
	}
}
