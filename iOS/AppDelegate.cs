using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using UIKit;
using VideoConcept.iOS.Services;
using VideoConcept.Messages;
using Xamarin.Forms;

namespace VideoConcept.iOS
{
	[Register("AppDelegate")]
	public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
	{
		public override bool FinishedLaunching(UIApplication app, NSDictionary options)
		{
			CrossCamera.iOS.CrossCamera.Initialize();

			Forms.Init();

			LoadApplication(new App(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)));

			RegisterVideoUploadTask();

			return base.FinishedLaunching(app, options);
		}

		void RegisterVideoUploadTask()
		{
			MessagingCenter.Subscribe<VideoUploadRequestMessage>(this, "UploadVideoRequest", async (message) =>
			{
				var videoUploadService = new VideoUploadService();
				await videoUploadService.Start(message);
			});
		}
	}
}
