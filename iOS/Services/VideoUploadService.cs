using System;
using System.Threading;
using System.Threading.Tasks;
using UIKit;
using VideoConcept.Messages;
using Xamarin.Forms;
using VideoConcept.Core.Services;

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
				// Option #1: Using PCL project service with Dependency Injection
				await MediaService.Instance.UploadPendingVideos();
			}
			catch (OperationCanceledException ex)
			{
				var errorMessage = new VideoUploadResponseMessage
				{
					HasErrors = true,
					Message = "ERROR: " + ex.Message
				};

				Device.BeginInvokeOnMainThread(() => MessagingCenter.Send(errorMessage, "UploadVideoResponse"));
			}
			finally
			{
				var responseMessage = new VideoUploadResponseMessage();

				if (_cts.IsCancellationRequested)
					responseMessage.Message = "Video Upload Canceled!";
				else
					responseMessage.Message = "Video(s) Uploaded Successfully!";

				Device.BeginInvokeOnMainThread(() => MessagingCenter.Send(responseMessage, "UploadVideoResponse"));
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
