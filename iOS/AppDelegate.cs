using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using UIKit;

namespace VideoConcept.iOS
{
	[Register("AppDelegate")]
	public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
	{
		public override bool FinishedLaunching(UIApplication app, NSDictionary options)
		{
			CrossCamera.iOS.CrossCamera.Initialize();

			global::Xamarin.Forms.Forms.Init();

			LoadApplication(new App(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)));

			return base.FinishedLaunching(app, options);
		}
	}
}
