using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using CrossCamera.Core;

namespace CrossCamera.Droid
{
	internal class DroidCamera : Core.Camera
	{
		readonly string _defaultVideoSaveDirectory = Android.OS.Environment.ExternalStorageDirectory.AbsolutePath + "/Movies/";

		void CheckVideoExtension(string path)
		{
			if (Path.GetExtension(path).ToLowerInvariant() != ".mp4")
			{
				throw new Exception("Invalid video extension.");
			}
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

		public override Task<Core.VideoFile> TakeVideoAsync(string path)
		{
			var taskSource = new TaskCompletionSource<Core.VideoFile>();

			if (Interlocked.CompareExchange(ref VideoCameraActivity.currentTaskSource, taskSource, null) != null)
			{
				throw new Exception("Camera is already in use.");
			}

			var fullPath = Path.Combine(_defaultVideoSaveDirectory, path);

			CheckVideoExtension(fullPath);

			if (File.Exists(fullPath))
			{
				throw new Exception("Video already exists.");
			}

			var context = Application.Context;

			var intent = new Intent(context, typeof(VideoCameraActivity));
			intent.AddFlags(ActivityFlags.NewTask);
			intent.PutExtra(VideoCameraActivity.ExtraPath, fullPath);

			context.StartActivity(intent);

			return VideoCameraActivity.currentTaskSource.Task;
		}
	}
}
