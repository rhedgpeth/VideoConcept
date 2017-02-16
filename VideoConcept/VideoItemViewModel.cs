using System;
using Xamarin.Forms;
using CrossCamera.Core;
using System.Threading.Tasks;
using System.Windows.Input;

namespace VideoConcept
{
	public class VideoItemViewModel
	{
		readonly Camera _camera;
		readonly Func<string, string, string, Task> _displayAlert;
		readonly Action<VideoItemViewModel> _removeFromVideos;

		public VideoItemViewModel(
			VideoItem videoItem,
			Camera camera, 
			Func<string, string, string, Task> displayAlert, 
			Action<VideoItemViewModel> removeFromVideos)
		{
			VideoItem = videoItem;
			_camera = camera;
			_displayAlert = displayAlert;
			_removeFromVideos = removeFromVideos;
		}

		VideoItem _videoItem;
		public VideoItem VideoItem
		{
			get
			{
				return _videoItem;
			}
			set
			{
				_videoItem = value;
				// mapping
				Title = value.Title;
				Path = value.Path;
			}
		}
		public string Title { get; private set; }

		public string Path { get; private set; }

		public ICommand UploadVideoCommand => new Command(async () =>
		{
			var videoFile = _camera.OpenVideoFile(Path);

			_camera.DeleteVideoFile(videoFile);

			await _displayAlert("Video Uploaded!", "Congratulations! You have uploaded a video!", "Ok");

			_removeFromVideos(this);
		});
	}
}
