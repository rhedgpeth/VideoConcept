
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Provider;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Java.IO;

namespace CrossCamera.Droid
{
	[Activity(Label = "VideoCameraActivity")]
	internal class VideoCameraActivity : Activity
	{
		public const string ExtraPath = "VideoPath";

		public static TaskCompletionSource<Core.VideoFile> currentTaskSource;

		File _currentFile;
		Core.VideoFile _result;

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			Start();
		}

		void Start()
		{
			string path = this.Intent.GetStringExtra(ExtraPath);

			var intent = new Intent(MediaStore.ActionVideoCapture);

			_currentFile = new File(path);

			intent.PutExtra(MediaStore.ExtraOutput, Android.Net.Uri.FromFile(_currentFile));
			StartActivityForResult(intent, 0);
		}

		protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
		{
			base.OnActivityResult(requestCode, resultCode, data);

			if (resultCode != Result.Canceled)
			{
				var path = _currentFile.Path;

				_currentFile.Dispose();

				_result = Core.InternalVideoFile.Open(path);
			}
			else
			{
				_currentFile.Dispose();
			}

			Finish();
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();

			var taskSource = currentTaskSource;
			currentTaskSource = null;
			taskSource.SetResult(_result);
		}
	}
}
