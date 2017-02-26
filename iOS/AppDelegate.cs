using System;
using Foundation;
using UIKit;
using VideoConcept.iOS.Services;
using Xamarin.Forms;

namespace VideoConcept.iOS
{
	[Register("AppDelegate")]
	public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
	{
		public override bool FinishedLaunching(UIApplication app, NSDictionary options)
		{
			CrossCamera.iOS.CrossCamera.Initialize();

			VideoConcept.Shared.Bootstrap.Init();

			Forms.Init();

			LoadApplication(new App(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)));

			RegisterVideoUploadTask();

			return base.FinishedLaunching(app, options);
		}

		void RegisterVideoUploadTask()
		{
			MessagingCenter.Subscribe<App>(this, "UploadVideoRequest", async (sender) =>
			{
				var videoUploadService = new VideoUploadService();
				await videoUploadService.Start();
			});
		}
	}
}
