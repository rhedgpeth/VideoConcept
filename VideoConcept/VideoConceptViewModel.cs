using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace VideoConcept
{
	public class VideoConceptViewModel
	{
		readonly Func<Task<string>> _captureVideo;
		readonly Action<string> _uploadVideo;

		public VideoConceptViewModel(Func<Task<string>> captureVideo, Action<string> uploadVideo)
		{
			_captureVideo = captureVideo;
			_uploadVideo = uploadVideo;
			Videos = new ObservableCollection<VideoItemViewModel>();
		}

		public ObservableCollection<VideoItemViewModel> Videos { get; private set; }

		public Command CaptureVideoCommand => new Command(async () =>
		{
			var path = await _captureVideo();
			if (!string.IsNullOrEmpty(path))
			{
				Videos.Add(new VideoItemViewModel(path, path, _uploadVideo, removeFromVideos: vm =>
				{
					Videos.Remove(vm);
				}));
			}
		});
	}
}
