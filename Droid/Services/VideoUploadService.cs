using Android.App;
using Android.Content;
using System.Threading.Tasks;
using Android.OS;
using System.Threading;
using Xamarin.Forms;
using VideoConcept.Messages;
using System;
using VideoConcept.Core.Services;

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
