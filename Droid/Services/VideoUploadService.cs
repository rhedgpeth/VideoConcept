using Android.App;
using Android.Content;
using System.Threading.Tasks;
using Android.OS;
using System.Threading;
using Xamarin.Forms;
using VideoConcept.Messages;
using System;
using VideoConcept.Shared.Services;

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
					await MediaUploadService.Instance.UploadVideos();
				}
				catch (Exception ex)
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
