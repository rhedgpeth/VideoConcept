using Xamarin.Forms;
using CrossCamera.Core;

namespace VideoConcept
{
	public partial class VideoConceptPage : ContentPage
	{
		public VideoConceptPage()
		{
			InitializeComponent();

			this.CaptureVideoButton.Clicked += async (sender, e) =>
			{
				try
				{
					var vf = Camera.Current.OpenVideoFile(Camera.Current.DefaultVideoSaveDirectory + "/test.mov");
					Camera.Current.DeleteVideoFile(vf);

					vf = Camera.Current.OpenVideoFile(Camera.Current.DefaultVideoSaveDirectory + "/test.mp4");
					Camera.Current.DeleteVideoFile(vf);
				}
				catch
				{
				}

				var videoFile = await Camera.Current.TakeVideoAsync("test.mp4");

				if (videoFile != null)
				{
					videoFile.Dispose();
				}
			};
		}
	}
}
