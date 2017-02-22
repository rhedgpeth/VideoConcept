using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using UIKit;
using VideoConcept.Core.Data;
using VideoConcept.Messages;
using Xamarin.Forms;

namespace VideoConcept.iOS.Services
{
	public class VideoUploadService
	{
		nint _taskId;

		CancellationTokenSource _cts;

		public async Task Start(VideoUploadRequestMessage message)
		{
			_cts = new CancellationTokenSource();

			_taskId = UIApplication.SharedApplication.BeginBackgroundTask("VideoUploadTask", OnExpiration);

			try
			{
				// Temporary demo purposes only
				foreach (var video in message.Videos)
				{
					Console.WriteLine($"Uploading {video.Title}...");

					var videoData = File.Open(video.Path, FileMode.Open);

					await Task.Delay(2000);
					await VideoItemStore.Instance.DeleteVideoItemAsync(video);

					Console.WriteLine("Upload Complete!");
				}

				// 1.) Pull the video bytes from the device
				// 2.) Mark the video file as pending upload
				// 3.) Upload the video
				// 4.) Upon successful upload remove the video from the sqlite table
				// 5.) Repeat process for all videos
			}
			catch (OperationCanceledException ex)
			{
				var errorMessage = new VideoUploadErrorMessage
				{
					Message = "ERROR: " + ex.Message
				};

				Device.BeginInvokeOnMainThread(() => MessagingCenter.Send(message, "UploadVideoError"));

				// TODO: Log this? 
			}
			finally
			{
				if (_cts.IsCancellationRequested)
				{
					var responseMessage = new VideoUploadResponseMessage
					{
						Message = "Video Uploads Successful!"
					};

					Device.BeginInvokeOnMainThread(() => MessagingCenter.Send(responseMessage, "UploadVideoResponse"));
				}
			}

			UIApplication.SharedApplication.EndBackgroundTask(_taskId);
		}

		public void Stop()
		{
			_cts.Cancel();
		}

		void OnExpiration()
		{
			_cts.Cancel();
		}
	}
}
