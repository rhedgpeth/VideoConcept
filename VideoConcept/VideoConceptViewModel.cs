using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using CrossCamera.Core;

namespace VideoConcept
{
	public class VideoConceptViewModel
	{
		readonly Camera _camera;
		readonly Func<string, string, string, Task> _displayAlert;
		readonly ObservableCollection<VideoItemViewModel> _videos = new ObservableCollection<VideoItemViewModel>();

		public VideoConceptViewModel(Camera camera, Func<string, string, string, Task> displayAlert)
		{
			_camera = camera;
			_displayAlert = displayAlert;
		}

		public ObservableCollection<VideoItemViewModel> Videos
		{
			get
			{
				return _videos;
			}
		}

		public Command CaptureVideoCommand => new Command(async () =>
		{
			try
			{
				var vf = _camera.OpenVideoFile(_camera.DefaultVideoSaveDirectory + "/test.mov");
				_camera.DeleteVideoFile(vf);
			}
			catch
			{
			}

			try
			{
				var vf = _camera.OpenVideoFile(_camera.DefaultVideoSaveDirectory + "/test.mp4");
				_camera.DeleteVideoFile(vf);
			}
			catch
			{
			}

			var videoFile = await _camera.TakeVideoAsync("test.mp4");

			if (videoFile != null)
			{
				var path = videoFile.Path;
				videoFile.Dispose();
				Videos.Add(new VideoItemViewModel(path, path, _camera, _displayAlert, removeFromVideos: vm =>
				{
					Videos.Remove(vm);
				}));
			}
		});
	}
}
