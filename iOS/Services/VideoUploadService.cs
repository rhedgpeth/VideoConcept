using System;
using System.Threading;
using System.Threading.Tasks;
using UIKit;
using VideoConcept.Core.Services;
using VideoConcept.Messages;
using Xamarin.Forms;

namespace VideoConcept.iOS.Services
{
	public class VideoUploadService
	{
		nint _taskId;

		CancellationTokenSource _cts;

		public async Task Start()
		{
			_cts = new CancellationTokenSource();

			_taskId = UIApplication.SharedApplication.BeginBackgroundTask("VideoUploadTask", OnExpiration);

			try
			{
				await MediaUploadService.Instance.UploadVideos();
			}
			catch (OperationCanceledException ex)
			{
				var errorMessage = new VideoUploadErrorMessage
				{
					Message = "ERROR: " + ex.Message
				};

				Device.BeginInvokeOnMainThread(() => MessagingCenter.Send(errorMessage, "UploadVideoError"));
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
