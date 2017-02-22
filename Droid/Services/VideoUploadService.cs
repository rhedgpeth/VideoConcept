using Android.App;
using Android.Content;
using System.Threading.Tasks;
using Android.OS;
using System.Threading;
using Xamarin.Forms;
using VideoConcept.Messages;
using VideoConcept.Core.Data;
using System;
using System.IO;

namespace VideoConcept.Droid.Services
{
	[Service]
	public class VideoUploadService : Service
	{
		CancellationTokenSource _cts;

		public override IBinder OnBind(Intent intent)
		{
			return null;
		}

		public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
		{
			_cts = new CancellationTokenSource();

			Task.Run(async () =>
			{
				try
				{
					var paths = intent.GetStringExtra("VideoFilePathString").Split(',');;

					// Temporary demo purposes only
					foreach (var path in paths)
					{
						Console.WriteLine($"Uploading {path}...");

						var videoData = File.Open(path, FileMode.Open);

						await Task.Delay(2000);

						// TODO: Hook this up for Android, limited at the moment due to intent usage
						//await VideoItemStore.Instance.DeleteVideoItemAsync(video);

						Console.WriteLine("Upload Complete!");
					}

					// 1.) Pull the video bytes from the device
					// 2.) Mark the video file as pending upload
					// 3.) Upload the video
					// 4.) Upon successful upload remove the video from the sqlite table
					// 5.) Repeat process for all videos
				}
				catch (Exception ex)
				{
					var errorMessage = new VideoUploadErrorMessage
					{
						Message = "ERROR: " + ex.Message
					};

					Device.BeginInvokeOnMainThread(() => MessagingCenter.Send(errorMessage, "UploadVideoError"));

					// TODO: Log this? 
				}
				finally
				{
					if (_cts.IsCancellationRequested)
					{
						var responseMessage = new VideoUploadResponseMessage
						{
							Message = "Video Upload Canceled!"
						};

						Device.BeginInvokeOnMainThread(() => MessagingCenter.Send(responseMessage, "UploadVideoResponse"));
					}
				}

			}, _cts.Token);

			return StartCommandResult.Sticky;
		}

		public override void OnDestroy()
		{
			if (_cts != null)
			{
				_cts.Token.ThrowIfCancellationRequested();
				_cts.Cancel();
			}

			base.OnDestroy();
		}
	}
}
