using Xamarin.Forms;
using VideoConcept.Core;
using Plugin.Connectivity;

using System.Linq;
using System.Collections.Generic;
using VideoConcept.Messages;
using System.Diagnostics;
using VideoConcept.Core.Data;

namespace VideoConcept
{
	public partial class App : Application
	{
		bool _uploadProcessing;

		VideoConceptPage _videoConceptPage;

		public App(string environmentalDocumentPath)
		{
			InitializeComponent();

			Global.EnvironmentalDocumentsPath = environmentalDocumentPath;

			_videoConceptPage = new VideoConceptPage();

			MainPage = _videoConceptPage;

			CrossConnectivity.Current.ConnectivityTypeChanged += async (sender, e) =>
			{
				if (!_uploadProcessing && e.IsConnected && e.ConnectionTypes.Contains(Plugin.Connectivity.Abstractions.ConnectionType.WiFi))
				{
					_uploadProcessing = true;

					var videos = await VideoItemStore.Instance.GetVideoItems().ConfigureAwait(false);

					if (videos?.Count > 0)
					{
						var videoUploadRequestMessage = new VideoUploadRequestMessage
						{
							Videos = videos
						};

						MessagingCenter.Send(videoUploadRequestMessage, "UploadVideoRequest");
					}
					else
						_uploadProcessing = false;
				}
			};

			MessagingCenter.Subscribe<VideoUploadErrorMessage>(this, "UploadVideoError", (message) =>
			{
				Debug.WriteLine(message.Message);
				_uploadProcessing = false;
			});

			MessagingCenter.Subscribe<VideoUploadResponseMessage>(this, "UploadVideoResponse", (message) =>
			{
				Debug.WriteLine(message.Message);

				_videoConceptPage.Refresh();

				_uploadProcessing = false;
			});
		}

		protected override void OnStart()
		{
			// Handle when your app starts
		}

		protected override void OnSleep()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume()
		{
			// Handle when your app resumes
		}
	}
}
