using System;
using System.Linq;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Xamarin.Forms;
using VideoConcept.Messages;
using VideoConcept.Droid.Services;

namespace VideoConcept.Droid
{
	[Activity(Label = "VideoConcept.Droid", Icon = "@drawable/icon", Theme = "@style/MyTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
	public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
	{
		protected override void OnCreate(Bundle bundle)
		{
			TabLayoutResource = Resource.Layout.Tabbar;
			ToolbarResource = Resource.Layout.Toolbar;

			base.OnCreate(bundle);

			CrossCamera.Droid.CrossCamera.Initialize();

			VideoConcept.Shared.Bootstrap.Init();

			Forms.Init(this, bundle);

			LoadApplication(new App(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal)));

			RegisterVideoUploadTask();
		}

		void RegisterVideoUploadTask()
		{
			MessagingCenter.Subscribe<App>(this, "UploadVideoRequest", (sender) =>
			{
				var intent = new Intent(this, typeof(VideoUploadService));
				StartService(intent);
			});
		}
	}
}
