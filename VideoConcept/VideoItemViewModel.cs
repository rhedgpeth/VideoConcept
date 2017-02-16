using System;
using Xamarin.Forms;
using CrossCamera.Core;
using System.Threading.Tasks;

namespace VideoConcept
{
	public class VideoItemViewModel
	{
		readonly Camera _camera;
		readonly Func<string, string, string, Task> _displayAlert;
		readonly Action<VideoItemViewModel> _removeFromVideos;

		public VideoItemViewModel(
			string title, 
			string path, 
			Camera camera, 
			Func<string, string, string, Task> displayAlert, 
			Action<VideoItemViewModel> removeFromVideos)
		{
			_camera = camera;
			_displayAlert = displayAlert;
			_removeFromVideos = removeFromVideos;
			Title = title;
			Path = path;
		}

		public string Title { get; private set; }
		public string Path { get; private set; }

		public Command UploadVideoCommand => new Command(async () =>
		{
			var videoFile = _camera.OpenVideoFile(Path);
			_camera.DeleteVideoFile(videoFile);
			await _displayAlert("Video Uploaded!", "Congratulations! You have uploaded a video!", "Ok");
			_removeFromVideos(this);
		});
	}
}
