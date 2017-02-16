using Xamarin.Forms;
using CrossCamera.Core;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace VideoConcept
{
	public partial class VideoConceptPage : ContentPage
	{
		public VideoConceptPage()
		{
			InitializeComponent();

			Action<string> uploadVideo = async p =>
			{
				var vf = Camera.Current.OpenVideoFile(p);
				Camera.Current.DeleteVideoFile(vf);
				await DisplayAlert("Video Uploaded!", "Congratulations! You have uploaded a video!", "Ok");
			};

			Func<Task<string>> captureVideo = async () =>
			{
				try
				{
					var vf = Camera.Current.OpenVideoFile(Camera.Current.DefaultVideoSaveDirectory + "/test.mov");
					Camera.Current.DeleteVideoFile(vf);
				}
				catch
				{
				}

				try
				{
					var vf = Camera.Current.OpenVideoFile(Camera.Current.DefaultVideoSaveDirectory + "/test.mp4");
					Camera.Current.DeleteVideoFile(vf);
				}
				catch
				{
				}

				var videoFile = await Camera.Current.TakeVideoAsync("test.mp4").ConfigureAwait(false);

				if (videoFile != null)
				{
					var path = videoFile.Path;
					videoFile.Dispose();
					return path;
				}

				return null;
			};

			var vm = new VideoConceptViewModel(captureVideo, uploadVideo);

			this.BindingContext = vm;
		}
	}
}
